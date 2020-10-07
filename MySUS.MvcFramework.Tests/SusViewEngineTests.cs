using MySUS.MvcFramework.ViewEngine;
using System;
using System.IO;
using Xunit;

namespace MySUS.MvcFramework.Tests
{
    public class SusViewEngineTests
    {
        [Theory]
        [InlineData("CleanHtml")]
        [InlineData("Foreach")]
        [InlineData("IfElseFor")]
        [InlineData("ViewModel")]
        public void TestGetHtml(string fileName)
        {
            var viewModel = new TestViewModel()
            {
                DateOfBirth = new DateTime(2019, 6, 1),
                Name = "Doggo Argentino",
                Price = 12345.67m
            };

            IViewEngine viewEngine  = new SusViewEngine();
            var view = File.ReadAllText($"ViewTests/{fileName}.html");
            var result = viewEngine.GetHtml(view, viewModel);
            var expected = File.ReadAllText($"ViewTests/{fileName}.Result.html");
            Assert.Equal(expected, result);
        }

        public class TestViewModel
        {
            public string Name { get; set; }

            public DateTime DateOfBirth { get; set; }

            public decimal Price { get; set; }
        }
    }
}
