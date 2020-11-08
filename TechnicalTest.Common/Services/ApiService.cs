using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Common.Models;
using TechnicalTest.Common.Models.Request;
using TechnicalTest.Common.Models.Response;

namespace TechnicalTest.Common.Services
{
    public class ApiService : IApiService
    {

        public async Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request)
        {
            try
            {
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<TokenResponse>
                    {
                        RealizadoCorrectamente = false,
                        Mensaje = result,
                    };
                }

                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response<TokenResponse>
                {
                    RealizadoCorrectamente = true,
                    Resultado = token
                };
            }
            catch (Exception ex)
            {
                return new Response<TokenResponse>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = ex.Message
                };
            }
        }

        public async Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        RealizadoCorrectamente = false,
                        Mensaje = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response<object>
                {
                    RealizadoCorrectamente = true,
                    Resultado = list
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = ex.Message
                };
            }
        }

        public async Task<Response<object>> PostAddUser<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var finalResponse = JsonConvert.DeserializeObject<Response<object>>(answer);
                    return new Response<object>
                    {
                        RealizadoCorrectamente = false,
                        Mensaje = finalResponse.Mensaje,
                    };
                }

                return new Response<object>
                {
                    RealizadoCorrectamente = true
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = ex.Message,
                };
            }
        }

        public async Task<Response<UserInformationResponse>> PostUserState<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<UserInformationResponse>
                    {
                        RealizadoCorrectamente = false,
                        Mensaje = answer,
                    };
                }

                var finalResponse = JsonConvert.DeserializeObject<Response<UserInformationResponse>>(answer);
                return finalResponse;
            }
            catch (Exception ex)
            {
                return new Response<UserInformationResponse>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = ex.Message,
                };
            }
        }
    }
}
