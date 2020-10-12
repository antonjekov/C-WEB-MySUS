using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace BattleCards.Data
{

//    •	Has Id – an int, Primary Key
//•	Has Name – a string (required); min.length: 5, max.length: 15
//•	Has ImageUrl – a string (required)
//•	Has Keyword – a string (required)
//•	Has Attack – an int (required); cannot be negative
//•	Has Health – an int (required); cannot be negative
//•	Has a Description – a string with max length 200 (required)
//•	Has UserCard collection

    public class Card
    {
        public Card()
        {
            this.Users = new HashSet<UserCard>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Keyword { get; set; }

        [Required]
        public int Attack { get; set; }

        [Required]
        public int Health { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<UserCard> Users { get; set; }
    }
}
