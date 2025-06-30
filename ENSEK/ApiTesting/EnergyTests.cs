using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightTests;
using PlaywrightTests.Tests;

namespace PlaywrightTests.Tests
{
    [TestClass]
    public class EnergyTests : BaseApiTest
    {
        [TestMethod]
        public async Task GetEnergy_ShouldReturnOkAndEnergyList()
        {
            var response = await ApiClient!.GetAsync("/ENSEK/energy");

            Assert.IsTrue(response.Ok, $"API Request to get energy failed. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // Further assertions on the response content (e.g., it's a JSON array)
        }
        // Add more specific energy-related tests if the API offers more endpoints
    }
}