using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework
{
    public abstract class BaseHttpAttribute: Attribute
    {
        public string Url { get; set; }

        public abstract HttpMethod Method { get;}
    }
}
