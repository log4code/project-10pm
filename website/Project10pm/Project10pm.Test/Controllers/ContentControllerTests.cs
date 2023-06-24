using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project10pm.Controllers;
using Project10pm.Repositories;
using static Project10pm.Controllers.ContentController;

namespace Project10pm.API.Test.Controllers
{
    [TestFixture]
    internal class ContentControllerTests
    {
        private TextContentRepo _textContentRepo;

        [SetUp]
        public void SetUp()
        {
            _textContentRepo = new TextContentRepo();
        }

        [Test]
        public void ContentIndex_DefaultPagination_Returns200WithView()
        {
            for (var i = 1; i <= 10; i++)
            {
                var content = new TextContent
                {
                    RawText = $"content{i}"
                };
                _textContentRepo.Add(content);
            }

            var controller = new ContentController(_textContentRepo);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Index() as ViewResult;

            Assert.That(controller.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.Not.Null); // add additional checks on the Model
            Assert.That(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index", Is.True);
        }

        [Test]
        public void ContentIndex_DefaultPagination_ReturnsListModel()
        {
            for(var i = 1; i <= 10; i++)
            {
                var content = new TextContent
                {
                    RawText = $"content{i}"
                };
                _textContentRepo.Add(content);
            }

            var controller = new ContentController(_textContentRepo);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Index() as ViewResult;

            Assert.That(controller.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.Not.Null); // add additional checks on the Model
            Assert.That(result.Model, Is.TypeOf<Dictionary<int, TextContent?>>());
        }

        [Test]
        public void AddTextContent_ValidModel_ReturnsRedirectToAction()
        {
            var model = new NewText
            {
                Text = "2023-06-16"
            };
            var controller = new ContentController(_textContentRepo);

            var result = controller.Add(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ControllerName, Is.EqualTo("Content"));
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void AddTextContent_InvalidModel_Returns422WithView()
        {
            var controller = new ContentController(_textContentRepo);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Text", "Text is required");
            var result = controller.Add(new NewText()) as ViewResult;

            Assert.That(controller.Response.StatusCode, Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.Not.Null); // add additional checks on the Model
            Assert.That(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Add", Is.True);
        }
    }
}
