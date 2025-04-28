/**
 * Blockchain Utility Service
 * This service provides functions for interacting with blockchain-specific features
 */

import { getTokenListings, getAvailableNetworks, generateDepositAddress, verifyDeposit } from './api';

/**
 * Get all supported blockchain networks
 * @returns {Promise<Array>} List of available networks
 */
export const getSupportedNetworks = async () => {
  try {
    return await getAvailableNetworks();
  } catch (error) {
    console.error('Error fetching supported networks:', error);
    throw error;
  }
};

/**
 * Get all supported tokens across all networks
 * @returns {Promise<Array>} List of token listings
 */
export const getSupportedTokens = async () => {
  try {
    return await getTokenListings();
  } catch (error) {
    console.error('Error fetching supported tokens:', error);
    throw error;
  }
};

/**
 * Generate a deposit address for the specified network and token
 * @param {string} network - Blockchain network (Ethereum, Polygon, Tron)
 * @param {string} tokenSymbol - Token symbol (USDT, USDC)
 * @returns {Promise<Object>} Generated address information
 */
export const getDepositAddress = async (network, tokenSymbol) => {
  try {
    return await generateDepositAddress({
      network,
      tokenSymbol
    });
  } catch (error) {
    console.error('Error generating deposit address:', error);
    throw error;
  }
};

/**
 * Verify a deposit transaction on the blockchain
 * @param {string} txHash - Transaction hash/ID
 * @param {string} network - Blockchain network (Ethereum, Polygon, Tron)
 * @returns {Promise<Object>} Verification result
 */
export const verifyTransaction = async (txHash, network) => {
  try {
    return await verifyDeposit({
      txHash,
      network
    });
  } catch (error) {
    console.error('Error verifying transaction:', error);
    throw error;
  }
};

/**
 * Format blockchain address for display (shortens the address)
 * @param {string} address - Full blockchain address
 * @returns {string} Shortened address (e.g., 0x1234...5678)
 */
export const formatAddress = (address) => {
  if (!address) return '';
  if (address.length < 10) return address;
  
  return `${address.substring(0, 6)}...${address.substring(address.length - 4)}`;
};

/**
 * Get blockchain explorer URL for a transaction
 * @param {string} txHash - Transaction hash/ID
 * @param {string} network - Blockchain network (Ethereum, Polygon, Tron)
 * @returns {string} Explorer URL
 */
export const getExplorerUrl = (txHash, network) => {
  if (!txHash) return '';
  
  switch (network.toLowerCase()) {
    case 'ethereum':
      return `https://etherscan.io/tx/${txHash}`;
    case 'polygon':
      return `https://polygonscan.com/tx/${txHash}`;
    case 'tron':
      return `https://tronscan.org/#/transaction/${txHash}`;
    default:
      return '';
  }
};

/**
 * Get blockchain explorer URL for an address
 * @param {string} address - Blockchain address
 * @param {string} network - Blockchain network (Ethereum, Polygon, Tron)
 * @returns {string} Explorer URL
 */
export const getAddressExplorerUrl = (address, network) => {
  if (!address) return '';
  
  switch (network.toLowerCase()) {
    case 'ethereum':
      return `https://etherscan.io/address/${address}`;
    case 'polygon':
      return `https://polygonscan.com/address/${address}`;
    case 'tron':
      return `https://tronscan.org/#/address/${address}`;
    default:
      return '';
  }
};

/**
 * Get the token icon URL based on token symbol and network
 * @param {string} symbol - Token symbol
 * @param {string} network - Blockchain network
 * @returns {string} URL to token icon
 */
export const getTokenIconUrl = (symbol, network) => {
  const symbolLower = symbol.toLowerCase();
  const networkLower = network.toLowerCase();
  
  // For USDT and USDC, we can use standard icons
  if (symbolLower === 'usdt') {
    return 'https://cryptologos.cc/logos/tether-usdt-logo.png';
  } else if (symbolLower === 'usdc') {
    return 'https://cryptologos.cc/logos/usd-coin-usdc-logo.png';
  }
  
  // Default icon if no match
  return 'https://cryptologos.cc/logos/icon-placeholder.png';
};

export default {
  getSupportedNetworks,
  getSupportedTokens,
  getDepositAddress,
  verifyTransaction,
  formatAddress,
  getExplorerUrl,
  getAddressExplorerUrl,
  getTokenIconUrl
};