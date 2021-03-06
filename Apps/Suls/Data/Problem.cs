﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suls.Data
{
//    •	Has an Id – a string, Primary Key
//•	Has a Name – a string with min length 5 and max length 20 (required)
//•	Has Points– an integer between 50 and 300 (required)


    public class Problem
    {
        public Problem()
        {
            this.Submissions = new HashSet<Submission>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public int Points { get; set; }

        public ICollection<Submission> Submissions { get; set; }
    }
}