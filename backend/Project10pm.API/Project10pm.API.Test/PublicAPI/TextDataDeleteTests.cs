using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Project10pm.API.DataIngest.TextController;

namespace Project10pm.API.Test.PublicAPI
{
    internal class TextDataDeleteTests
    {
        private const string TEXT_POST_ENDPOINT = @"/api/v1/text";
        private const string TEXT_DELETE_ENDPOINT = @"/api/v1/text";
        private WebApplicationFactory<Program> _appFactory;

        [SetUp]
        public void SetUp()
        {
            _appFactory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task TextDelete_ValidId_ReturnsStatusCode204()
        {
            var model = new NewText()
            {
                Text = "2023-06-08"
            };
            var client = _appFactory.CreateClient();

            var postResponse = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            var postResult = await postResponse.Content.ReadFromJsonAsync<NewTextParseResult>();
            var response = await client.DeleteAsync($"{TEXT_DELETE_ENDPOINT}/{postResult?.Id}");

            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }
    }
}
