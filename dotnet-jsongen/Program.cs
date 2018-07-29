using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Templates.Tools
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("missing template path");
                return;
            }
            Task.WhenAll(Directory.GetFiles(args[0], "*.cshtml", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .SelectMany(x => x.Directory.GetFiles("*.json", SearchOption.TopDirectoryOnly)
                    .Select(CreateTemlpate<DynamicTemplateBase>(x).RunTemplateAsync)
                    .ToArray())
                .ToArray()).Wait();
        }

        private static T CreateTemlpate<T>(FileInfo file)
        {

            var result = new ServiceCollection().AddRazorTemlpate(
                builder => builder.AddDefaultImports(
                    "@using Templates.Tools", 
                    "@using Newtonsoft.Json", 
                    "@using Newtonsoft.Json.Linq"))
                .BuildServiceProvider()
                .GetRequiredService<ITemplateFactory>()
                .Create(File.ReadAllText(file.FullName));
            if (result.Message.Any())
                throw new Exception(result.Message);
            else
                return (T)Activator.CreateInstance(result.TemplateType);
        }
    }
}
