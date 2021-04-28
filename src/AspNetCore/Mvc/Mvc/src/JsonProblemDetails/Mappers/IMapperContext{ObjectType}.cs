// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers
{
    public interface IMapperContext<out MappableObjectType> : IMapperContext
    {
        MappableObjectType MappableObject { get; }
    }
}
