using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.JsonProblemDetails.Reflection
{
    public static class ParameterEvaluationExtensions
    {
        public static bool TryGetMapperContextParameterEvaluation(this ParameterEvaluation parameterEvaluation,
            [MaybeNullWhen(false)] out MapperContextParameterEvaluation mapperContextParameterEvaluation)
        {
            if (parameterEvaluation is null) {
                goto exit;
            }

            if (parameterEvaluation.IsMapperContextParameter) {
                mapperContextParameterEvaluation = (MapperContextParameterEvaluation)parameterEvaluation;
                return true;
            }

            exit:
            mapperContextParameterEvaluation = null;
            return false;
        }
    }
}
