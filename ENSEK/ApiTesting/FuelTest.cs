using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightTests;
using PlaywrightTests.Tests;

namespace PlaywrightTests.Tests
{
    [TestClass]
    public class FuelTests : BaseApiTest
    {
        [TestMethod]
        public async Task BuyFuel_WithValidQuantity_ShouldReturnSuccess()
        {
            // Endpoint for buying fuel, assuming "1" is energyId and "2" is quantity
            var response = await ApiClient!.PutAsync("/ENSEK/buy/1/2");

            Assert.IsTrue(response.Ok, $"API Request to buy fuel failed. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // Further assertions could be added to confirm the purchase was successful (e.g., check updated balance).
        }

        [TestMethod]
        public async Task BuyFuel_WithInsufficientQuantity_ShouldFail()
        {
            // Example: Assuming buying 1000000 units of energy ID 1 would fail (e.g., too much or not enough stock)
            // You'll need to adapt this based on the actual API's error handling for invalid quantities.
            var response = await ApiClient!.PutAsync("/ENSEK/buy/1/1000000");

            Assert.IsFalse(response.Ok, $"Buying large quantity unexpectedly succeeded. Status: {response.Status}");
            // Replace with the actual expected status code for a failed purchase (e.g., 400 Bad Request, 403 Forbidden).
            Assert.IsTrue(response.Status >= 400 && response.Status < 500, $"Expected a client error status, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
        }
    }
}