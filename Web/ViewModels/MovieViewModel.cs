using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class MovieViewModel
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
        
        public string Post { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режисер")]
        public int ProducerId { get; set; }
        
        public string ProducerName { get; set; }
        public bool IsEdited { get; set; }
        
        [Required(ErrorMessage = "Выберите постер")]
        [DataType(DataType.Upload)]
        [Display(Name = "Постер")]
        public IFormFile File { get; set; }
    }
}