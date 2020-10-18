using Suls.Data;
using Suls.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Services
{
    public interface ISubmissionServices
    {
        IEnumerable<SubmissionDetailsViewModel> GetSubmissionsByProblemId(string problemId);

        public string Create(string code, string problemId, string userId);

        public void Delete(string submissionId);
    }
}
