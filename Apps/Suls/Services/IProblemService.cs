using Suls.Data;
using Suls.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Services
{
    public interface IProblemService
    {
        string CreateProblem(string name, int points);

        IEnumerable<ProblemViewModel> GetAllProblems();

        Problem GetProblemById(string id);


    }
}
