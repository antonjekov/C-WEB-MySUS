using MySUS.MvcFramework.ViewEngine;
using System;
using System.Collections.Generic;
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

            IViewEngine viewEngine = new SusViewEngine();
            var view = File.ReadAllText($"ViewTests/{fileName}.html");
            var result = viewEngine.GetHtml(view, viewModel, null);
            var expected = File.ReadAllText($"ViewTests/{fileName}.Result.html");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestTemplateViewMode()
        {
            IViewEngine viewEngine = new SusViewEngine();
            var result = viewEngine.GetHtml(@"@foreach(var num in Model)
{
<span>@num</span>
}", new List<int>() { 1, 2, 3 },null);

            var expected = @"<span>1</span>
<span>2</span>
<span>3</span>";
            Assert.Equal(expected, result);
        }
    }
}
