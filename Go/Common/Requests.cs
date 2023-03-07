using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Go.Common
{
    public static class Requests
    {
        /// <summary>
        /// Method of execute http get to service
        /// </summary>
        /// <param name="url">Address of service</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpGet(string url, string token = null, string user_agent = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(user_agent))
                        client.DefaultRequestHeaders.Add("user-agent", user_agent);

                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
                    
                    response = await client.GetAsync(url);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Method of execute http post to service
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="jsonParams">Parametros to send in post</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPost(string url, object jsonParams = null, string token = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    HttpContent data = new StringContent(JsonConvert.SerializeObject(jsonParams), Encoding.UTF8, "application/json");

                    response = await Client.PostAsync(new Uri(url), data).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Send request post with files
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPostFiles(string URL, MultipartFormDataContent Data, string token = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    response = await Client.PostAsync(new Uri(URL), Data).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Method of execute http put to service
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="jsonParams">Parametros to send in post</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPut(string url, object jsonParams, string token = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    HttpContent data = new StringContent(JsonConvert.SerializeObject(jsonParams), Encoding.UTF8, "application/json");

                    response = await Client.PutAsync(new Uri(url), data).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Method of execute http delete to service
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="jsonParams">Parametros to send in post</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpDelete(string url, string token = null, object jsonParams = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var Request = new HttpRequestMessage(new HttpMethod("DELETE"), new Uri(url));

                    if(jsonParams != null)
                    {
                        Request.Content = new StringContent(JsonConvert.SerializeObject(jsonParams), Encoding.UTF8, "application/json");
                    }

                    response = await Client.SendAsync(Request).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Method of execute http patch to service
        /// </summary>
        /// <param name="url">Url to invoke</param>
        /// <param name="jsonParams">Parametros to send in post</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPatch(string url, object jsonParams, string token = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var Request = new HttpRequestMessage(new HttpMethod("PATCH"), new Uri(url))
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(jsonParams), Encoding.UTF8, "application/json")
                    };

                    response = await Client.SendAsync(Request).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Send request post with files
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPatchFiles(string URL, MultipartFormDataContent Data, string token = null)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (var Client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var Request = new HttpRequestMessage(new HttpMethod("PATCH"), new Uri(URL))
                    {
                        Content = Data
                    };

                    response = await Client.SendAsync(Request).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch (TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }
    }
}