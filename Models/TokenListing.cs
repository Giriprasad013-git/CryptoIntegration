using System;

namespace CryptoDepositApp.Models
{
    public class TokenListing
    {
        public string TokenSymbol { get; set; }
        public string TokenName { get; set; }
        public string Network { get; set; }
        public string ContractAddress { get; set; }
        public int Decimals { get; set; }
        public bool IsActive { get; set; }
        public decimal MinimumDeposit { get; set; }
        public decimal WithdrawalFee { get; set; }
        public decimal GasEstimate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class TokenNetwork
    {
        public string NetworkName { get; set; }
        public string NetworkDescription { get; set; }
        public string ChainId { get; set; }
        public string ExplorerUrl { get; set; }
        public string RpcUrl { get; set; }
        public bool IsActive { get; set; }
    }
}