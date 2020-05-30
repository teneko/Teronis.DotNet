using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Collections.Extensions;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Teronis.Identity.Presenters
{
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonErrorsJsonConverter))]
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonErrors : ICollection<JsonError>
    {
        internal const string ErrorsPropertyName = "errors";

        /// <summary>
        /// Makes a copy of <see cref="jsonErrors"/>.
        /// </summary>
        public static JsonErrors FromJsonErrors(JsonErrors? jsonErrors)
        {
            var newJsonErrors = new JsonErrors();

            if (jsonErrors?.Error == null) {
                return newJsonErrors;
            }

            foreach (var error in jsonErrors.Errors) {
                newJsonErrors.Errors.Add(error.Key, new JsonError(error.Key, error.Value.Error));
            }

            return newJsonErrors;
        }

        public OrderedDictionary<string, JsonError> Errors { get; private set; } =
            new OrderedDictionary<string, JsonError>();

        public JsonError? Error => Errors.Values.FirstOrDefault();

        [JsonProperty(ErrorsPropertyName)]
        private JsonErrorsEntity jsonErrors {
            get => (JsonErrorsEntity)this;
            set => Errors = ((JsonErrors)value).Errors;
        }

        #region ICollection<JsonError>

        public int Count => Errors.Count;
        public bool IsReadOnly => false;

        #endregion

        public JsonErrors() { }

        public JsonErrors(JsonError? error) =>
            AddError(error ?? new JsonError(null));

        public JsonErrors(string? errorCode, Exception? error)
            : this(new JsonError(errorCode, error))
        { }

        public JsonErrors(Exception? error)
            : this(JsonError.DefaultErrorCode, error) { }

        public JsonErrors(string errorMessage)
            : this(new Exception(errorMessage)) { }

        public JsonErrors(IEnumerable<KeyValuePair<string, string>> errors)
        {
            errors = errors ?? throw new ArgumentNullException(nameof(errors));
            fillWithErrorMessages(errors);
        }

        public JsonErrors(IEnumerable<KeyValuePair<string, Exception>> errors)
        {
            errors = errors ?? throw new ArgumentNullException(nameof(errors));
            fillWithErrors(errors);
        }

        public void AddError(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            Errors.Add(error.ErrorCode, error);
        }

        private void fillWithErrorMessages(IEnumerable<KeyValuePair<string, string>> errors)
        {
            foreach (var error in errors) {
                AddError(new JsonError(error.Key, error.Value));
            }
        }

        private void fillWithErrors(IEnumerable<KeyValuePair<string, Exception>> errors)
        {
            foreach (var error in errors) {
                AddError(new JsonError(error.Key, error.Value));
            }
        }

        public AggregateException CreateAggregatedException() =>
            new AggregateException("Error", Errors.Select(error =>
                new Exception(error.Key, error.Value.Error)));

        public override string ToString() =>
            CreateAggregatedException().ToString();

        #region ICollection<JsonError>

        public void Add(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            Errors.Add(error.ErrorCode, error);
        }

        public void Clear() =>
            Errors.Clear();

        public bool Contains(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            return Errors.ContainsKey(error.ErrorCode);
        }

        public void CopyTo(JsonError[] array, int beginInsertAtIndex)
        {
            array = array ?? throw new ArgumentNullException(nameof(array));
            var errorValues = Errors.Values;
            var errorsValuesCount = errorValues.Count;

            if (beginInsertAtIndex < 0) {
                throw new IndexOutOfRangeException("The array index is smaller than zero.");
            } else if ((beginInsertAtIndex + errorsValuesCount) > array.Length) {
                throw new IndexOutOfRangeException("The array is too small.");
            }

            for (var errorValuesIndex = 0; errorValuesIndex < errorsValuesCount; errorValuesIndex++) {
                array[beginInsertAtIndex] = errorValues[errorValuesIndex];
                beginInsertAtIndex++;
            }
        }

        public bool Remove(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            return Errors.Remove(error.ErrorCode);
        }

        public IEnumerator<JsonError> GetEnumerator() =>
            Errors.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Errors.Values.GetEnumerator();

        #endregion ICollection<JsonError>
    }
}
