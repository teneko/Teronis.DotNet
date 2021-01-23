namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    public interface IMapperContext
    {
        /// <summary>
        /// This is the status code that has been provided by
        /// <see cref="IHasProblemDetailsStatusCode.StatusCode"/>.
        /// </summary>
        public int? StatusCode { get; }
        public ProblemDetailsFactoryScoped ProblemDetailsFactory { get; }
    }
}
