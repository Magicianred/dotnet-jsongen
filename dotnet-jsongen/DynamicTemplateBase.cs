using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Templates.Tools
{
    public abstract class DynamicTemplateBase : TemplateBase
    {
        public string Extension { get; set; }
        public dynamic Model { get; set; }
        public string OutputPath { get; set; }
        public virtual Task RunTemplateAsync(FileInfo file)
        {
            var jobj = JsonConvert.DeserializeObject(File.ReadAllText(file.FullName));
            return (jobj is JArray array
                ? (Func<Action<dynamic>,Task>)(config => RunArrayTempalteAsync(array.AsEnumerable().OfType<JObject>(), config))
                : config => Clone().RunObjectAsync(jobj, config))
                .Invoke(Configure(file));
        }

        private Action<dynamic> Configure(FileInfo file)
            => model =>
            {
                model.OutputPath = file.Directory.FullName;
                var name = file.Name.Replace(".json", string.Empty).Replace("_", ".");
                model.Name = model.Name ?? name.Substring(name.LastIndexOf(".") + 1).ToString();
                model.Namespace = name.ToString().EndsWith((string)model.Name, StringComparison.OrdinalIgnoreCase)
                    ? name.Substring(0, name.LastIndexOf(".")).ToString()
                    : name.ToString();
                model.Extensions = ".json";
            };

        private Task RunArrayTempalteAsync(IEnumerable<JObject> objs,Action<dynamic> config)
            => Task.WhenAll(objs.Select(x => Clone().RunObjectAsync(x,config)).ToArray());

        async private Task RunObjectAsync(dynamic obj, Action<dynamic> config)
        {
            Model = obj;
            config(Model);
            Output = new StringWriter();
            await ExecuteAsync();
            WriteToFile(
                Path.Combine((string)Model.OutputPath, (string)(Model.Name + Extension)),
                Output.ToString());
        }

        private DynamicTemplateBase Clone()
            => (DynamicTemplateBase)Activator.CreateInstance(GetType());

        private void WriteToFile(string path,string content)
            => File.WriteAllText(path, content);
    }
}
