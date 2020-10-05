using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.HTTP
{
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value):base(name,value)
        {
            this.Path = "/";
        }

        //Max-Age
        public int MaxAge { get; set; }

        public bool HttpOnly { get; set; }

        public string Path { get; set; }

        //Domain, Secure ...

        //sid=sid; Max-Age= {100*24*60*60}; HttpOnly; SameSite = Strict" 
        public override string ToString()
        {
            StringBuilder cookieBuilder = new StringBuilder();
            cookieBuilder.Append(base.ToString()+$" Path={this.Path};") ;
            if (MaxAge!=0)
            {
                cookieBuilder.Append($" Max-Age={this.MaxAge};");
            }
            if (this.HttpOnly)
            {
                cookieBuilder.Append(" HttpOnly;");
            }

            return cookieBuilder.ToString();
        }

    }
}
