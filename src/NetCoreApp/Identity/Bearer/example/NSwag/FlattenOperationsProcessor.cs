using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Teronis.Identity.Bearer.NSwag
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
