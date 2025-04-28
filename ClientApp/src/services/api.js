import axios from 'axios';

// Get the API base URL based on window location to handle different environments
const getApiBaseUrl = () => {
  // In development, use the proxy config
  if (process.env.NODE_ENV === 'development') {
    return '/api';
  }

  // In production, use the actual URL from the window location
  const url = new URL(window.location.href);
  return `${url.protocol}//${url.host}/api`;
};

const API_BASE_URL = getApiBaseUrl();

// Configure axios with default settings
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Handle API errors
const handleApiError = (error) => {
  const errorMessage = error.response?.data?.message || 'An unexpected error occurred';
  console.error('API Error:', error);
  throw new Error(errorMessage);
};

// ------------- TOKEN APIs ------------- //

// Get all token listings
export const getTokenListings = async () => {
  try {
    const response = await api.get('/token/listings');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get token listings by network
export const getTokenListingsByNetwork = async (network) => {
  try {
    const response = await api.get(`/token/listings/network/${network}`);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get token listings by symbol
export const getTokenListingsBySymbol = async (symbol) => {
  try {
    const response = await api.get(`/token/listings/symbol/${symbol}`);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get specific token listing by symbol and network
export const getTokenListing = async (symbol, network) => {
  try {
    const response = await api.get(`/token/listing/${symbol}/${network}`);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get available networks
export const getAvailableNetworks = async () => {
  try {
    const response = await api.get('/token/networks');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// ------------- CRYPTO APIs ------------- //

// Get available balance
export const getBalance = async () => {
  try {
    const response = await api.get('/crypto/balance');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Create a deposit request
export const createDeposit = async (depositData) => {
  try {
    const response = await api.post('/crypto/deposit', depositData);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Create a withdrawal request
export const createWithdrawal = async (withdrawalData) => {
  try {
    const response = await api.post('/crypto/withdrawal', withdrawalData);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get all transactions
export const getTransactions = async () => {
  try {
    const response = await api.get('/crypto/transactions');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Get a specific transaction
export const getTransaction = async (id) => {
  try {
    const response = await api.get(`/crypto/transaction/${id}`);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Generate a deposit address
export const generateDepositAddress = async (data) => {
  try {
    const response = await api.post('/crypto/generate-address', data);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

// Verify a deposit transaction
export const verifyDeposit = async (verificationData) => {
  try {
    const response = await api.post('/crypto/verify-deposit', verificationData);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

export default api;
