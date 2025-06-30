using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightTests.Tests
{
    /// <summary>
    /// Base class for all API test classes. Handles Playwright and ApiClient setup and teardown.
    /// </summary>
    public abstract class BaseApiTest
    {
        protected IPlaywright? PlaywrightInstance { get; private set; }
        protected ApiClient? ApiClient { get; private set; }

        private const string BaseUrl = "https://qacandidatetest.ensek.io";
        // IMPORTANT: In a real app, manage this token securely (e.g., from environment variables or a login flow).
        private const string Token = "eyJ0eXAiOi..."; // Replace with your actual token

        [TestInitialize]
        public async Task BaseSetup()
        {
            PlaywrightInstance = await Playwright.CreateAsync();
            var apiContext = await PlaywrightInstance.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = BaseUrl,
                ExtraHTTPHeaders = new Dictionary<string, string> { { "Accept", "application/json" } }
            });
            ApiClient = new ApiClient(apiContext, BaseUrl, Token);
            Console.WriteLine($"--- Test setup for {TestContext.TestName} ---");
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            ApiClient?.Dispose();
            PlaywrightInstance?.Dispose();
            Console.WriteLine($"--- Test cleanup for {TestContext.TestName} ---");
        }

        // MSTest provides a TestContext property for each test class if you need it.
        public TestContext TestContext { get; set; } = null!;
    }
}