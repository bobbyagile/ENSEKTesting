using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightTests;
using PlaywrightTests.Tests;

namespace PlaywrightTests.Tests
{
    [TestClass]
    public class OrderTests : BaseApiTest
    {
        [TestMethod]
        public async Task GetOrders_ShouldReturnOkAndOrdersList()
        {
            var response = await ApiClient!.GetAsync("/ENSEK/orders");

            Assert.IsTrue(response.Ok, $"API Request to get orders failed. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // Further assertions on the response content (e.g., it's a JSON array)
        }

        [TestMethod]
        public async Task GetOrderById_WithAuthorization_ShouldReturnSpecificOrder()
        {
            string orderId = "31fc32da-bccb-44ab-9352-4f43fc44ed4b"; // Example order ID

            var response = await ApiClient!.GetAsync($"/ENSEK/orders/{orderId}", ApiClient.AuthorizedHeaders());

            Assert.IsTrue(response.Ok, $"Failed to get order {orderId}. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // Assertions to verify the returned order's ID matches the requested ID.
        }

        [TestMethod]
        public async Task UpdateOrder_WithAuthorization_ShouldReturnUpdatedOrder()
        {
            string orderIdToUpdate = "31fc32da-bccb-44ab-9352-4f43fc44ed4b"; // Example order ID
            var updatedQuantity = 50;
            var updatedEnergyId = 1;

            var payload = new
            {
                id = orderIdToUpdate,
                quantity = updatedQuantity,
                energy_id = updatedEnergyId
            };

            var response = await ApiClient!.PutAsync($"/ENSEK/orders/{orderIdToUpdate}", payload, ApiClient.JsonHeaders(withToken: true));

            Assert.IsTrue(response.Ok, $"Failed to update order {orderIdToUpdate}. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)response.Status, $"Expected status OK, but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
            // Assertions to verify the response reflects the updated quantity/energy_id.
        }

        [TestMethod]
        public async Task DeleteOrder_WithAuthorization_ShouldReturnSuccess()
        {
            // IMPORTANT: In a real test suite, consider creating an order first, then deleting it.
            string orderIdToDelete = "2cdd6f69-95df-437e-b4d3-e772472db8de"; // Example order ID

            var response = await ApiClient!.DeleteAsync($"/ENSEK/orders/{orderIdToDelete}", ApiClient.AuthorizedHeaders());

            Assert.IsTrue(response.Ok, $"Failed to delete order {orderIdToDelete}. Status: {response.Status}. Body: {await response.TextAsync()}");
            Assert.IsTrue(response.Status == (int)HttpStatusCode.OK || response.Status == (int)HttpStatusCode.NoContent,
                $"Expected status 200 OK or 204 No Content for deletion, but got {response.Status}");
            Console.WriteLine($"Delete {orderIdToDelete} response status: {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
        }

        // Add negative test cases for orders (e.g., get non-existent order, update with invalid data)
        [TestMethod]
        public async Task GetOrderById_WithInvalidId_ShouldReturnNotFound()
        {
            string invalidOrderId = "00000000-0000-0000-0000-000000000000"; // A common pattern for an invalid ID

            var response = await ApiClient!.GetAsync($"/ENSEK/orders/{invalidOrderId}", ApiClient.AuthorizedHeaders());

            Assert.IsFalse(response.Ok, $"Unexpectedly found order with invalid ID: {invalidOrderId}. Status: {response.Status}");
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)response.Status, $"Expected status NotFound (404), but got {response.Status}");
            await ApiClient.PrintResponseBodyAsync(response);
        }
    }
}