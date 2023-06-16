using Microsoft.AspNetCore.Mvc;
using Project10pm.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Project10pm.Controllers
{
    public class ContentController : Controller
    {
        private TextContentRepo _textContentRepo;

        public ContentController(TextContentRepo textContentRepo)
        {
            _textContentRepo = textContentRepo;
        }

        public IActionResult Index()
        {
            var records = _textContentRepo.Get(1,10);
            return View(records);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        }

        /// <summary>
        /// Add new text based content for parsing
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewTextParseResult))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Add(NewText model)
        {
            if(false == ModelState.IsValid) 
            {
                //TODO: Need to figure out how to configure middleware so this status code happens automatically instead of a 200 or 400
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return View("Add", model);
            }

            var id = _textContentRepo.Add(model.Text);
            var result = new NewTextParseResult()
            {
                Id = id,
                InputText = model.Text
            };
            return RedirectToAction("Index", "Content");
        }

        public class NewText
        {
            //TODO: move validation messaging to use resource files: https://medium.com/@hoda_sedighi/localize-validation-error-message-using-data-annotation-in-asp-net-boilerplate-fbd4a0371f2e
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
