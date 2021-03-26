// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public interface IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> : IServiceResultPostConfiguration
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> WithStatusCode(int? statusCode);
    }
}
