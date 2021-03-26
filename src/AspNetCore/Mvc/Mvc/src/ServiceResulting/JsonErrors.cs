// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Teronis.AspNetCore.Identity;

namespace Teronis.Mvc.ServiceResulting
{
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonErrorsTextJsonConverter))]
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonErrors : IList<JsonError>
    {
        internal const string ErrorsPropertyName = "errors";

        public List<JsonError> Errors { get; private set; } =
            new List<JsonError>();

        public JsonError? Error => Errors.FirstOrDefault();

        #region ICollection<JsonError>

        public int Count => Errors.Count;
        public bool IsReadOnly => false;

        #endregion

        #region IList<JsonError>

        public JsonError this[int index] {
            get => Errors[index];
            set => Errors[index] = value;
        }

        #endregion

        public JsonErrors() { }

        public JsonErrors(JsonError? error) =>
            Add(error ?? new JsonError(null));

        public JsonErrors(Exception? error, string? errorCode = null)
            : this(new JsonError(error, errorCode)) { }

        public JsonErrors(string? errorMessage, string? errorCode = null)
            : this(new JsonError(errorMessage, errorCode)) { }

        public JsonErrors(IEnumerable<KeyValuePair<string, string>> errors)
        {
            errors = errors ?? throw new ArgumentNullException(nameof(errors));
            fillWithErrorMessages(errors);
        }

        public JsonErrors(IEnumerable<KeyValuePair<Exception, string>> errors)
        {
            errors = errors ?? throw new ArgumentNullException(nameof(errors));
            fillWithErrors(errors);
        }

        private void fillWithErrorMessages(IEnumerable<KeyValuePair<string, string>> errors)
        {
            foreach (var error in errors) {
                Add(new JsonError(error.Key, error.Value));
            }
        }

        private void fillWithErrors(IEnumerable<KeyValuePair<Exception, string>> errors)
        {
            foreach (var error in errors) {
                Add(new JsonError(error.Key, error.Value));
            }
        }

        public override string ToString()
        {
            var errorsCount = Errors.Count;

            if (errorsCount == 0) {
                return StringResources.DefaultErrorMessage;
            } else if (errorsCount == 1) {
                return Errors[0].Error.Message;
            }

            return $"{StringResources.MoreThanOneExcpetionOccuredMessage} {Errors.Select(x => x.Error.Message)}";
        }

        #region ICollection<JsonError>

        public void Add(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            Errors.Add(error);
        }

        public void Clear() =>
            Errors.Clear();

        public bool Contains(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            return Errors.Contains(error);
        }

        public void CopyTo(JsonError[] array, int beginInsertAtIndex)
        {
            Errors.CopyTo(array, beginInsertAtIndex);
        }

        //public void CopyTo(JsonError[] array, int beginInsertAtIndex)
        //{
        //    array = array ?? throw new ArgumentNullException(nameof(array));
        //    var errorValues = Errors.Values;
        //    var errorsValuesCount = errorValues.Count;

        //    if (beginInsertAtIndex < 0) {
        //        throw new IndexOutOfRangeException("The array index is smaller than zero.");
        //    } else if ((beginInsertAtIndex + errorsValuesCount) > array.Length) {
        //        throw new IndexOutOfRangeException("The array is too small.");
        //    }

        //    for (var errorValuesIndex = 0; errorValuesIndex < errorsValuesCount; errorValuesIndex++) {
        //        array[beginInsertAtIndex] = errorValues[errorValuesIndex];
        //        beginInsertAtIndex++;
        //    }
        //}

        public bool Remove(JsonError error)
        {
            error = error ?? throw new ArgumentNullException(nameof(error));
            return Errors.Remove(error.ErrorCode);
        }

        public IEnumerator<JsonError> GetEnumerator() =>
            Errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Errors.GetEnumerator();

        #endregion ICollection<JsonError>

        #region IList<JsonError>

        public int IndexOf(JsonError item)
        {
            item = item ?? throw new ArgumentNullException(nameof(item));
            return Errors.IndexOf(item);
        }

        public void Insert(int index, JsonError item)
        {
            item = item ?? throw new ArgumentNullException(nameof(item));
            Errors.Insert(index, item);
        }

        public void RemoveAt(int index) =>
            Errors.RemoveAt(index);

        #endregion
    }
}
