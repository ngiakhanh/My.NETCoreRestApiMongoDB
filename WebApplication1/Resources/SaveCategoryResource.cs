using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Mapping
{
    public class SaveCategoryResource : IValidatableObject
    {
        [Required]
        [Range(1, 100)]
        public int Id { get; set; }
        

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Id < 1 || Id > 100)
            {
                results.Add(new ValidationResult("Id must be between 1 and 100."));
            }
            return results;
        }
    }
}
