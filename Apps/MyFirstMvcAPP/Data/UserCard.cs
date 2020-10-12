using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BattleCards.Data
{
//    •	Has UserId – a string
//•	Has User – a User object
//•	Has CardId – an int
//•	Has Card – a Card object


    public class UserCard
    {
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}
