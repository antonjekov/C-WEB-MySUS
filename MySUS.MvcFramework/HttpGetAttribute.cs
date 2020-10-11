using MySUS.HTTP;

namespace MySUS.MvcFramework
{
    public class HttpGetAttribute : BaseHttpAttribute
    {
        public HttpGetAttribute()
        {

        }
        public HttpGetAttribute(string url)
        {
            this.Url = url;
        }

        public override HttpMethod Method => HttpMethod.GET;
    }
}
