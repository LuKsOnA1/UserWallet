using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public int SenderUserId { get; set; }
        public int RecipientUserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime DateOfTransfer { get; set; }
    }
}
