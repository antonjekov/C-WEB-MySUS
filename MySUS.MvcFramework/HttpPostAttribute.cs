using MySUS.HTTP;

namespace MySUS.MvcFramework
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public HttpPostAttribute()
        {

        }

        public HttpPostAttribute(string url)
        {
            this.Url = url;
        }

        public override HttpMethod Method => HttpMethod.POST;
    }
}
