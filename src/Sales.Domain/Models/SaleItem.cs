using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Models
{
    [Table("SaleItems")]  
    public class SaleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();  

        [Required]
        public Guid SaleId { get; set; }  

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
