using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project10pm.API.DataIngest
{
    [ApiController]
    [Route("api/v1/text")]
    public class TextController : Controller
    {
        private readonly TextContentRepo _textContentRepo;

        public TextController(TextContentRepo textContentRepo)
        {
            _textContentRepo = textContentRepo;
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
    }
}
