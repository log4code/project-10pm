using Microsoft.AspNetCore.Mvc;
using Project10pm.API.DataIngest;
using static Project10pm.API.DataIngest.TextController;

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
    }
}
