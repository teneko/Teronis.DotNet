// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc.Formatters;
using static Teronis.Utils.ICollectionGenericUtils;

namespace Teronis.Mvc.ServiceResulting
{
    public class ServiceResult : JsonResult, IServiceResult, IMutableServiceResult
    {
        public JsonErrors? Errors {
            get => Succeeded ? null : (JsonErrors)Value;
            private set => Value = value;
        }

        public object? Content {
            get => ContentOrDefault;
            private set => Value = value;
        }

        public bool Succeeded { get; private set; }

        protected virtual object? ContentOrDefault => Succeeded ? Value : null;

        #region IMutableServiceResult

        bool IMutableServiceResult.Succeeded {
            get => Succeeded;
            set => Succeeded = value;
        }

        object? IMutableServiceResult.Content {
            get => Content;
            set => Content = value;
        }

        int? IMutableServiceResult.StatusCode {
            get => StatusCode;
            set => StatusCode = value;
        }

        JsonErrors? IMutableServiceResult.Errors {
            get => Errors;
            set => Errors = value;
        }

        #endregion

        /// <summary>
        /// Creates a new service result from <paramref name="datransject"/>.
        /// </summary>
        /// <param name="datransject">The data with what this instance get filled. Noting will be deep copied.</param>
        internal ServiceResult(in ServiceResultDatransject datransject)
        {
            Succeeded = datransject.Succeeded;

            if (Succeeded) {
                Content = datransject.Content;
            } else {
                Errors = datransject.Errors;
            }

            DeclaredType = datransject.DeclaredType;
            Formatters = datransject.Formatters;
            ContentTypes = datransject.ContentTypes;
            StatusCode = datransject.StatusCode;
        }

        public ServiceResult(bool succeeded, object? content = null, int? statusCode = null)
        {
            Succeeded = succeeded;
            Content = content;
            StatusCode = statusCode;
        }

        public ServiceResultDatransject DeepCopy()
        {
            return new ServiceResultDatransject(Succeeded, Content, Errors is null ? null : AddItemRangeAndReturnList(new JsonErrors(), Errors),
                DeclaredType, Formatters is null ? null : AddItemRangeAndReturnList(new FormatterCollection<IOutputFormatter>(), Formatters),
                ContentTypes is null ? null : AddItemRangeAndReturnList(new MediaTypeCollection(), ContentTypes), StatusCode);
        }
    }
}
