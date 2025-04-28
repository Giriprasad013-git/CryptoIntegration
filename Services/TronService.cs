using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using TronNet;
using TronNet.Accounts;
using TronNet.Crypto;
using CryptoDepositApp.Models;

namespace CryptoDepositApp.Services
{
    public interface ITronService
    {
        Task<string> GenerateDepositAddress(string seedPhrase, int addressIndex);
        Task<bool> VerifyDeposit(string txHash);
        Task<decimal> GetTokenBalance(string address, string contractAddress);
    }

    public class TronService : ITronService
    {
        private readonly ILogger<TronService> _logger;
        private readonly ITronClient _tronClient;
        private readonly IWalletClient _walletClient;
        private readonly HttpClient _httpClient;
        private readonly string _tronGridApiKey;
        
        // USDT contract address on Tron
        private const string USDT_CONTRACT_ADDRESS = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t";
        // USDC contract address on Tron
        private const string USDC_CONTRACT_ADDRESS = "TEkxiTehnzSmSe2XqrBj4w32RUN966rdz8";

        // TronGrid API base URL
        private const string TRONGRID_API_URL = "https://api.trongrid.io";

        public TronService(ITronClient tronClient, IWalletClient walletClient, ILogger<TronService> logger, IConfiguration configuration, HttpClient httpClient = null)
        {
            _tronClient = tronClient;
            _walletClient = walletClient;
            _logger = logger;
            _tronGridApiKey = configuration["TRONGRID_API_KEY"] ?? string.Empty;
            
            // Create HttpClient if not provided
            _httpClient = httpClient ?? new HttpClient();
            
            if (!string.IsNullOrEmpty(_tronGridApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("TRON-PRO-API-KEY", _tronGridApiKey);
            }
            
            _httpClient.BaseAddress = new Uri(TRONGRID_API_URL);
        }

        /// <summary>
        /// Generates a Tron address from a seed phrase
        /// </summary>
        public async Task<string> GenerateDepositAddress(string seedPhrase, int addressIndex)
        {
            try
            {
                // In a real implementation, we would derive the key from the seed phrase
                // For Tron, we'd need to use their specific HD wallet implementation
                // This is just a placeholder for the implementation logic

                // Create a deterministic address by hashing the seed phrase with the index
                using var sha256 = SHA256.Create();
                var combinedSeed = seedPhrase + addressIndex.ToString();
                var bytes = Encoding.UTF8.GetBytes(combinedSeed);
                var hash = sha256.ComputeHash(bytes);
                var hexPrivateKey = BitConverter.ToString(hash).Replace("-", "").ToLower();

                // Generate a fixed address for demo purposes
                return "TJCnKsPa7y5okkXvQAidZBzqx3QyQ6sxMW"; // Demo address
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex, "Error generating TRON deposit address");
                }
                throw new InvalidOperationException("Unable to generate TRON deposit address");
            }
        }

        /// <summary>
        /// Verifies a TRC20 token deposit transaction using TronGrid API
        /// </summary>
        public async Task<bool> VerifyDeposit(string txHash)
        {
            try
            {
                // Get transaction info from TronGrid API
                var response = await _httpClient.GetAsync($"/v1/transactions/{txHash}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to verify transaction {txHash}. Status code: {response.StatusCode}");
                    return false;
                }
                
                var content = await response.Content.ReadAsStringAsync();
                var txInfo = JsonSerializer.Deserialize<JsonElement>(content);
                
                // Check if transaction exists and is confirmed
                if (!txInfo.TryGetProperty("data", out var data) || data.GetArrayLength() == 0)
                {
                    _logger.LogWarning($"Transaction {txHash} not found or has no data");
                    return false;
                }
                
                var txData = data[0];
                
                // Check transaction status (1 = confirmed/success)
                if (!txData.TryGetProperty("ret", out var retArray) || retArray.GetArrayLength() == 0)
                {
                    return false;
                }
                
                var ret = retArray[0];
                if (!ret.TryGetProperty("contractRet", out var contractRet) || 
                    contractRet.GetString() != "SUCCESS")
                {
                    return false;
                }
                
                // Add additional checks as needed (recipient address, amount, token contract, etc.)
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying TRON deposit for transaction {txHash}");
                return false;
            }
        }

        /// <summary>
        /// Gets the TRC20 token balance for an address using TronGrid API
        /// </summary>
        public async Task<decimal> GetTokenBalance(string address, string contractAddress = USDT_CONTRACT_ADDRESS)
        {
            try
            {
                // TRC20 tokens require contract call to get balance
                // We'll use TronGrid API to call the contract
                
                // Prepare request body for contract call
                var requestData = new
                {
                    owner_address = address,
                    contract_address = contractAddress,
                    function_selector = "balanceOf(address)",
                    parameter = TronParameters.ToHex(address), // Need to encode address as parameter
                    visible = true
                };
                
                var requestContent = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");
                
                // Call TronGrid API to get token balance
                var response = await _httpClient.PostAsync("/wallet/triggersmartcontract", requestContent);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get balance for {address}. Status code: {response.StatusCode}");
                    return 0;
                }
                
                var content = await response.Content.ReadAsStringAsync();
                var balanceInfo = JsonSerializer.Deserialize<JsonElement>(content);
                
                // Extract balance from response (will be in smallest unit, like SUN for TRX)
                if (balanceInfo.TryGetProperty("constant_result", out var resultArray) && 
                    resultArray.GetArrayLength() > 0)
                {
                    var hexBalance = resultArray[0].GetString();
                    var decimalBalance = Convert.ToInt64(hexBalance, 16);
                    
                    // Convert to standard unit (assuming 6 decimals for USDT/USDC)
                    return decimalBalance / 1_000_000m;
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting TRC20 balance for {address} with contract {contractAddress}");
                throw new InvalidOperationException("Unable to retrieve TRON token balance");
            }
        }
        
        // Helper class for parameter conversion
        private static class TronParameters
        {
            // Convert address to hex parameter for contract calls
            public static string ToHex(string address)
            {
                // This is simplified - in real implementation we'd:
                // 1. Convert Tron address from base58 to hex
                // 2. Ensure proper parameter encoding for the contract call
                // 3. Return properly formatted parameter
                
                // For now, return a dummy parameter to avoid compilation errors
                return "0000000000000000000000000000000000000000000000000000000000000000";
            }
        }
    }
}