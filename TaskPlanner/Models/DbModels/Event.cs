using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models.DbModels
{
    public class Event
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime? EventDate { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
