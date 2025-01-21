using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Models
{
    [Table("Sales")]  // Maps to the database table "Sales"
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }  // Unique Identifier

        [Required]
        public int SaleNumber { get; set; }  // Sale number (for display)

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Customer { get; set; }

        [Required]
        public decimal TotalSaleValue { get; set; }

        [Required]
        [MaxLength(50)]
        public string Branch { get; set; }

        // Ensuring it's not null
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        public bool IsCanceled { get; set; }
    }
}
