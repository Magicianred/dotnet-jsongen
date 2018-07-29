// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Templates
{
    public class TemplateProcessingException : Exception
    {
        public TemplateProcessingException(IEnumerable<string> messages, string generatedCode)
            : base(FormatMessage(messages))
        {
            Messages = messages ?? Array.Empty<string>();
            GeneratedCode = generatedCode ?? string.Empty;
        }

        public string GeneratedCode { get; private set; }

        public IEnumerable<string> Messages { get; private set; } = Array.Empty<string>();

        public override string Message => string.Format($"TemplateProcessingError\r\n{FormatMessage(Messages)}");

        private static string FormatMessage(IEnumerable<string> messages) => String.Join(Environment.NewLine, messages);
    }
}