// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Templates
{

    public class TemplateFactoryResult
    {
        public static TemplateFactoryResult Ok(Type type)
            => new TemplateFactoryResult(type);
        public static TemplateFactoryResult Error(string message)
            => new TemplateFactoryResult(message);
        public TemplateFactoryResult(string message) => Message = message;

        public TemplateFactoryResult(Type type) => TemplateType = type;
        public string Message { get; } = string.Empty;
        public Type TemplateType { get; }
    }
}
