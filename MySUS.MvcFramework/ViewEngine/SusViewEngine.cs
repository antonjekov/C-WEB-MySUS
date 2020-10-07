using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MySUS.MvcFramework.ViewEngine
{
    public class SusViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            string cSharpCode = GenerateCSharpFromTemplate(templateCode);
            IView executableObject = GenerateExecutableCode(cSharpCode,viewModel);
            string html = executableObject.ExecuteTemplate(viewModel);
            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            string methodBody = GetMethodBody(templateCode);
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
                                
                                            + methodBody + 

                                            @"
                                            return html.ToString();
                                        }
                                    }
                                }
                                ";
            return cSharpCode;

        }

        private string GetMethodBody(string templateCode)
        {
            return string.Empty;
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
            if (viewModel!=null)
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
                EmitResult emitResult =  compileResult.Emit(memoryStream);
                if (!emitResult.Success)
                {
                    return new ErrorView(
                        emitResult.Diagnostics
                        .Where(err=>err.Severity==DiagnosticSeverity.Error) 
                        .Select(mes=>mes.GetMessage())
                        , cSharpCode);
                }
                
                memoryStream.Seek(0, SeekOrigin.Begin);
                var byteAssembly = memoryStream.ToArray();
                var assembly = Assembly.Load(byteAssembly);
                var viewType = assembly.GetType("ViewNamespace.ViewClass");
                var instance = Activator.CreateInstance(viewType);
                return instance as IView;
            }
                
        }
    }
}
