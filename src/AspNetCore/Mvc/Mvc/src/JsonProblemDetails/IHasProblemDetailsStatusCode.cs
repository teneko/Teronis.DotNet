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
