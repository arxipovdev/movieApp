using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Movie : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public string Post { get; set; }
        
        [Required]
        public DateTime CreateAt { get; set; }
        
        [Required]
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }

        [Required]
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
        public bool IsDeleted => DeleteAt != null;
    }
}