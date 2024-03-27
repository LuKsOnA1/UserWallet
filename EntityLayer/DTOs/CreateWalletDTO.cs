using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace EntityLayer.DTOs
{
    public class CreateWalletDTO
    {
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
