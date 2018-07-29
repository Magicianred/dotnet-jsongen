// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Templates.Compilation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Templates
{
    internal class TemplateFactory : ITemplateFactory
    {
        private readonly ICompilationService _compilationService;
        private readonly RazorProjectEngine _engine;
        private readonly ILogger<ITemplateFactory> _logger;

        public TemplateFactory(
            RazorProjectEngine engine,
            ICompilationService compilationService,
            ILogger<ITemplateFactory> logger)
        {
            _compilationService = compilationService ?? throw new ArgumentNullException(nameof(compilationService));
            _engine = engine;
            _logger = logger;
        }

        public TemplateFactoryResult Create(string content)
        {
            var result = _engine.Process(new TemplateRazorProjectItem(content));
            return result.GetCSharpDocument().Diagnostics.Any()
                    ? GenerateError(result)
                    : Compile(result);
        }
        private TemplateFactoryResult Compile(RazorCodeDocument document)
        {
            var content = document.GetCSharpDocument().GeneratedCode;
            var result = _compilationService.Compile(content);
            return result.Messages.Any() ? CompilationError(document, result) : TemplateFactoryResult.Ok(result.CompiledType);
        }

        private TemplateFactoryResult GenerateError(RazorCodeDocument document)
        {
            var csdocs = document.GetCSharpDocument();

            return TemplateFactoryResult.Error(new StringValues(new string[]
            {
                document.Source.FilePath + ".error",
                string.Join(
                    Environment.NewLine,
                        csdocs.Diagnostics.Select(d => d.GetMessage())),
                csdocs.GeneratedCode
            }).ToString());
        }

        private TemplateFactoryResult CompilationError(RazorCodeDocument document, CompilationResult result)
        {
            var csdocs = document.GetCSharpDocument();
            return TemplateFactoryResult.Error(new StringValues(new string[]
            {
                document.Source.FilePath + ".error",
                string.Join(
                    Environment.NewLine,
                          result.Messages),
                csdocs.GeneratedCode
            }).ToString());
        }
    }
}
