using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoDepositApp.Models;
using Microsoft.Extensions.Logging;

namespace CryptoDepositApp.Services
{
    public interface ITokenListingService
    {
        Task<IEnumerable<TokenListing>> GetTokenListings();
        Task<IEnumerable<TokenListing>> GetTokenListingsByNetwork(string network);
        Task<IEnumerable<TokenListing>> GetTokenListingsBySymbol(string symbol);
        Task<TokenListing> GetTokenListing(string symbol, string network);
        Task<IEnumerable<TokenNetwork>> GetAvailableNetworks();
    }

    public class TokenListingService : ITokenListingService
    {
        private readonly ILogger<TokenListingService> _logger;
        private readonly List<TokenListing> _tokenListings;
        private readonly List<TokenNetwork> _networks;

        // USDT Contract Addresses
        private const string USDT_ETH_CONTRACT = "0xdAC17F958D2ee523a2206206994597C13D831ec7";
        private const string USDT_POLYGON_CONTRACT = "0xc2132D05D31c914a87C6611C10748AEb04B58e8F";
        private const string USDT_TRON_CONTRACT = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t";
        
        // USDC Contract Addresses
        private const string USDC_ETH_CONTRACT = "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48";
        private const string USDC_POLYGON_CONTRACT = "0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174";
        private const string USDC_TRON_CONTRACT = "TEkxiTehnzSmSe2XqrBj4w32RUN966rdz8";

        public TokenListingService(ILogger<TokenListingService> logger)
        {
            _logger = logger;
            
            // Initialize network information
            _networks = new List<TokenNetwork>
            {
                new TokenNetwork
                {
                    NetworkName = "Ethereum",
                    NetworkDescription = "Ethereum Mainnet",
                    ChainId = "1",
                    ExplorerUrl = "https://etherscan.io",
                    RpcUrl = "https://mainnet.infura.io/v3/{YOUR_INFURA_KEY}",
                    IsActive = true
                },
                new TokenNetwork
                {
                    NetworkName = "Polygon",
                    NetworkDescription = "Polygon Mainnet (formerly Matic)",
                    ChainId = "137",
                    ExplorerUrl = "https://polygonscan.com",
                    RpcUrl = "https://polygon-rpc.com",
                    IsActive = true
                },
                new TokenNetwork
                {
                    NetworkName = "Tron",
                    NetworkDescription = "Tron Mainnet",
                    ChainId = "MainNet",
                    ExplorerUrl = "https://tronscan.org",
                    RpcUrl = "grpc.trongrid.io:50051",
                    IsActive = true
                }
            };
            
            // Initialize token listings
            _tokenListings = new List<TokenListing>
            {
                // USDT on Ethereum
                new TokenListing
                {
                    TokenSymbol = "USDT",
                    TokenName = "Tether USD",
                    Network = "Ethereum",
                    ContractAddress = USDT_ETH_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 10.0m,
                    WithdrawalFee = 2.5m,
                    GasEstimate = 0.001m,
                    LastUpdated = DateTime.UtcNow
                },
                
                // USDT on Polygon
                new TokenListing
                {
                    TokenSymbol = "USDT",
                    TokenName = "Tether USD",
                    Network = "Polygon",
                    ContractAddress = USDT_POLYGON_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 5.0m,
                    WithdrawalFee = 1.0m,
                    GasEstimate = 0.0001m,
                    LastUpdated = DateTime.UtcNow
                },
                
                // USDT on Tron
                new TokenListing
                {
                    TokenSymbol = "USDT",
                    TokenName = "Tether USD",
                    Network = "Tron",
                    ContractAddress = USDT_TRON_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 1.0m,
                    WithdrawalFee = 0.5m,
                    GasEstimate = 0.00001m,
                    LastUpdated = DateTime.UtcNow
                },
                
                // USDC on Ethereum
                new TokenListing
                {
                    TokenSymbol = "USDC",
                    TokenName = "USD Coin",
                    Network = "Ethereum",
                    ContractAddress = USDC_ETH_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 10.0m,
                    WithdrawalFee = 2.5m,
                    GasEstimate = 0.001m,
                    LastUpdated = DateTime.UtcNow
                },
                
                // USDC on Polygon
                new TokenListing
                {
                    TokenSymbol = "USDC",
                    TokenName = "USD Coin",
                    Network = "Polygon",
                    ContractAddress = USDC_POLYGON_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 5.0m,
                    WithdrawalFee = 1.0m,
                    GasEstimate = 0.0001m,
                    LastUpdated = DateTime.UtcNow
                },
                
                // USDC on Tron
                new TokenListing
                {
                    TokenSymbol = "USDC",
                    TokenName = "USD Coin",
                    Network = "Tron",
                    ContractAddress = USDC_TRON_CONTRACT,
                    Decimals = 6,
                    IsActive = true,
                    MinimumDeposit = 1.0m,
                    WithdrawalFee = 0.5m,
                    GasEstimate = 0.00001m,
                    LastUpdated = DateTime.UtcNow
                }
            };
        }

        /// <summary>
        /// Get all token listings
        /// </summary>
        public async Task<IEnumerable<TokenListing>> GetTokenListings()
        {
            try
            {
                return _tokenListings.Where(t => t.IsActive);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving token listings");
                return Enumerable.Empty<TokenListing>();
            }
        }

        /// <summary>
        /// Get token listings filtered by network
        /// </summary>
        public async Task<IEnumerable<TokenListing>> GetTokenListingsByNetwork(string network)
        {
            try
            {
                return _tokenListings.Where(t => t.IsActive && t.Network.Equals(network, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error retrieving token listings for network: {network}");
                return Enumerable.Empty<TokenListing>();
            }
        }

        /// <summary>
        /// Get token listings filtered by symbol (USDT, USDC)
        /// </summary>
        public async Task<IEnumerable<TokenListing>> GetTokenListingsBySymbol(string symbol)
        {
            try
            {
                return _tokenListings.Where(t => t.IsActive && t.TokenSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error retrieving token listings for symbol: {symbol}");
                return Enumerable.Empty<TokenListing>();
            }
        }

        /// <summary>
        /// Get specific token listing by symbol and network
        /// </summary>
        public async Task<TokenListing> GetTokenListing(string symbol, string network)
        {
            try
            {
                return _tokenListings.FirstOrDefault(t => 
                    t.IsActive && 
                    t.TokenSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase) && 
                    t.Network.Equals(network, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error retrieving token listing for {symbol} on {network}");
                return null;
            }
        }

        /// <summary>
        /// Get available networks
        /// </summary>
        public async Task<IEnumerable<TokenNetwork>> GetAvailableNetworks()
        {
            try
            {
                return _networks.Where(n => n.IsActive);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving available networks");
                return Enumerable.Empty<TokenNetwork>();
            }
        }
    }
}