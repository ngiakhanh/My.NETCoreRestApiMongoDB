using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Models;

namespace WebApplication1.Resources
{
    public class SaveProductResource : IValidatableObject
    {
        [Required]
        [Range(1, 100)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public short QuantityInPackage { get; set; }

        [Required]
        public EUnitOfMeasurement UnitOfMeasurement { get; set; }

        [Required]
        [Range(1, 100)]
        public int CategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Id < 1 || Id > 100)
            {
                results.Add(new ValidationResult("Id must be between 1 and 100."));
            }
            if (QuantityInPackage < 1 || QuantityInPackage > 1000)
            {
                results.Add(new ValidationResult("Quantity in Package must be between 1 and 1000."));
            }
            if (!System.Enum.IsDefined(typeof(EUnitOfMeasurement), UnitOfMeasurement))
            {
                results.Add(new ValidationResult("Unit of Measurement must be between 1 and 5."));
            }
            if (CategoryId < 1 || CategoryId > 100)
            {
                results.Add(new ValidationResult("Category Id must be between 1 and 100."));
            }
            return results;
        }
    }
}