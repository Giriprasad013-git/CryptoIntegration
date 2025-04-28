const axios = require('axios');

// Simple API client for testing
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Handle API errors
const handleApiError = (error) => {
  const errorMessage = error.response?.data?.message || 'An unexpected error occurred';
  console.error('API Error:', error.message);
  return { error: errorMessage };
};

// Get available networks
const getAvailableNetworks = async () => {
  try {
    const response = await api.get('/token/networks');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get token listings
const getTokenListings = async () => {
  try {
    const response = await api.get('/token/listings');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Run tests
async function runTests() {
  console.log('Testing API integrations with real blockchain services...');
  
  console.log('\n1. Fetching available networks:');
  const networks = await getAvailableNetworks();
  console.log(JSON.stringify(networks, null, 2));
  
  console.log('\n2. Fetching token listings:');
  const tokens = await getTokenListings();
  console.log(JSON.stringify(tokens, null, 2));
  
  console.log('\nTests completed!');
}

// Run the tests
runTests();