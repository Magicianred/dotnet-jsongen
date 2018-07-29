// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Templates
{
    public abstract class TemplateBase
    {
        public TextWriter Output { get; set; } = new StringWriter();
        public abstract Task ExecuteAsync();

        public void WriteLiteral(object value) => WriteLiteralTo(Output, value);

        public virtual void WriteLiteralTo(TextWriter writer, object text)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (text != null)
            {
                writer.Write(text.ToString());
            }
        }

        public virtual void Write(object value) => WriteTo(Output, value);

        public virtual void WriteTo(TextWriter writer, object content)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (content != null)
            {
                writer.Write(content.ToString());
            }
        }

        public virtual void BeginContext(int position, int length, bool x) { }
        public virtual void EndContext() { }
    }
}