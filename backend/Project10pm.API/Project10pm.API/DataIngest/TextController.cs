using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Project10pm.API.DataIngest
{
    [ApiController]
    [Route("api/v1/text")]
    public class TextController : Controller
    {
        public const int DEFAULT_PAGE = 1;
        public const int DEFAULT_PAGE_SIZE = 10;

        private readonly TextContentRepo _textContentRepo;

        public TextController(TextContentRepo textContentRepo)
        {
            _textContentRepo = textContentRepo;
        }

        /// <summary>
        /// Retrieve all records based on the given filters
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TextContent>))]
        public IActionResult Get()
        {
            Dictionary<int, string?> records = _textContentRepo.Get(DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
            var results = records.Select(item => new TextContent
            {
                Id = item.Key,
                Text = item.Value
            });
            return Ok(results);
        }

        /// <summary>
        /// Add new text based content for parsing
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewTextParseResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public IActionResult Post([FromBody] NewText model)
        {
            var id = _textContentRepo.Add(model.Text);
            var result = new NewTextParseResult()
            {
                Id = id,
                InputText = model.Text
            };
            return Ok(result);
        }

        public class NewText
        {
            [Required]
            public string Text { get; set; } = string.Empty;
        }

        public class NewTextParseResult
        {
            public int Id { get; set; }
            public string? InputText { get; set; }
        }

        public class TextContent
        {
            public int Id { get; set; }
            public string? Text { get; set; }
        }

        public class TextContentComparer : IEqualityComparer<TextContent?>
        {
            public bool Equals(TextContent? x, TextContent? y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.Id ==  y.Id && x.Text == y.Text)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode([DisallowNull] TextContent obj)
            {
                var hash = $"{obj.Id}:{obj.Text}".GetHashCode();
                return hash;
            }
        }
    }
}
