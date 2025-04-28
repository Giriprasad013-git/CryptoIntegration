/**
 * Wallet Service
 * This service handles wallet-related functionality for the application
 */

import { getBalance, createDeposit, createWithdrawal, getTransactions, getTransaction } from './api';
import { getDepositAddress, verifyTransaction } from './blockchain';

/**
 * Get the user's wallet balance
 * @returns {Promise<Object>} Balance information
 */
export const fetchWalletBalance = async () => {
  try {
    return await getBalance();
  } catch (error) {
    console.error('Error fetching wallet balance:', error);
    throw error;
  }
};

/**
 * Initiate a deposit process
 * @param {Object} depositData - Deposit information
 * @param {string} depositData.network - Blockchain network
 * @param {string} depositData.tokenSymbol - Token symbol to deposit
 * @param {number} depositData.amount - Amount to deposit
 * @returns {Promise<Object>} Deposit transaction details
 */
export const initiateDeposit = async (depositData) => {
  try {
    // First, generate a deposit address for the specified network and token
    const addressInfo = await getDepositAddress(depositData.network, depositData.tokenSymbol);
    
    // Then create a deposit request in the system
    const deposit = await createDeposit({
      ...depositData,
      depositAddress: addressInfo.address
    });
    
    return {
      ...deposit,
      addressInfo
    };
  } catch (error) {
    console.error('Error initiating deposit:', error);
    throw error;
  }
};

/**
 * Check the status of a deposit transaction
 * @param {string} txHash - Transaction hash/ID
 * @param {string} network - Blockchain network
 * @returns {Promise<Object>} Verification result
 */
export const checkDepositStatus = async (txHash, network) => {
  try {
    return await verifyTransaction(txHash, network);
  } catch (error) {
    console.error('Error checking deposit status:', error);
    throw error;
  }
};

/**
 * Initiate a withdrawal
 * @param {Object} withdrawalData - Withdrawal information
 * @param {string} withdrawalData.network - Blockchain network
 * @param {string} withdrawalData.tokenSymbol - Token symbol to withdraw
 * @param {number} withdrawalData.amount - Amount to withdraw
 * @param {string} withdrawalData.destinationAddress - Recipient address
 * @returns {Promise<Object>} Withdrawal transaction details
 */
export const initiateWithdrawal = async (withdrawalData) => {
  try {
    return await createWithdrawal(withdrawalData);
  } catch (error) {
    console.error('Error initiating withdrawal:', error);
    throw error;
  }
};

/**
 * Get the user's transaction history
 * @returns {Promise<Array>} List of transactions
 */
export const getTransactionHistory = async () => {
  try {
    return await getTransactions();
  } catch (error) {
    console.error('Error fetching transaction history:', error);
    throw error;
  }
};

/**
 * Get details of a specific transaction
 * @param {string} transactionId - Transaction ID
 * @returns {Promise<Object>} Transaction details
 */
export const getTransactionDetails = async (transactionId) => {
  try {
    return await getTransaction(transactionId);
  } catch (error) {
    console.error('Error fetching transaction details:', error);
    throw error;
  }
};

/**
 * Format a currency amount with proper symbol and decimals
 * @param {number} amount - Amount to format
 * @param {string} symbol - Currency symbol
 * @returns {string} Formatted amount
 */
export const formatCurrencyAmount = (amount, symbol = 'USDT') => {
  if (amount === undefined || amount === null) return `0 ${symbol}`;
  
  // Format with 2 decimal places for display
  const formatted = parseFloat(amount).toFixed(2);
  return `${formatted} ${symbol}`;
};

export default {
  fetchWalletBalance,
  initiateDeposit,
  checkDepositStatus,
  initiateWithdrawal,
  getTransactionHistory,
  getTransactionDetails,
  formatCurrencyAmount
};