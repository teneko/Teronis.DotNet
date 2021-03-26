// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Teronis.AspNetCore.Identity.Bearer.Controllers
{
    [Route("")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;

        public RoutesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) =>
            this.actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;

        [HttpGet("routes")]
        public RootResultModel Get()
        {
            var routes = actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
                ad => ad.AttributeRouteInfo != null).Select(ad => new RouteModel {
                    Name = ad.AttributeRouteInfo?.Template ?? string.Empty,
                    Method = ad.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.First(),
                }).ToList();

            var rootResultModel = new RootResultModel {
                Routes = routes
            };

            return rootResultModel;
        }

        public class RouteModel
        {
            public string Name { get; set; } = null!;
            public string? Method { get; set; }
        }

        public class RootResultModel
        {
            public List<RouteModel> Routes { get; set; } = null!;
        }
    }
}
