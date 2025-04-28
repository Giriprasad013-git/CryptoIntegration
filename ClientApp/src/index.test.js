// Import the API services to test
import api from './services/api';
import blockchain from './services/blockchain';
import wallet from './services/wallet';

// Function to test our API integrations
async function testAPI() {
  try {
    console.log('Testing API connections...');
    
    // Try to get available networks
    console.log('Fetching available networks...');
    const networks = await blockchain.getSupportedNetworks();
    console.log('Available networks:', networks);
    
    // Try to get token listings
    console.log('Fetching token listings...');
    const tokens = await blockchain.getSupportedTokens();
    console.log('Available tokens:', tokens);
    
    // Try to get wallet balance
    console.log('Fetching wallet balance...');
    const balance = await wallet.fetchWalletBalance();
    console.log('Wallet balance:', balance);
    
    console.log('API tests completed successfully!');
  } catch (error) {
    console.error('API Test Error:', error.message);
  }
}

// Run the test if this script is executed directly
if (require.main === module) {
  testAPI();
}

export default testAPI;