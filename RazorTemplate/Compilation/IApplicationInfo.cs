﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace Templates.Compilation
{
    public interface IApplicationInfo
    {
        string ApplicationBasePath { get; }
        string ApplicationName { get; }
        string ApplicationConfiguration { get; }
    }
}
