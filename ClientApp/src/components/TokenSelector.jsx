import React, { useState, useRef, useEffect } from 'react';
import { ChevronDown } from 'feather-icons-react';

const tokenOptions = [
  { token: 'USDT', network: 'PolygonPOS', icon: '🟢' },
  { token: 'USDT', network: 'Tron', icon: '🟢' },
  { token: 'USDT', network: 'Ethereum', icon: '🟢' },
  { token: 'USDT', network: 'BEP20', icon: '🟢' },
  { token: 'USDC', network: 'PolygonPOS', icon: '🔵' },
  { token: 'USDC', network: 'BEP20', icon: '🔵' }
];

const TokenSelector = ({ onSelect, selectedToken }) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef(null);

  useEffect(() => {
    function handleClickOutside(event) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setIsOpen(false);
      }
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleSelect = (option) => {
    onSelect(option);
    setIsOpen(false);
  };

  return (
    <div className="relative" ref={dropdownRef}>
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="w-full flex items-center justify-between border border-gray-300 rounded-md p-3"
      >
        <div className="flex items-center">
          <span className="mr-2">{selectedToken.icon}</span>
          <span>{selectedToken.token} ({selectedToken.network})</span>
        </div>
        <ChevronDown size={20} />
      </button>

      {isOpen && (
        <div className="absolute z-10 mt-1 w-full bg-white border border-gray-200 rounded-md shadow-lg">
          {tokenOptions.map((option, index) => (
            <div
              key={index}
              className="dropdown-item"
              onClick={() => handleSelect(option)}
            >
              <span className="mr-2">{option.icon}</span>
              <span>{option.token} ({option.network})</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default TokenSelector;
