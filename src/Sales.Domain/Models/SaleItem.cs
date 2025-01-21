using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Models
{
    [Table("SaleItems")]  // Maps to the database table "SaleItems"
    public class SaleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();  // Changed to Guid

        [Required]
        public Guid SaleId { get; set; }  // Foreign Key to Sale (Now Guid)

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        // Navigation Property
        [ForeignKey("SaleId")]
        public Sale Sale { get; set; }
    }
}
