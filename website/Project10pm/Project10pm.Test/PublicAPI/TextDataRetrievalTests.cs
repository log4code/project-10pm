using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Project10pm.API.DataIngest;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using static Project10pm.API.DataIngest.TextController;

namespace Project10pm.API.Test.PublicAPI
{
    internal class TextDataRetrievalTests : TextDataApiFixture
    {
        [Test]
        public async Task TextGet_SingleId_ReturnsStatusCode200()
        {
            var model = new NewText()
            {
                Text = "2023-06-12"
            };

            var postResponse = await PostNewTextContent(model);
            var postResult = await postResponse.Content.ReadFromJsonAsync<NewTextParseResult>();
            var getResponse = await GetSingle(postResult?.Id);
            
            Assert.That((int)getResponse.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task TextGet_SingleId_ReturnsValidRecord()
        {
            var model = new NewText()
            {
                Text = "2023-06-12"
            };

            var postResponse = await PostNewTextContent(model);
            var postResult = await postResponse.Content.ReadFromJsonAsync<NewTextParseResult>();
            var getResult = await GetSingleTextContent(postResult?.Id);

            Assert.That(getResult?.Id, Is.EqualTo(postResult?.Id));
            Assert.That(getResult.Text, Is.EqualTo(model.Text));
        }

        [Test]
        public async Task TextGet_InvalidId_ReturnsStatusCode404()
        {
            var getResponse = await GetSingle(1);
            Assert.That((int)getResponse.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task TextGet_BadId_ReturnsStatusCode422()
        {
            var getResponse = await _httpClient.GetAsync($"{TEXT_GET_ENDPOINT}/asdfasdf");
            Assert.That((int)getResponse.StatusCode, Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
        }

        [Test]
        public async Task TextGet_NoFilters_ReturnPaginatedRecords()
        {
            var date = new DateTime(2023, 1, 1,0,0,0,DateTimeKind.Utc);
            var dateCount = TextController.DEFAULT_PAGE_SIZE;

            List<TextContent?> storedRecords = new List<TextContent?>();
            for(var i = 0; i < dateCount; i++)
            {
                var model = new NewText()
                {
                    Text = date.AddDays(i).ToString(),
                };
                var response = await PostNewTextContent(model);
                var parseResult = await response.Content.ReadFromJsonAsync<NewTextParseResult>();
                storedRecords.Add(new TextContent
                {
                    Id = parseResult?.Id ?? -1,
                    Text = parseResult?.InputText ?? string.Empty,
                });
            }
            
            var records = await _httpClient.GetFromJsonAsync<List<TextContent>>(TEXT_GET_ENDPOINT) ?? new List<TextContent>();

            var except = storedRecords.Except(records, new TextContentComparer());
            Assert.That(except.Count, Is.EqualTo(dateCount - TextController.DEFAULT_PAGE_SIZE));
        }
    }
}
