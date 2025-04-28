using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptoDepositApp.Models;
using CryptoDepositApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoDepositApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly ILogger<CryptoController> _logger;

        public CryptoController(ICryptoService cryptoService, ILogger<CryptoController> logger)
        {
            _cryptoService = cryptoService;
            _logger = logger;
        }

        [HttpPost("deposit")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<Transaction>> CreateDeposit(DepositRequest request)
        {
            try
            {
                // Validate network status
                if (!await _cryptoService.IsNetworkOperational(request.Network))
                {
                    return StatusCode(503, new { message = "Selected network is currently unavailable" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var transaction = await _cryptoService.CreateDepositRequest(request);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating deposit request");
                return StatusCode(500, new { message = "An error occurred while processing your deposit request." });
            }
        }

        [HttpPost("withdrawal")]
        public async Task<ActionResult<Transaction>> CreateWithdrawal(WithdrawalRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var transaction = await _cryptoService.CreateWithdrawalRequest(request);
                return Ok(transaction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal request");
                return StatusCode(500, new { message = "An error occurred while processing your withdrawal request." });
            }
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            try
            {
                var balance = await _cryptoService.GetAvailableBalance();
                return Ok(new { balance });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving balance");
                return StatusCode(500, new { message = "An error occurred while retrieving your balance." });
            }
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            try
            {
                var transactions = await _cryptoService.GetUserTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions");
                return StatusCode(500, new { message = "An error occurred while retrieving your transactions." });
            }
        }

        [HttpGet("transaction/{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            try
            {
                var transaction = await _cryptoService.GetTransaction(id);

                if (transaction == null)
                {
                    return NotFound(new { message = "Transaction not found." });
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction");
                return StatusCode(500, new { message = "An error occurred while retrieving the transaction." });
            }
        }

        [HttpGet("depositAddress")]
        public async Task<ActionResult> GenerateDepositAddress([FromQuery] string network, [FromQuery] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(network) || string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Network and token are required." });
                }

                // Validate network and token
                if (!new[] { "ethereum", "polygon", "tron" }.Contains(network.ToLower()))
                {
                    return BadRequest(new { message = "Invalid network. Supported networks are Ethereum, Polygon, and Tron." });
                }

                if (!new[] { "usdt", "usdc" }.Contains(token.ToLower()))
                {
                    return BadRequest(new { message = "Invalid token. Supported tokens are USDT and USDC." });
                }

                // Generate a deposit address using the appropriate service
                string address;

                try
                {
                    if (network.ToLower() == "tron")
                    {
                        var tronService = HttpContext.RequestServices.GetRequiredService<ITronService>();
                        address = await tronService.GenerateDepositAddress(token.ToUpper(), 0); // Use index 0 for testing
                    }
                    else
                    {
                        var ethereumService = HttpContext.RequestServices.GetRequiredService<IEthereumService>();
                        address = await ethereumService.GenerateDepositAddress(network + token.ToUpper(), 0); // Use index 0 for testing
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating deposit address for {Network} {Token}", network, token);
                    return StatusCode(500, new { message = "Failed to generate deposit address. Please try again later." });
                }

                if (string.IsNullOrEmpty(address))
                {
                    return StatusCode(500, new { message = "Failed to generate a valid deposit address." });
                }

                return Ok(new { address, network, token = token.ToUpper() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deposit address generation");
                return StatusCode(500, new { message = "An error occurred while generating a deposit address." });
            }
        }
    }
}