// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class HttpClientExtensions
    {
        #region PrepareHttpRequest

        public static Task<T> PrepareHttpRequestAsync<T>(this HttpClient client, HttpClientHandler? clientHandler, Func<HttpClient, HttpClientHandler?, Task<T>> callback)
        {
            if (callback != null) {
                return callback.Invoke(client, clientHandler);
            }

            return Task.FromResult<T>(default!);
        }

        public static Task<T> PrepareHttpRequestAsync<T>(this HttpClient pseudoClient, Func<HttpClient, Task<T>> clientHandler)
            => PrepareHttpRequestAsync(pseudoClient, null, async (client, handler) => await clientHandler?.Invoke(client)!);

        public static async Task PrepareHttpRequestAsync(this HttpClient pseudoClient, HttpClientHandler? clientHandler, Func<HttpClient, HttpClientHandler?, Task> callback)
        {
            await PrepareHttpRequestAsync(pseudoClient, clientHandler, (_client, _handler) => {
                if (callback != null) {
                    return callback.Invoke(_client, clientHandler);
                }

                return Task.CompletedTask;
            });
        }

        public static async Task PrepareHttpRequestAsync(this HttpClient pseudoClient, Func<HttpClient, Task> clientHandler)
            => await PrepareHttpRequestAsync(pseudoClient, null, async (client, handler) => await clientHandler?.Invoke(client)!);

        #endregion
    }
}
