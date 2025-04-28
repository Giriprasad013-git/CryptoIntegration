using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoDepositApp.Models;

namespace CryptoDepositApp.Services
{
    public interface ICryptoService
    {
        Task<Transaction> CreateDepositRequest(DepositRequest request);
        Task<Transaction> CreateWithdrawalRequest(WithdrawalRequest request);
        Task<IEnumerable<Transaction>> GetUserTransactions();
        Task<Transaction> GetTransaction(Guid id);
        Task<decimal> GetAvailableBalance();
        string GenerateDepositAddress(TokenType token, Network network);
    }

    public class CryptoService : ICryptoService
    {
        // In-memory storage of transactions
        private readonly List<Transaction> _transactions = [];
        
        // Mock deposit addresses for different networks (in a real implementation these would be generated)
        private readonly Dictionary<(TokenType, Network), string> _depositAddresses = new Dictionary<(TokenType, Network), string>
        {
            { (TokenType.USDT, Network.PolygonPOS), "0x490d1ac37173746cf0e1207baebe2cb55a3de351" },
            { (TokenType.USDT, Network.Tron), "TXu9xgKVtoXiZiJGNRxoJB1nAaYRTrNrqQ" },
            { (TokenType.USDT, Network.Ethereum), "0x3e465106053762319F29C2de564bD921D9AF6d22" },
            { (TokenType.USDT, Network.BEP20), "0x8936cE4f0668DD58fE1B498b12A0aA6c9D4AB772" },
            { (TokenType.USDC, Network.PolygonPOS), "0x982746cF23E9129CD08A24d8DB0A9D3D84F33B6F" },
            { (TokenType.USDC, Network.BEP20), "0x7De4A907e428390c2B48920aF0417A97F84cCB14" }
        };

        // Mock initial balance for demo purposes
        private decimal _userBalance = 1587.44m;

        public CryptoService()
        {
            // Initialize with some example transactions for development
            // In a real app, this would be loaded from a database
            // Intentionally left empty to avoid mocked data in production
        }

        public async Task<Transaction> CreateDepositRequest(DepositRequest request)
        {
            var transaction = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = request.Amount,
                Token = request.Token,
                Network = request.Network,
                WalletAddress = GenerateDepositAddress(request.Token, request.Network),
                TXID = $"{new Random().Next(1000000, 9999999)}",
                Status = TransactionStatus.AwaitingPayment
            };

            _transactions.Add(transaction);
            return await Task.FromResult(transaction);
        }

        public async Task<Transaction> CreateWithdrawalRequest(WithdrawalRequest request)
        {
            // Validate available balance
            if (request.Amount > _userBalance)
            {
                throw new InvalidOperationException("Insufficient balance for withdrawal");
            }

            var transaction = new Transaction
            {
                Type = TransactionType.Withdrawal,
                Amount = request.Amount,
                Token = request.Token,
                Network = request.Network,
                WalletAddress = request.WalletAddress,
                Status = TransactionStatus.AwaitingPayment
            };

            _transactions.Add(transaction);
            _userBalance -= request.Amount;
            
            return await Task.FromResult(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactions()
        {
            return await Task.FromResult(_transactions.OrderByDescending(t => t.CreatedAt));
        }

        public async Task<Transaction> GetTransaction(Guid id)
        {
            return await Task.FromResult(_transactions.FirstOrDefault(t => t.Id == id));
        }

        public async Task<decimal> GetAvailableBalance()
        {
            return await Task.FromResult(_userBalance);
        }

        public string GenerateDepositAddress(TokenType token, Network network)
        {
            // In a real implementation, this would generate a unique deposit address
            // For demo purposes, we're using pre-defined addresses
            if (_depositAddresses.TryGetValue((token, network), out string address))
            {
                return address;
            }
            
            // Fallback to a random address if not found
            return $"0x{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 40)}";
        }
    }
}
