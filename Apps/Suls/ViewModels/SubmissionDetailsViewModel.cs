using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.ViewModels
{
    public class SubmissionDetailsViewModel
    {
        public string Username { get; set; }

        public int AchievedResult { get; set; }

        public int MaxPoints { get; set; }

        public string SubmissionId { get; set; }

        public string CreatedOn { get; set; }
    }
}
