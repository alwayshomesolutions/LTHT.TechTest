namespace LTHT.PeopleManagement.Helpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Web query helper.
    /// </summary>
    public static class WebQuery
    {
        /// <summary>
        /// Returns result of the get request as string.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="uri">The uri of the JSON endpoint.</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>
        /// Result of the get request.
        /// </returns>
        public static async Task<T> GetJson<T>(Uri uri, CancellationToken token) where T : class
        {
            return await ExecuteJsonWebRequest<T>(uri, HttpMethod.Get, token);
        }

        /// <summary>
        /// Returns result of the post.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="uri">The uri of the JSON endpoint.</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// Result of the post.
        /// </returns>
        public static async Task<T> PostJson<T>(Uri uri, CancellationToken token, T content) where T : class
        {
            return await ExecuteJsonWebRequest<T>(uri, HttpMethod.Post, token, content);
        }

        /// <summary>
        /// Returns result of the put.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="uri">The uri of the JSON endpoint.</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// Result of the put.
        /// </returns>
        public static async Task<T> PutJson<T>(Uri uri, CancellationToken token, T content) where T : class
        {
            return await ExecuteJsonWebRequest<T>(uri, HttpMethod.Put, token, content);
        }

        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="uri">The uri of the JSON endpoint.</param>
        /// <param name="token">A cancellation token</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// A Task
        /// </returns>
        public static async Task DeleteJson<T>(Uri uri, CancellationToken token, T content = null) where T : class
        {
            await ExecuteJsonWebRequest<T>(uri, HttpMethod.Delete, token, content);
        }

        /// <summary>
        /// Returns result of the http request
        /// </summary>
        /// <typeparam name="T">The type of the entity in the response.</typeparam>
        /// <param name="uri">The uri of the JSON endpoint.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// Result of the http request.
        /// </returns>
        private static async Task<T> ExecuteJsonWebRequest<T>(Uri uri, HttpMethod httpMethod, CancellationToken token, T content = null, string authToken = null) where T : class
        {
            // NOTE: CancellationTokens appear in the API but are not used in the test. In a real app they would be used to allow user cancellation (in the
            // event of long running / non-responsive ops) but it needs a little more plumbing than I had time for.
            
            // Ensure we have enough info to proceed
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && content == null)
            {
                throw new ArgumentNullException("content");
            }

            // Set-up a default to return in case of failure
            var result = default(T);

            using (var httpClient = new HttpClient())
            {
                // Connection defaults
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                httpClient.Timeout = TimeSpan.FromMilliseconds(3500);

                try
                {
                    var url = uri.AbsoluteUri;
                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, url))
                    {
                        // Add any content if supplied
                        if (content != null)
                        {
                            // Serialize the content
                            // NOTE: Json.Net is much quicker (and simpler), but I've avoided all external libraries for this test....
                            // Also, there are additional (unecessary) conversions below that could be avoided by piping the stream straight to the request
                            // but leaving them made debugging simpler
                            var serializer = new DataContractJsonSerializer(typeof(T));
                            MemoryStream stream = new MemoryStream();
                            serializer.WriteObject(stream, content);
                            var streamArray = stream.ToArray();
                            string jsonString = Encoding.UTF8.GetString(streamArray, 0, streamArray.Length);
                            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        }

                        // Make the request...
                        using (var response = await httpClient.SendAsync(request, token))
                        {
                            // Ensure a success response
                            response.EnsureSuccessStatusCode();

                            // If we have a response, deserialze into T
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                if (stream != null)
                                {
                                    var serializer = new DataContractJsonSerializer(typeof(T));
                                    result = serializer.ReadObject(stream) as T;
                                }
                            }
                        }
                    }
                }
                // These are some typical exceptions. In a real system they would be captured and handled appropriately
                catch (SerializationException e)
                {
                    // Centralised logger TODO
                    Debug.WriteLine(e.ToString());
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                catch (TaskCanceledException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
                finally
                {
                    // Always tidy up resources
                    httpClient.CancelPendingRequests();
                    httpClient.Dispose();
                }
            }

            return result;
        }
    }
}
