using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVCDHProject.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Custid { get;set; }

        [MaxLength(100)]
        [Column(TypeName ="varchar")]
        public string? Name { get;set; }

        [Column(TypeName ="Money")]
        public decimal? Balance { get;set; }

        [MaxLength(100)]
        [Column(TypeName ="varchar")]
        public string? City { get; set; }
        public bool Status { get; set; }

    }
}
