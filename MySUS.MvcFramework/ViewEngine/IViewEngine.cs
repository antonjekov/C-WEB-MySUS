using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework.ViewEngine
{
    public interface IViewEngine
    {
        string GetHtml(string templateCode, object viewModel, string user);
    }
}
