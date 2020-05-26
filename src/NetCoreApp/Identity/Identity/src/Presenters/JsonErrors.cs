using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Collections.Extensions;
using Newtonsoft.Json;

namespace Teronis.Identity.Presenters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonErrors : ICollection<JsonError>
    {
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

        [JsonProperty("errors")]
        private JsonErrorsEntity jsonErrors {
            get => (JsonErrorsEntity)this;
            set => Errors = ((JsonErrors)value).Errors;
        }

        #region ICollection<JsonError>

        public int Count => Errors.Count;
        public bool IsReadOnly => false;

        #endregion

        public JsonErrors() { }

        public JsonErrors([DisallowNull]JsonError error) =>
            AddError(error);

        public JsonErrors(string errorCode, Exception errorMessage)
            : this(new JsonError(
                errorCode,
                errorMessage ?? throw new ArgumentNullException(nameof(errorMessage))))
        { }

        public JsonErrors(Exception errorMessage)
            : this(JsonError.DefaultErrorCode, errorMessage) { }

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

        public void AddError([DisallowNull]JsonError error)
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

        public void Add([DisallowNull]JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            Errors.Add(error.ErrorCode, error);
        }

        public void Clear() =>
            Errors.Clear();

        public bool Contains([DisallowNull]JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            return Errors.ContainsKey(error.ErrorCode);
        }

        public void CopyTo([DisallowNull]JsonError[] array, int beginInsertAtIndex)
        {
            array = array ?? throw new ArgumentNullException(nameof(array));
            var errorValues = Errors.Values;
            var errorsValuesCount = errorValues.Count;

            if (beginInsertAtIndex < 0) {
                throw new IndexOutOfRangeException("The array index is smaller than zero");
            } else if ((beginInsertAtIndex + errorsValuesCount) > array.Length) {
                throw new IndexOutOfRangeException("The array is too small");
            }

            for (var errorValuesIndex = 0; errorValuesIndex < errorsValuesCount; errorValuesIndex++) {
                array[beginInsertAtIndex] = errorValues[errorValuesIndex];
                beginInsertAtIndex++;
            }
        }

        public bool Remove([DisallowNull]JsonError error)
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
