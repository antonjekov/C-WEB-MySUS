using System;

namespace MySUS.HTTP
{
    public class Header
    {
        public Header(string headerLine)
        {
            //Accept-Encoding: gzip, deflate, br
            var headerParts = headerLine.Split(new string[] { ": "},2, StringSplitOptions.None);
            this.Name = headerParts[0];
            this.Value = headerParts[1];
        }

        public Header(string name, string value)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}";
        }
    }
}