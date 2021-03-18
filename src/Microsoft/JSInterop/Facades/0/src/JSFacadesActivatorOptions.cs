using System;
using System.Collections.Generic;
using Teronis.Collections.Specialized;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssignments;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivatorOptions
    {
        /// <summary>
        /// Contains default component property assignments:
        /// <br/>- <see cref="JSModuleComponentAssignment"/>
        /// </summary>
        public OrderedDictionary<Type, Func<IServiceProvider, IComponentPropertyAssignment>> ComponentPropertyAssignmentFactories { get; }

        internal List<IComponentPropertyAssignment> ComponentPropertyAssignments;

        public JSFacadesActivatorOptions()
        {
            ComponentPropertyAssignmentFactories = new OrderedDictionary<Type, Func<IServiceProvider, IComponentPropertyAssignment>>();
            ComponentPropertyAssignments = new List<IComponentPropertyAssignment>();
            this.AddDefaultComponentPropertyAssignments();
        }
    }
}
