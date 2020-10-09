using System;

namespace Web.Models
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreateAt { get; set; }
        DateTime UpdateAt { get; set; }
        DateTime? DeleteAt { get; set; }
        bool IsDeleted { get; }
        string UserId { get; set; }
        User User { get; set; }
    }
}