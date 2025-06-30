using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Newtonsoft.Json;

namespace PlaywrightTests
{
    /// <summary>
    /// Represents a client for interacting with the API under test.
    /// Encapsulates Playwright API request context and common HTTP operations.
    /// </summary>
    public class ApiClient : IDisposable
    {
        private readonly IAPIRequestContext _apiContext;
        private readonly string _baseUrl;
        private readonly string _token;

        public ApiClient(IAPIRequestContext apiContext, string baseUrl, string token)
        {
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        // --- Helper Methods for Headers ---

        /// <summary>
        /// Gets default headers for API requests (Accept: application/json).
        /// </summary>
        public Dictionary<string, string> DefaultHeaders() => new()
        {
            { "Accept", "application/json" }
        };

        /// <summary>
        /// Gets headers including an Authorization Bearer token.
        /// </summary>
        public Dictionary<string, string> AuthorizedHeaders() => new()
        {
            { "Accept", "application/json" },
            { "Authorization", $"Bearer {_token}" }
        };

        /// <summary>
        /// Gets headers for JSON content, with an optional Authorization Bearer token.
        /// </summary>
        /// <param name="withToken">True to include the authorization token; otherwise, false.</param>
        public Dictionary<string, string> JsonHeaders(bool withToken = false)
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "Content-Type", "application/json" }
            };

            if (withToken)
                headers["Authorization"] = $"Bearer {_token}";

            return headers;
        }

        // --- HTTP Request Helper Methods ---

        /// <summary>
        /// Sends a GET request to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/ENSEK/orders").</param>
        /// <param name="headers">Optional custom headers for the request.</param>
        /// <returns>The API response.</returns>
        public async Task<IAPIResponse> GetAsync(string endpoint, Dictionary<string, string>? headers = null)
        {
            Console.WriteLine($"Sending GET request to {_baseUrl}{endpoint}");
            return await _apiContext.GetAsync(endpoint, new APIRequestContextOptions { Headers = headers });
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint with a payload.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/ENSEK/login").</param>
        /// <param name="payload">The object to be sent as the request body (will be JSON serialized).</param>
        /// <param name="headers">Optional custom headers for the request.</param>
        /// <returns>The API response.</returns>
        public async Task<IAPIResponse> PostAsync(string endpoint, object payload, Dictionary<string, string>? headers = null)
        {
            Console.WriteLine($"Sending POST request to {_baseUrl}{endpoint} with payload: {JsonConvert.SerializeObject(payload)}");
            return await _apiContext.PostAsync(endpoint, new APIRequestContextOptions { DataObject = payload, Headers = headers });
        }

        /// <summary>
        /// Sends a PUT request to the specified endpoint with an optional payload.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/ENSEK/buy/1/2").</param>
        /// <param name="payload">The object to be sent as the request body (will be JSON serialized). Can be null.</param>
        /// <param name="headers">Optional custom headers for the request.</param>
        /// <returns>The API response.</returns>
        public async Task<IAPIResponse> PutAsync(string endpoint, object? payload = null, Dictionary<string, string>? headers = null)
        {
            Console.WriteLine($"Sending PUT request to {_baseUrl}{endpoint} with payload: {(payload != null ? JsonConvert.SerializeObject(payload) : "null")}");
            return await _apiContext.PutAsync(endpoint, new APIRequestContextOptions { DataObject = payload, Headers = headers });
        }

        /// <summary>
        /// Sends a DELETE request to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/ENSEK/orders/123").</param>
        /// <param name="headers">Optional custom headers for the request.</param>
        /// <returns>The API response.</returns>
        public async Task<IAPIResponse> DeleteAsync(string endpoint, Dictionary<string, string>? headers = null)
        {
            Console.WriteLine($"Sending DELETE request to {_baseUrl}{endpoint}");
            return await _apiContext.DeleteAsync(endpoint, new APIRequestContextOptions { Headers = headers });
        }

        /// <summary>
        /// Prints the response body to the console.
        /// </summary>
        /// <param name="response">The API response.</param>
        public async Task PrintResponseBodyAsync(IAPIResponse response)
        {
            var body = await response.TextAsync();
            Console.WriteLine("Response body:");
            Console.WriteLine(body);
        }

        /// <summary>
        /// Disposes the underlying APIRequestContext.
        /// </summary>
        public void Dispose()
        {
            _apiContext.DisposeAsync().AsTask().Wait();
        }
    }
}