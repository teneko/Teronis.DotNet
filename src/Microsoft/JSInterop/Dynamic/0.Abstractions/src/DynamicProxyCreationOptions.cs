using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class DynamicProxyCreationOptions
    {
        /// <summary>
        /// The methods you don't want have proxied. Only members of a derived interface of <see cref="IJSObjectReferenceFacade"/>
        /// are taken into regard. Properties are not proxied naturally. You are in responsiblity to implement them. 
        /// </summary>
        public IReadOnlySet<string>? MethodsNotProxied { get; set; }
        public IReadOnlySet<Type>? InterfaceTypesNotProxied { get; set; }
    }
}
