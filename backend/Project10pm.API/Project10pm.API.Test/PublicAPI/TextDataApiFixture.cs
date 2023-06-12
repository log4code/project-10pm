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
    internal class TextDataApiFixture
    {
        protected const string TEXT_DELETE_ENDPOINT = @"/api/v1/text";
        protected const string TEXT_GET_ENDPOINT = @"/api/v1/text";
        protected const string TEXT_POST_ENDPOINT = @"/api/v1/text";

        private WebApplicationFactory<Program> _appFactory;
        protected HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _appFactory = new WebApplicationFactory<Program>();
            _httpClient = _appFactory.CreateClient();
        }

        /// <summary>
        /// Post your own new text content. If not supplied, default valid text content will be used
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostNewTextContent(NewText? content = null)
        {
            if (content == null)
            {
                content = new NewText()
                {
                    Text = "2023-06-08"
                };
            }
            
            var postResponse = await _httpClient.PostAsync(TEXT_POST_ENDPOINT, JsonContent.Create(content));
            return postResponse;
        }

        protected async Task<HttpResponseMessage> GetSingle(int? id)
        {
            var getResponse = await _httpClient.GetAsync($"{TEXT_GET_ENDPOINT}/{id}");
            return getResponse;

        }

        protected async Task<TextContent?> GetSingleTextContent(int? id)
        {
            var getResponse = await _httpClient.GetAsync($"{TEXT_GET_ENDPOINT}/{id}");
            var getResult = await getResponse.Content.ReadFromJsonAsync<TextContent?>();
            return getResult;
        }
    }
}
