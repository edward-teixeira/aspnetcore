// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components.WebAssembly.Infrastructure;

/// <summary>
/// Contains methods called by interop. Intended for framework use only, not supported for use in application
/// code.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class JSInteropMethods
{
    /// <summary>
    /// For framework use only.
    /// </summary>
    [JSInvokable(nameof(NotifyLocationChanged))]
    public static void NotifyLocationChanged(string uri, bool isInterceptedLink)
    {
        WebAssemblyNavigationManager.Instance.SetLocation(uri, isInterceptedLink);
    }
}
