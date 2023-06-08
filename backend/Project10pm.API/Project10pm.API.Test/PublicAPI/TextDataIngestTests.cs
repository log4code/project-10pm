﻿using Microsoft.AspNetCore.Http;
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
    [TestFixture]
    internal class TextDataIngestTests
    {
        private const string TEXT_POST_ENDPOINT = @"/api/v1/text";
        private WebApplicationFactory<Program> _appFactory;

        [SetUp]
        public void SetUp() 
        {
            _appFactory = new WebApplicationFactory<Program>();
        }

        [TestCase("2023-06-06")]
        public async Task TextPost_NonEmptyContent_ReturnsStatusCode200(string input)
        {
            var model = new NewText()
            {
                Text = input
            };
            var client = _appFactory.CreateClient();
            var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            Assert.That(((int)response.StatusCode), Is.EqualTo(StatusCodes.Status200OK));
        }

        [TestCase("2023-06-06")]
        public async Task TextPost_NonEmptyContent_ReturnsRecordId(string input)
        {
            var model = new NewText()
            {
                Text = input
            };
            var client = _appFactory.CreateClient();
            var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            var parseResult = await response.Content.ReadFromJsonAsync<NewTextParseResult>();
            Assert.That(parseResult?.Id, Is.GreaterThan(0));
        }

        [TestCase("2023-06-06")]
        public async Task TextPost_NonEmptyContent_ReturnsInputText(string input)
        {
            var model = new NewText()
            {
                Text = input
            };
            var client = _appFactory.CreateClient();
            var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            var parseResult = await response.Content.ReadFromJsonAsync<NewTextParseResult>();
            Assert.That(parseResult?.InputText, Is.EqualTo(input));
        }

        [Test]
        public async Task TwoTextPosts_NonEmptyContent_ReturnDifferentIds()
        {
            var model = new NewText()
            {
                Text = "2023-06-06"
            };
            var model2 = new NewText()
            {
                Text = "2023-06-07"
            };
            
            var client = _appFactory.CreateClient();
            var response1 = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            var response2 = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(model));
            var parseResult1 = await response1.Content.ReadFromJsonAsync<NewTextParseResult>();
            var parseResult2 = await response2.Content.ReadFromJsonAsync<NewTextParseResult>();

            Assert.That(parseResult1?.Id, Is.Not.EqualTo(parseResult2?.Id));
        }

        [Test]
        public async Task TextPost_EmptyContent_ReturnsStatusCode400()
        {
            var client = _appFactory.CreateClient();
            var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(string.Empty));
            Assert.That(((int)response.StatusCode), Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task TextPost_NoContent_ReturnsStatusCode400()
        {
            var client = _appFactory.CreateClient();
            var response = await client.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create((object?)null));
            Assert.That(((int)response.StatusCode), Is.EqualTo(StatusCodes.Status400BadRequest));
        }
    }
}
