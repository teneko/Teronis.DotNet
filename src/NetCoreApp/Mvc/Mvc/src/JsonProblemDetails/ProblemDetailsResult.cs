using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class ProblemDetailsResult : ObjectResult
    {
        /// <summary>
        /// The problem details that is about to be serialized.
        /// </summary>
        public new ProblemDetails Value {
            get => (ProblemDetails)base.Value ?? ProblemDetailsUtils.CreateDefault("A problem occured.");

            set {
                base.Value = value;
                onValueChanged();
            }
        }

        public ProblemDetailsResult(ProblemDetails? problemDetails)
            : base(problemDetails)
        {
            onValueChanged();
            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));
        }

        private void onValueChanged()
        {
            var problemDetails = Value;
            StatusCode = problemDetails.Status;
            DeclaredType = problemDetails.GetType();
        }
    }
}
