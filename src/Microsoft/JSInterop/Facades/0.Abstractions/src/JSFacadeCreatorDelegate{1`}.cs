﻿using System;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSLocalObject module)
        where T : class, IAsyncDisposable;
}
