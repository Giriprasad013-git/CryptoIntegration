using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoDepositApp.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }

    public enum TransactionStatus
    {
        AwaitingPayment,
        Completed,
        Failed
    }

    public enum Network
    {
        PolygonPOS,
        Tron,
        Ethereum,
        BEP20
    }

    public enum TokenType
    {
        USDT,
        USDC
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public TokenType Token { get; set; }
        public Network Network { get; set; }
        public string WalletAddress { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string TXID { get; set; }
        public decimal ExchangeRate { get; set; }

        public Transaction()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Status = TransactionStatus.AwaitingPayment;
            // Default exchange rate (in a real app this would come from a price feed)
            ExchangeRate = 0.9994835m;
        }
    }
}
