using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MySUS.MvcFramework.ViewEngine
{
    public class SusViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            string cSharpCode = GenerateCSharpFromTemplate(templateCode);
            IView executableObject = GenerateExecutableCode(cSharpCode, viewModel);
            string html = executableObject.ExecuteTemplate(viewModel);
            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            string cSharpCode = @"
                                using System;
                                using System.Collections.Generic;
                                using System.Linq;
                                using System.Text;
                                using MySUS.MvcFramework.ViewEngine;
                                
                                namespace ViewNamespace
                                {
                                    public class ViewClass : IView
                                    {
                                        public string ExecuteTemplate(object viewModel)
                                        {
                                            var html = new StringBuilder();"
                                            + GetMethodBody(templateCode) +
                                            @"
                                            return html.ToString().Trim();
                                        }
                                    }
                                }
                                ";
            return cSharpCode;

        }

        private string GetMethodBody(string templateCode)
        {
            Regex cSharpCodeRegex = new Regex(@"[^\""\s&\'\<]+");
            var supportedOperators = new List<string>() { "for", "foreach", "if", "else", "while" };
            StringBuilder cSharpCode = new StringBuilder();
            StringReader sr = new StringReader(templateCode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (supportedOperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    var index = line.IndexOf("@");
                    line = line.Remove(index, 1);
                    cSharpCode.AppendLine(line);
                }
                else if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    cSharpCode.AppendLine(line);
                }
                else
                {
                    cSharpCode.AppendLine($"html.Append(@\"");
                    while (line.Contains("@"))
                    {
                        var atSignLocation = line.IndexOf("@");
                        var htmlBeforeAtSign = line.Substring(0, atSignLocation);
                        cSharpCode.Append(htmlBeforeAtSign.Replace("\"", "\"\"") + "\" +");
                        var lineAfterAtSign = line.Substring(atSignLocation + 1);
                        var code = cSharpCodeRegex.Match(lineAfterAtSign).Value;
                        cSharpCode.Append(code + " + @\"");
                        line = line.Substring(lineAfterAtSign.Length);
                    }
                    cSharpCode.AppendLine(line.Replace("\"", "\"\"") + "\");");
                }

            }
            return cSharpCode.ToString();
        }

        private IView GenerateExecutableCode(string cSharpCode, object viewModel)
        {
            //Roslyn
            //C# as string => executable => IView => ExecuteTemplate
            var compileResult = CSharpCompilation
                .Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));
            if (viewModel != null)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(cSharpCode));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult emitResult = compileResult.Emit(memoryStream);
                if (!emitResult.Success)
                {
                    return new ErrorView(
                        emitResult.Diagnostics
                        .Where(err => err.Severity == DiagnosticSeverity.Error)
                        .Select(mes => mes.GetMessage())
                        , cSharpCode);
                }

                try
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var byteAssembly = memoryStream.ToArray();
                    var assembly = Assembly.Load(byteAssembly);
                    var viewType = assembly.GetType("ViewNamespace.ViewClass");
                    var instance = Activator.CreateInstance(viewType);
                    return instance as IView;

                }
                catch (Exception ex)
                {
                    return new ErrorView(new List<string> { ex.ToString() }, cSharpCode);
                }

            }

        }
    }
}
