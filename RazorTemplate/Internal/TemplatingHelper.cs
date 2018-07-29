// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace Templates
{

    internal static class TemplatingHelper
    {
        public static void Register(RazorProjectEngineBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            //InjectDirective.Register(builder);
            //ModelDirective.Register(builder);
            //NamespaceDirective.Register(builder);

            FunctionsDirective.Register(builder);
            InheritsDirective.Register(builder);
            SectionDirective.Register(builder);

            builder.AddDefaultImports(new string[] {
                "@using System",
                "@using System.Linq",
                "@using System.Threading.Tasks",
                "@Templates"
            });

            //builder.Features.Add(new DefaultTagHelperDescriptorProvider());
            //builder.Features.Add(new ModelExpressionPass());
            //builder.Features.Add(new PagesPropertyInjectionPass());
            //builder.Features.Add(new RazorPageDocumentClassifierPass());
            //builder.Features.Add(new AssemblyAttributeInjectionPass());
            //builder.Features.Add(new InstrumentationPass());
        }
    }
}
