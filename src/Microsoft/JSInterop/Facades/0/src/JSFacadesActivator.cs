using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssignments;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivator : IJSFacadesActivator
    {
        private readonly IJSFacadeResolver jsFacadeResolver;
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;
        private readonly List<IComponentPropertyAssignment> componentPropertyAssignments;

        public JSFacadesActivator(
            IJSFacadeResolver jsFacadeResolver,
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            JSFacadesActivatorOptions options)
        {
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
            this.jsModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            componentPropertyAssignments = options.ComponentPropertyAssignments;
        }

        private JSFacades CreateFacades() =>
            new JSFacades(jsFacadeResolver, jsModuleActivator, jsLocalObjectActivator);

        public IJSFacades CreateInstance() =>
            CreateFacades();

        public async ValueTask<IJSFacades> CreateInstanceAsync(object component)
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacadesDisposables = new List<IAsyncDisposable>();

            foreach (var componentProperty in ComponentPropertyInfoCollection.Create(component.GetType())) {
                foreach (var componentPropertyAssignment in componentPropertyAssignments) {
                    if ((await componentPropertyAssignment.TryAssignComponentProperty(componentProperty, jsFacadeResolver)).TryGetNotNull(out var jsFacade)) {
                        jsFacadesDisposables.Add(jsFacade);
                        componentProperty.PropertyInfo.SetValue(component, jsFacade);
                        continue;
                    }
                }
            }

            //foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(component.GetType())) {
            //    if (!ComponentPropertyUtils.TryGetModuleNameOrPath(propertyInfo, out var pathRelativeToWwwRoot)) {
            //        continue;
            //    }

            //    var jsModule = await jsFacadeResolver.CreateModuleFacadeAsync(pathRelativeToWwwRoot, propertyInfo.PropertyType);
            //    
            //}

            var jsFacades = CreateFacades();
            jsFacades.RegisterAsyncDisposables(jsFacadesDisposables);
            return jsFacades;
        }
    }
}
