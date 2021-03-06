using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Producer : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        
        [Required]
        public DateTime CreateAt { get; set; }
        
        [Required]
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
        public string Name => $"{FirstName} {LastName} {MiddleName}";
        public bool IsDeleted => DeleteAt != null;
    }
}