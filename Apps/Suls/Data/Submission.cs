using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suls.Data
{
//    •	Has an Id – a string, Primary Key
//•	Has Code – a string with min length 30 and max length 800 (required)
//•	Has Achieved Result – an integer between 0 and 300 (required)
//•	Has a Created On – a DateTime object (required)
//•	Has Problem – a Problem object
//•	Has User – a User object

    public class Submission
    {
        public Submission()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(800)]
        public string Code { get; set; }

        public int AchievedResult { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string ProblemId { get; set; }
        public Problem Problem { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}