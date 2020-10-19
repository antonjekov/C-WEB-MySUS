using Suls.Data;
using Suls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Suls.Services
{
    public class ProblemService : IProblemService
    {
        private ApplicationDbContext db;

        public ProblemService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string CreateProblem(string name, int points)
        {
            var problem = new Problem() { Name = name, Points = points };
            this.db.Problems.Add(problem);
            db.SaveChanges();
            return problem.Id;
        }

        public IEnumerable<ProblemViewModel> GetAllProblems()
        {
            return this.db.Problems.Select(x => new ProblemViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Submissions.Count
            }).ToList();
        }

        public Problem GetProblemById(string id)
        {
            return this.db.Problems.FirstOrDefault(x => x.Id == id);
        }
    }
}
