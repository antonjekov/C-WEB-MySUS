using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework.ViewEngine
{
    public interface IView
    {
        string ExecuteTemplate(object viewModel, string user);
    }
}
