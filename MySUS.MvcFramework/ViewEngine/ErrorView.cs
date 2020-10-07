﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySUS.MvcFramework.ViewEngine
{
    public class ErrorView : IView
    {
        private readonly IEnumerable<string> errors;
        private readonly string cSharpCode;

        public ErrorView(IEnumerable<string> errors, string cSharpCode)
        {
            this.errors = errors;
            this.cSharpCode = cSharpCode;
        }

        public string ExecuteTemplate(object viewModel)
        {
            var html = new StringBuilder();
            html.AppendLine($"<h1>View compile {this.errors.Count()} errors: </h1><ul>");
            foreach (var error in errors)
            {
                html.AppendLine($"<li>{error}</li>");
            }
            html.AppendLine($"</ul>{this.cSharpCode}<pre><pre>");
            return html.ToString();
        }
    }
}
