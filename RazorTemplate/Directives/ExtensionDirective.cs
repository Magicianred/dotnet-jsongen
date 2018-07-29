// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Primitives;

namespace Templates
{

    public static class ExtensionDirective
    {
        public const string TOKEN = "extension";
        public static readonly DirectiveDescriptor Directive = DirectiveDescriptor.CreateDirective(
            TOKEN,
            DirectiveKind.SingleLine,
            builder =>
            {
                builder.AddStringToken(TOKEN, "output file extensions");
                builder.Usage = DirectiveUsage.FileScopedSinglyOccurring;
                builder.Description = "output file extensions";
            });

        public static RazorProjectEngineBuilder Register(RazorProjectEngineBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddDirective(Directive);
            builder.Features.Add(new ExtensionPass());
            return builder;
        }

        private static string GetExtension(DocumentIntermediateNode document, Visitor visitor)
        {
            visitor.Visit(document);

            for (var i = visitor.ExtensionDirectives.Count - 1; i >= 0; i--)
            {
                var directive = visitor.ExtensionDirectives[i];

                var tokens = directive.Tokens.ToArray();
                if (tokens.Length >= 1)
                {
                    return tokens[0].Content;
                }
            }

            return string.Empty;
        }

        internal class ExtensionPass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
        {
            // Runs after the @inherits directive
            public override int Order => 5;

            protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
            {
                var visitor = new Visitor();
                var output = GetExtension(documentNode, visitor);



                documentNode.FindPrimaryClass().Children.Insert(0, new IntermediateToken
                {
                    Content = $"public override string Extension => {output};",
                    Kind = TokenKind.CSharp
                });
                
            }
        }

        private class Visitor : IntermediateNodeWalker
        {
            public NamespaceDeclarationIntermediateNode Namespace { get; private set; }

            public ClassDeclarationIntermediateNode Class { get; private set; }

            public IList<DirectiveIntermediateNode> ExtensionDirectives { get; } = new List<DirectiveIntermediateNode>();

            public override void VisitNamespaceDeclaration(NamespaceDeclarationIntermediateNode node)
            {
                if (Namespace == null)
                {
                    Namespace = node;
                }

                base.VisitNamespaceDeclaration(node);
            }

            public override void VisitClassDeclaration(ClassDeclarationIntermediateNode node)
            {
                if (Class == null)
                {
                    Class = node;
                }

                base.VisitClassDeclaration(node);
            }

            public override void VisitDirective(DirectiveIntermediateNode node)
            {
                if (node.Directive == Directive)
                {
                    ExtensionDirectives.Add(node);
                }
            }
        }
    }
}
