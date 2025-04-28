import React from 'react';
import { ArrowLeft, AlertTriangle, Copy } from 'feather-icons-react';

const DepositDetails = ({ details, onBack }) => {
  const handleCopy = (text) => {
    navigator.clipboard.writeText(text);
    // You could add a toast notification here
  };
  
  const getNetworkName = (network) => {
    switch(network) {
      case 'PolygonPOS':
        return 'POLYGON';
      default:
        return network.toUpperCase();
    }
  };

  return (
    <div>
      <div className="bg-alert p-4">
        <div className="flex items-start">
          <AlertTriangle className="text-natural8 mr-2 mt-1 flex-shrink-0" size={20} />
          <p className="text-sm text-natural8">
            Rest assured when you deposit with Natural8. Your financial privacy is important to us and there will be no mention of poker or gambling activities in your bank statement or e-wallet.
          </p>
        </div>
      </div>

      <div className="p-4">
        <div className="flex items-center mb-4">
          <span className="mr-2">{details.token === 'USDT' ? '🟢' : '🔵'}</span>
          <h2 className="text-xl font-medium">Pay with {details.token}</h2>
        </div>

        <div className="space-y-4">
          <div className="flex justify-between items-center">
            <span className="text-gray-600">Network</span>
            <span className="font-medium">Only for {details.token} ({getNetworkName(details.network)})</span>
          </div>

          <div className="flex justify-between items-center">
            <span className="text-gray-600">Amount</span>
            <div className="flex items-center">
              <span className="font-medium">{details.token} {details.network} {details.amount.toFixed(6)}</span>
              <button onClick={() => handleCopy(details.amount.toString())} className="ml-2">
                <Copy size={16} />
              </button>
            </div>
          </div>

          <div className="flex justify-between items-center">
            <span className="text-gray-600">Exchange Rate</span>
            <span className="font-medium">1 {details.token} {details.network} = USD {details.exchangeRate}</span>
          </div>

          <div className="flex justify-between items-center">
            <span className="text-gray-600">Address</span>
            <div className="flex items-center overflow-hidden">
              <span className="font-medium truncate max-w-[150px]">{details.walletAddress}</span>
              <button onClick={() => handleCopy(details.walletAddress)} className="ml-2 flex-shrink-0">
                <Copy size={16} />
              </button>
            </div>
          </div>

          <div className="flex justify-between items-center">
            <span className="text-gray-600">Status</span>
            <span className="font-medium text-yellow-600">Awaiting Payment</span>
          </div>

          <div className="flex justify-between items-center">
            <span className="text-gray-600">TXID</span>
            <span className="font-medium text-gray-500">{details.txid}</span>
          </div>
        </div>

        <div className="mt-6 p-4 bg-amber-50 rounded-md border border-amber-100">
          <div className="flex items-start">
            <AlertTriangle className="text-amber-500 mr-2 mt-1 flex-shrink-0" size={20} />
            <div>
              <p className="text-sm text-gray-700 mb-2">
                <span className="font-medium">Note</span>
              </p>
              <ul className="text-sm text-gray-700 list-disc pl-4 space-y-1">
                <li>A new cashier request is required for each deposit</li>
                <li>Please ensure you use the correct address, coin/token, and network as specified in the above section.</li>
                <li>We are not responsible for transactions that cannot be recovered.</li>
              </ul>
            </div>
          </div>
        </div>

        <button
          onClick={onBack}
          className="w-full btn-natural8 mt-6 flex items-center justify-center"
        >
          <ArrowLeft size={16} className="mr-2" />
          Back to Deposit
        </button>
      </div>
    </div>
  );
};

export default DepositDetails;
