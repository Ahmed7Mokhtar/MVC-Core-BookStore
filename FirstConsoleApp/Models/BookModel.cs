using FirstConsoleApp.Enums;
using FirstConsoleApp.Helpers;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FirstConsoleApp.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        // title must have 'book' after title
        //[MyCustomValidation("book")]
        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please Enter the Title!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Please Enter the Author name!")]
        public string? Author { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public string? Category { get; set; }
        
        [Required(ErrorMessage = "Please choose the language of your book")]
        public int LanguageId { get; set; }

        public string? Language { get; set; }

        //[Required(ErrorMessage = "Please choose the language of your book")]
        //public LanguageEnum LanguageEnum { get; set; }

        [Required(ErrorMessage = "Please Enter the Total pages!")]
        // changes label in form
        [Display(Name = "Total Pages")]
        public int? TotalPages { get; set; }

        [Display(Name = "Choose the cover photo of your book")]
        [Required]
        public IFormFile CoverPhoto { get; set; }

        [Display(Name = "Choose the gallery images of your book")]
        [Required]
        public IFormFileCollection GalleryFiles { get; set; }

        public string? CoverImageUrl { get; set; }

        public List<GalleryModel>? Galleries { get; set; }

        [Display(Name = "Upload your book in PDF format")]
        [Required]
        public IFormFile BookPdf { get; set; }

        public string? BookPdfUrl { get; set; }

        // change field datatype in form (enum param DataType)
        // DataType is not used to validate the data
        //[DataType(DataType.EmailAddress)]
        //[Display(Name = "Choose date")]
        //// to validate email
        //[EmailAddress]
        //public string? MyField { get; set; }
    }
}
