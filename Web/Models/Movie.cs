using System;

namespace Web.Models
{
    public class Movie : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public Guid Post { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        
        public bool IsDeleted => DeleteAt != null;
    }
}