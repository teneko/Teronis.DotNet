

namespace Teronis.Identity.Presenters
{
    public static class JsonErrorExtensions
    {
        /// <summary>
        /// Creates a servce result from provided <paramref name="jsonError"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonError"></param>
        /// <returns></returns>
        public static ServiceResult ToServiceResult(this JsonError? jsonError) =>
            new ServiceResult(false, content: jsonError);

        public static JsonErrors ToJsonErrors(this JsonError? jsonError) =>
            new JsonErrors(jsonError);
    }
}
