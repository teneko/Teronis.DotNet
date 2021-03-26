// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Teronis.AspNetCore.Identity.Bearer.NSwag
{
    public class FlattenOperationsProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.OperationId = $"{context.MethodInfo.Name}";
            return true;
        }
    }
}
