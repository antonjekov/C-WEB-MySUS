using MySUS.HTTP;
using MySUS.MvcFramework;
using Suls.Data;
using Suls.Services;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Suls.ViewModels;

namespace Suls.Controllers
{
    public class ProblemsController : Controller
    {
        private IProblemService problemService;
        private ISubmissionServices submissionServices;

        public ProblemsController(IProblemService problemService, ISubmissionServices submissionServices)
        {
            this.problemService = problemService;
            this.submissionServices = submissionServices;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, int points)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            if (string.IsNullOrWhiteSpace(name)|| name.Length<5||name.Length>20)
            {
                return this.Error("Name should be between 5 and 20 charackters.");
            }
            if (points<50||points>300)
            {
                return this.Error("Points should be number between 50 and 300");
            }

            this.problemService.CreateProblem(name, points);
            return this.Redirect("/");
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var problem = this.problemService.GetProblemById(id);
            var submissionsForProblem = this.submissionServices.GetSubmissionsByProblemId(id);
            var problemDetails = new ProblemDetailsViewModel()
            {
                Name = problem.Name,
                Submissions = submissionsForProblem
            };
            return this.View(problemDetails);
        }
    }
}
