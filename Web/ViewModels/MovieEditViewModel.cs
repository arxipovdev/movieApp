using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class MovieEditViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год")]
        public int Year { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Загрузите пост")]
        public string Post { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режисер")]
        public int ProducerId { get; set; }
    }
}