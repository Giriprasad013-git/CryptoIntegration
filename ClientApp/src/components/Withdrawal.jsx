import React, { useState, useEffect } from 'react';
import TokenSelector from './TokenSelector';
import { getBalance, createWithdrawal } from '../services/api';

const Withdrawal = () => {
  const [amount, setAmount] = useState(0);
  const [walletAddress, setWalletAddress] = useState('');
  const [selectedToken, setSelectedToken] = useState({
    token: 'USDT',
    network: 'PolygonPOS',
    icon: '🟢'
  });
  const [availableBalance, setAvailableBalance] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const data = await getBalance();
        setAvailableBalance(data.balance);
      } catch (err) {
        console.error('Failed to fetch balance:', err);
      }
    };

    fetchBalance();
  }, []);

  const handleAmountChange = (e) => {
    setAmount(parseFloat(e.target.value) || 0);
  };

  const handleWalletAddressChange = (e) => {
    setWalletAddress(e.target.value);
  };

  const handleTokenSelect = (token) => {
    setSelectedToken(token);
  };

  const handleSubmit = async () => {
    if (amount <= 0) {
      setError('Please enter a valid amount');
      return;
    }

    if (!walletAddress) {
      setError('Please enter a wallet address');
      return;
    }

    if (amount > availableBalance) {
      setError('Withdrawal amount exceeds available balance');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      await createWithdrawal({
        amount,
        token: selectedToken.token,
        network: selectedToken.network,
        walletAddress
      });
      
      setSuccess(true);
      setAmount(0);
      setWalletAddress('');
      
      // Refresh balance
      const data = await getBalance();
      setAvailableBalance(data.balance);
    } catch (err) {
      setError(err.message || 'Failed to process withdrawal request');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-sm overflow-hidden">
      <div className="p-4 border-b">
        <h2 className="text-gray-700 font-medium mb-2">Withdrawal Details</h2>
        <div className="mb-4">
          <div className="flex justify-between text-sm mb-1">
            <span>Withdrawal Amount</span>
            <span>Available: ${availableBalance.toFixed(2)}</span>
          </div>
          <div className="flex items-center">
            <span className="text-lg mr-2">$</span>
            <input
              type="number"
              value={amount || ''}
              onChange={handleAmountChange}
              className="w-full text-2xl outline-none"
              placeholder="0"
            />
          </div>
        </div>
      </div>

      <div className="p-4">
        <h2 className="text-gray-700 font-medium mb-3">Select Token</h2>
        <TokenSelector onSelect={handleTokenSelect} selectedToken={selectedToken} />
        
        <div className="mt-4">
          <h2 className="text-gray-700 font-medium mb-2">Wallet Address</h2>
          <input
            type="text"
            value={walletAddress}
            onChange={handleWalletAddressChange}
            className="w-full border border-gray-300 rounded-md p-3"
            placeholder="Enter wallet address"
          />
        </div>

        {error && (
          <div className="mt-4 p-2 bg-red-100 text-red-700 rounded-md">
            {error}
          </div>
        )}

        {success && (
          <div className="mt-4 p-2 bg-green-100 text-green-700 rounded-md">
            Withdrawal request submitted successfully!
          </div>
        )}

        <button
          onClick={handleSubmit}
          disabled={loading || amount <= 0 || !walletAddress}
          className={`w-full btn-natural8 mt-6 ${(loading || amount <= 0 || !walletAddress) ? 'opacity-50 cursor-not-allowed' : ''}`}
        >
          {loading ? 'Processing...' : 'Withdrawal'}
        </button>
      </div>
    </div>
  );
};

export default Withdrawal;
