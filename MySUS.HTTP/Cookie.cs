using System;

namespace MySUS.HTTP
{
    //cookie: AMP_TOKEN=%24NOT_FOUND; _ga=GA1.2.1287458627.1601471589; _gid=GA1.2.828868538.1601471589; __gfp_64b=zKfVQq_h9o78kQqDrmwVzhvihJUXl.Tqs98bV_4AnPj.I7; _gat=1; _fbp=fb.1.1601471589249.564787253; CUID=N,1601471589449:ALHGLuQAAAAPTiwxNjAxNDcxNTg5NDQ5qMUiV6uFKNcLOmzY07j2MJzSrxVz/6fpg+XqQek80BZOUJaM/YiGX5zIHdTd1Erae/RvFpXSRroTJMOnvcT5J1cMw429hN/RYUw1GzGTQ6NnSuCOwoT3a3jtf/9FgFA3GAbGb8LVWqy11fHfhEdYx+A8CWmelo/xPtZMroAexx6CJ1gWcDj9tOn9HC4MXzI7bmlz+XEM0TYXgAViCLEnUCib06BqftvAM5xHZyo+t02j7SPDOxFu/Vgdh4iymD7XAR0DsuSLWDVo7ZWaSN1s/NcU9lldi56LrZKKTLfcWSsWjuyqGO/t10hYFnhMam/YCtDw2g0kgXek6BIs8zqzcA==; __qca=P0-699245158-1601471589199; FCCDCF=[["AKsRol9mbJtFaW5d8LnjH7HcBJqvuwfr3uJhnMa0Q4Zkht8EwiBQtso-ZLXxCPtT6VmOvIK9rReHipB8yoPfGVjIPGImusBQ-ftdDvTLyuXVhLF1ACzay8lLhtXWpqMJHh9IegV2anHTsXkzgsLal0LB8a41alDwsA=="],null,["[[],[],[],[],null,null,true]",1601471589978]]; __gads=ID=acdba94f0e085dde:T=1601471589:S=ALNI_MYT0i7AheWHuBIp37TJj2tukVf8CA
    public class Cookie
    {
        public Cookie(string cookieValue)
        {
            var cookieParts = cookieValue.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
            this.Name = cookieParts[0];
            this.Value = cookieParts[1];
        }

        public Cookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}={this.Value};";
        }
    }
}