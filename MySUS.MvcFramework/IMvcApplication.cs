using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route>routeTable);
    }
}
