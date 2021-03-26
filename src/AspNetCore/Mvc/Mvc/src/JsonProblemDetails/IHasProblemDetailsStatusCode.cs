// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.JsonProblemDetails
{
    /// <summary>
    /// If implemented in mappable objects, then the <see cref="StatusCode"/> 
    /// takes precendence over all other defined status codes.
    /// </summary>
    public interface IHasProblemDetailsStatusCode
    {
        /// <summary>
        /// The status code that takes precendence over all other defined status codes.
        /// </summary>
        public int StatusCode { get; }
    }
}
