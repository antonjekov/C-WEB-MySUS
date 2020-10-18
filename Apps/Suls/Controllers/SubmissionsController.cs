using MySUS.HTTP;
using MySUS.MvcFramework;
using Suls.Services;
using Suls.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Controllers
{
    public class SubmissionsController: Controller
    {
        private ISubmissionServices submissionServices;
        private IProblemService problemServices;

        public SubmissionsController(ISubmissionServices submissionServices, IProblemService problemService)
        {
            this.submissionServices = submissionServices;
            this.problemServices = problemService;
        }

        public HttpResponse Create(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var problem = problemServices.GetProblemById(id);
            return this.View(problem);
        }

        [HttpPost]
        public HttpResponse Create(string code, string problemId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            if (string.IsNullOrWhiteSpace(code)|| code.Length<30|| code.Length>800)
            {
                return this.Error("Code should be between 30 and 800 characters long.");
            }
            
            this.submissionServices.Create(code, problemId, this.GetUserId());
            return this.Redirect("/");
        }

        public HttpResponse Delete(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            this.submissionServices.Delete(id);
            return this.Redirect("/");
        }
    }
}
