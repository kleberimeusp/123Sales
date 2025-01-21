using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Models
{
    [Table("Sales")]  
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }  

        [Required]
        public int SaleNumber { get; set; }  

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
