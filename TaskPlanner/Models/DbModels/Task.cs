using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models.DbModels
{
    public class Task
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public bool IsFinished { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
