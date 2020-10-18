using Suls.Data;
using Suls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suls.Services
{
    public class SubmissionServices : ISubmissionServices
    {
        private ApplicationDbContext db;
        private IProblemService problemService;
        private Random random;

        public SubmissionServices(ApplicationDbContext db, IProblemService problemService, Random random)
        {
            this.db = db;
            this.problemService = problemService;
            this.random = random;
        }

        public string Create(string code, string problemId,string userId)
        {
            var problem = this.problemService.GetProblemById(problemId);
            var submission = new Submission()
            {
                Code = code,
                ProblemId = problemId,
                UserId = userId,
                AchievedResult = random.Next(0,problem.Points)
            };

            this.db.Submissions.Add(submission);
           db.SaveChanges();
            return submission.Id;
        }

        public IEnumerable<SubmissionDetailsViewModel> GetSubmissionsByProblemId(string problemId)
        {
            var result =  this.db.Submissions.Where(x => x.ProblemId == problemId).Select(x => new SubmissionDetailsViewModel()
            {
                SubmissionId = x.Id,
                Username = x.User.Username,
                CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy"),
                AchievedResult = (int)Math.Round(((double)x.AchievedResult/x.Problem.Points)*100),
                MaxPoints = 100
            }).ToList();
            return result;
        }

        public void Delete(string submissionId)
        {
            var submission = this.db.Submissions.FirstOrDefault(x => x.Id == submissionId);
            if (submission!=null)
            {
                this.db.Submissions.Remove(submission);
                this.db.SaveChanges();
            }
            
        }
    }
}
