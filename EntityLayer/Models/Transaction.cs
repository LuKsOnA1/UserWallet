using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }


        [Required]
        public Guid SenderUserId { get; set; }
        [ForeignKey("SenderUserId")]
        public User SenderUser { get; set; }


        [Required]
        public Guid RecipientUserId { get; set; }
        [ForeignKey("RecipientUserId")]
        public User RecipientUser { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime DateOfTransfer { get; set; }
    }
}
