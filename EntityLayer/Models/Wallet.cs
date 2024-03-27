using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
