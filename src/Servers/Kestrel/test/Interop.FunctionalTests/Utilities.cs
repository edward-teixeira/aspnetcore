// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Testing;

namespace Interop.FunctionalTests;

internal static class Utilities
{
    internal static bool CurrentPlatformSupportsHTTP2OverTls()
    {
        return // "Missing Windows ALPN support: https://en.wikipedia.org/wiki/Application-Layer_Protocol_Negotiation#Support" or missing compatible ciphers (Win8.1)
            new MinimumOSVersionAttribute(OperatingSystems.Windows, WindowsVersions.Win10).IsMet
            // "Missing SslStream ALPN support: https://github.com/dotnet/runtime/issues/27727"
            && new OSSkipConditionAttribute(OperatingSystems.MacOSX).IsMet;
    }
}
