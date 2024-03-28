using EntityLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.DTOs
{
    public class TransferMoneyDTO
    {
        [Required]
        public Guid SenderUserId { get; set; }
        [ForeignKey("SenderUserId")]


        [Required]
        public Guid RecipientUserId { get; set; }
        [ForeignKey("RecipientUserId")]


        public Guid WalletId { get; set; }
        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime DateOfTransfer { get; set; }
    }
}
