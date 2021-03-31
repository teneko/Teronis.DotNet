// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    internal class ParameterList : IReadOnlyList<ParameterInfo>
    {
        public static ParameterList Parse(ParameterInfo[] parameterInfos)
        {
            var parameterList = new ParameterList();

            foreach (var parameterInfo in parameterInfos) {
                parameterList.AddParameter(parameterInfo);
            }

            return parameterList;
        }

        public IReadOnlyList<Exception> Errors =>
            errors;

        public bool HasErrors =>
            Errors.Count != 0;

        public bool HasCancellationTokenParameter =>
            cancellableAnnotatedParameter?.IsCancellationToken ?? false;

        public bool HasTimeoutParameter =>
            cancellableAnnotatedParameter?.IsTimeout ?? false;

        public bool HasAccommodatableAnnotatedParameter =>
            !(accommodatableAnnotatedParameter is null);

        public IEnumerable<string> ArgumentNames =>
            parameterInfos.Select(x => x.Name!);

        public int Count =>
            parameterInfos.Count;

        private bool hasCancellableAnnotatedParameter =>
            !(cancellableAnnotatedParameter is null);

        private List<Exception> errors;
        private List<ParameterInfo> parameterInfos;
        private CancellableAnnotatedParameter? cancellableAnnotatedParameter;
        private AccommodatableAnnotatedParameter? accommodatableAnnotatedParameter;

        private ParameterList()
        {
            parameterInfos = new List<ParameterInfo>();
            errors = new List<Exception>();
        }

        public ParameterInfo this[int index] =>
           ((IReadOnlyList<ParameterInfo>)parameterInfos)[index];

        private void AddError(Exception error) =>
            errors.Add(error);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="annotatedParameter"></param>
        /// <param name="newAnnotatedParameter"></param>
        private void SetAnnotatedParameter<T>(ref T? annotatedParameter, T newAnnotatedParameter)
            where T : class, IParameterInfoReader
        {
            try {
                annotatedParameter = newAnnotatedParameter;
                annotatedParameter.ReadParameterInfo();
            } catch (Exception error) {
                AddError(error);
            }
        }

        private bool DetermineCancellableAnnotatedParameter(ParameterInfo parameterInfo)
        {
            var attribute = parameterInfo.GetCustomAttribute<CancellableAttribute>();

            if (attribute is null) {
                return false;
            }

            if (!(cancellableAnnotatedParameter is null)) {
                AddError(new NotSupportedException("More than two cancellable annotated parameters are not allowed."));
                return true;
            }

            var parameter = new CancellableAnnotatedParameter(parameterInfo, attribute);
            SetAnnotatedParameter(ref cancellableAnnotatedParameter, parameter);
            return true;
        }

        private bool DetermineAccommodatableAnnotatedParameter(ParameterInfo parameterInfo)
        {
            var attribute = parameterInfo.GetCustomAttribute<AccommodatableAttribute>();

            if (attribute is null) {
                return false;
            }

            if (!(accommodatableAnnotatedParameter is null)) {
                AddError(new NotSupportedException("More than two accommodatable annotated parameters are not allowed."));
                return true;
            }

            var parameter = new AccommodatableAnnotatedParameter(parameterInfo, attribute);
            SetAnnotatedParameter(ref accommodatableAnnotatedParameter, parameter);
            return true;
        }

        private void AddParameter(ParameterInfo parameterInfo)
        {
            var isCancellableAnnotatedParameter = DetermineCancellableAnnotatedParameter(parameterInfo);

            if (DetermineAccommodatableAnnotatedParameter(parameterInfo)) {
                if (isCancellableAnnotatedParameter) {
                    AddError(new NotSupportedException($"The parameter {parameterInfo} can not be cancellable annotated and accommodatable annotated."));
                }
            } else if (HasAccommodatableAnnotatedParameter) {
                AddError(new NotSupportedException($"The parameter {parameterInfo} cannot be after a parameter that is accommodatable annotated."));
            }

            parameterInfos.Add(parameterInfo);
        }

        public object?[] GetJavaScriptFunctionArguments(object?[] arguments)
        {
            var argumentList = new List<object?>();
            var parameterPosition = 0;

            foreach (var parameterInfo in parameterInfos) {
                if (hasCancellableAnnotatedParameter && cancellableAnnotatedParameter!.ParameterInfo.Position == parameterPosition) {
                    goto @continue;
                }

                if (HasAccommodatableAnnotatedParameter && accommodatableAnnotatedParameter!.ParameterInfo.Position == parameterPosition) {
                    if (!(arguments[parameterPosition] is IEnumerable accommodatableArgument)) {
                        throw new ArgumentException($"The accommodatable argument is not of type {typeof(IEnumerable)}.");
                    }

                    foreach (var value in accommodatableArgument) {
                        argumentList.Add(value);
                    }

                    goto @continue;
                }

                argumentList.Add(arguments[parameterPosition]);

                @continue:
                parameterPosition++;
            }

            return argumentList.ToArray();
        }

        public CancellationToken GetCancellationToken(object?[] arguments)
        {
            if (!HasCancellationTokenParameter) {
                throw new InvalidOperationException("Parameter list does not have a cancellation token parameter.");
            }

            return (CancellationToken)arguments[cancellableAnnotatedParameter!.ParameterInfo.Position]!;
        }

        public TimeSpan GetTimeSpan(object?[] arguments)
        {
            if (!HasTimeoutParameter) {
                throw new InvalidOperationException("Parameter list does not have a time span parameter.");
            }

            return (TimeSpan)arguments[cancellableAnnotatedParameter!.ParameterInfo.Position]!;
        }

        public IEnumerator<ParameterInfo> GetEnumerator() =>
            parameterInfos.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
