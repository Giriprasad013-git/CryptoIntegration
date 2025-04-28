import React, { useState } from 'react';
import { AlertTriangle } from 'feather-icons-react';
import TokenSelector from './TokenSelector';
import DepositDetails from './DepositDetails';
import { createDeposit } from '../services/api';

const Deposit = () => {
  const [amount, setAmount] = useState(0);
  const [selectedToken, setSelectedToken] = useState({
    token: 'USDT',
    network: 'PolygonPOS',
    icon: '🟢'
  });
  const [depositDetails, setDepositDetails] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleAmountChange = (e) => {
    setAmount(parseFloat(e.target.value) || 0);
  };

  const selectPresetAmount = (value) => {
    setAmount(value);
  };

  const handleTokenSelect = (token) => {
    setSelectedToken(token);
  };

  const handleSubmit = async () => {
    if (amount <= 0) {
      setError('Please enter a valid amount');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const response = await createDeposit({
        amount,
        token: selectedToken.token,
        network: selectedToken.network
      });
      
      setDepositDetails(response);
    } catch (err) {
      setError(err.message || 'Failed to create deposit request');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-sm overflow-hidden">
      <div className="bg-alert p-4">
        <div className="flex items-start">
          <AlertTriangle className="text-natural8 mr-2 mt-1 flex-shrink-0" size={20} />
          <p className="text-sm text-natural8">
            Rest assured when you deposit with Natural8. Your financial privacy is important to us and there will be no mention of poker or gambling activities in your bank statement or e-wallet.
          </p>
        </div>
      </div>

      {!depositDetails ? (
        <>
          <div className="p-4 border-b">
            <h2 className="text-gray-700 font-medium mb-2">Deposit Amount</h2>
            <div className="flex items-center mb-4">
              <span className="text-lg mr-2">$</span>
              <input
                type="number"
                value={amount || ''}
                onChange={handleAmountChange}
                className="w-full text-2xl outline-none"
                placeholder="0"
              />
            </div>
            <div className="grid grid-cols-4 gap-2">
              <button onClick={() => selectPresetAmount(25)} className="amount-button">+ 25</button>
              <button onClick={() => selectPresetAmount(50)} className="amount-button">+ 50</button>
              <button onClick={() => selectPresetAmount(100)} className="amount-button">+ 100</button>
              <button onClick={() => selectPresetAmount(500)} className="amount-button">+ 500</button>
            </div>
          </div>

          <div className="p-4">
            <h2 className="text-gray-700 font-medium mb-2">Select Token</h2>
            <TokenSelector onSelect={handleTokenSelect} selectedToken={selectedToken} />

            {error && (
              <div className="mt-4 p-2 bg-red-100 text-red-700 rounded-md">
                {error}
              </div>
            )}

            <button
              onClick={handleSubmit}
              disabled={loading || amount <= 0}
              className={`w-full btn-natural8 mt-6 ${(loading || amount <= 0) ? 'opacity-50 cursor-not-allowed' : ''}`}
            >
              {loading ? 'Processing...' : 'Generate Deposit Address'}
            </button>
          </div>
        </>
      ) : (
        <DepositDetails details={depositDetails} onBack={() => setDepositDetails(null)} />
      )}
    </div>
  );
};

export default Deposit;
