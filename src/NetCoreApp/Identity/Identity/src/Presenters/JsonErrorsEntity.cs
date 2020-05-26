using System.Linq;
using System.Collections.Generic;
using Microsoft.Collections.Extensions;

namespace Teronis.Identity.Presenters
{
    public class JsonErrorsEntity : OrderedDictionary<string, string>
    {
        public JsonErrorsEntity(IEnumerable<KeyValuePair<string, string>> collection)
            : base(collection) { }

        public static explicit operator JsonErrorsEntity(JsonErrors errors) =>
            new JsonErrorsEntity(errors.Errors.Select(error => new KeyValuePair<string, string>(error.Key, error.Value.Error.Message)));

        public static explicit operator JsonErrors(JsonErrorsEntity errors) =>
            new JsonErrors(errors);
    }
}
