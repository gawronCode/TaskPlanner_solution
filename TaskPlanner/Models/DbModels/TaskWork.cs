using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Models.DbModels
{
    public class TaskWork
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string WorkDescription { get; set; }
        public DateTime? StartDate { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
        [Required]
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
