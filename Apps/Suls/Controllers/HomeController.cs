using MySUS.HTTP;
using MySUS.MvcFramework;
using Suls.Services;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        private IProblemService problemService;

        public HomeController(IProblemService problemService)
        {
            this.problemService = problemService;
        }
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var allProblems = this.problemService.GetAllProblems();
                return this.View(allProblems, "IndexLoggedIn");
            }
            return this.View();
        }

    }
}
