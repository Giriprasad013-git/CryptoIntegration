using System.ComponentModel.DataAnnotations;

namespace CryptoDepositApp.Models
{
    public class DepositRequest
    {
        [Required]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Required]
        public TokenType Token { get; set; }

        [Required]
        public Network Network { get; set; }
    }
}
