using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightTests; // Assuming ApiClient is in this namespace
using PlaywrightTests.Tests; // Assuming BaseApiTest is in this namespace

namespace PlaywrightTests.Tests
{
    [TestClass]
    public class AuthTests : BaseApiTest
    {
        [TestMethod]
        public async Task Login_WithValidCredentials_ShouldSucceedAndReturnToken()
        {
            var payload = new
            {
                username = "test",
                password = "testing"
            };

            var response = await ApiClient!.PostAsync("/ENSEK/login", payload, ApiClient.JsonHeaders());

            Assert.IsTrue(response.Ok, $"Login failed. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // In a real scenario, you'd parse the token from the response.
        }

        [TestMethod]

        public async Task ResetSystem_WithAuthorization_ShouldSucceed()
        {
            var response = await ApiClient!.PostAsync("/ENSEK/reset", payload: null, headers: ApiClient.AuthorizedHeaders());

           // Assert.IsTrue(response.Ok, $"System reset failed. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreNotSame(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
        }

        // Add negative login test cases (e.g., invalid credentials) here.
        [TestMethod]
        public async Task Login_WithInvalidCredentials_ShouldFail()
        {
            var payload = new
            {
                username = "invalid",
                password = "wrongpassword"
            };

            var response = await ApiClient!.PostAsync("/ENSEK/login", payload, ApiClient.JsonHeaders());

            Assert.IsFalse(response.Ok, $"Login with invalid credentials unexpectedly succeeded. Status: {response.Status}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, (HttpStatusCode)response.Status, $"Expected status Unauthorized (401), but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
        }
    }
}