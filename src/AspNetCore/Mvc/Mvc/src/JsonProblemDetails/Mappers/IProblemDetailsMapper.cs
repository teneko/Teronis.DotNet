// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    public interface IProblemDetailsMapper
    {
        bool CanMap();
        ProblemDetails CreateProblemDetails();
    }
}
