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
    internal class TextDataDeleteTests : TextDataApiFixture
    {
        [Test]
        public async Task TextDelete_ValidId_ReturnsStatusCode204()
        {
            var model = new NewText()
            {
                Text = "2023-06-08"
            };

            var postResponse = await PostNewTextContent(model);
            var postResult = await postResponse.Content.ReadFromJsonAsync<NewTextParseResult>();
            var response = await _httpClient.DeleteAsync($"{TEXT_DELETE_ENDPOINT}/{postResult?.Id}");

            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Test]
        public async Task TextDelete_InvalidId_ReturnsStatusCode404()
        {
            var model = new NewText()
            {
                Text = "2023-06-08"
            };
            var response = await _httpClient.DeleteAsync($"{TEXT_DELETE_ENDPOINT}/1");

            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task TextDelete_BadId_ReturnsStatusCode422()
        {
            var model = new NewText()
            {
                Text = "2023-06-08"
            };
            var response = await _httpClient.DeleteAsync($"{TEXT_DELETE_ENDPOINT}/asdf");

            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
        }

        [Test]
        public async Task TextDelete_ValidId_IsReallyDeleted()
        {
            var model = new NewText()
            {
                Text = "2023-06-08"
            };

            var postResponse = await PostNewTextContent(model);
            var postResult = await postResponse.Content.ReadFromJsonAsync<NewTextParseResult>();
            var deleteResponse = await _httpClient.DeleteAsync($"{TEXT_DELETE_ENDPOINT}/{postResult?.Id}");
            var getResponse = await GetSingle(postResult?.Id);

            Assert.That((int)getResponse.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}
