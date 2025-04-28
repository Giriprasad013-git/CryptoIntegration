using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoDepositApp.Models;
using CryptoDepositApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptoDepositApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenListingService _tokenListingService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenListingService tokenListingService, ILogger<TokenController> logger)
        {
            _tokenListingService = tokenListingService;
            _logger = logger;
        }

        [HttpGet("listings")]
        public async Task<ActionResult<IEnumerable<TokenListing>>> GetAllTokenListings()
        {
            var tokens = await _tokenListingService.GetTokenListings();
            return Ok(tokens);
        }

        [HttpGet("listings/network/{network}")]
        public async Task<ActionResult<IEnumerable<TokenListing>>> GetTokenListingsByNetwork(string network)
        {
            var tokens = await _tokenListingService.GetTokenListingsByNetwork(network);
            return Ok(tokens);
        }

        [HttpGet("listings/symbol/{symbol}")]
        public async Task<ActionResult<IEnumerable<TokenListing>>> GetTokenListingsBySymbol(string symbol)
        {
            var tokens = await _tokenListingService.GetTokenListingsBySymbol(symbol);
            return Ok(tokens);
        }

        [HttpGet("listing/{symbol}/{network}")]
        public async Task<ActionResult<TokenListing>> GetTokenListing(string symbol, string network)
        {
            var token = await _tokenListingService.GetTokenListing(symbol, network);
            
            if (token == null)
            {
                return NotFound($"Token {symbol} on {network} not found");
            }
            
            return Ok(token);
        }

        [HttpGet("networks")]
        public async Task<ActionResult<IEnumerable<TokenNetwork>>> GetAvailableNetworks()
        {
            var networks = await _tokenListingService.GetAvailableNetworks();
            return Ok(networks);
        }
    }
}