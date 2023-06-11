﻿using Microsoft.AspNetCore.Mvc.Testing;
using Project10pm.API.DataIngest;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using static Project10pm.API.DataIngest.TextController;

namespace Project10pm.API.Test.PublicAPI
{
    internal class TextDataRetrievalTests
    {
        private const string TEXT_POST_ENDPOINT = @"/api/v1/text";
        private const string TEXT_GET_ENDPOINT = @"/api/v1/text";
        private WebApplicationFactory<Program> _appFactory;

        [SetUp]
        public void SetUp()
        {
            _appFactory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task TextGet_NoFilters_ReturnPaginatedRecords()
        {
            var client = _appFactory.CreateClient();
            var date = new DateTime(2023, 1, 1,0,0,0,DateTimeKind.Utc);
            var dateCount = TextController.DEFAULT_PAGE_SIZE;

            List<TextContent?> storedRecords = new List<TextContent?>();
            for(var i = 0; i < dateCount; i++)
            {
                var model = new NewText()
                {
                    Text = date.AddDays(i).ToString(),
                };
                var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
                var parseResult = await response.Content.ReadFromJsonAsync<NewTextParseResult>();
                storedRecords.Add(new TextContent
                {
                    Id = parseResult?.Id ?? -1,
                    Text = parseResult?.InputText ?? string.Empty,
                });
            }
            
            var records = await client.GetFromJsonAsync<List<TextContent>>(TEXT_GET_ENDPOINT) ?? new List<TextContent>();

            var except = storedRecords.Except(records, new TextContentComparer());
            Assert.That(except.Count, Is.EqualTo(dateCount - TextController.DEFAULT_PAGE_SIZE));
        }
    }
}
