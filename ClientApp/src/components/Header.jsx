import React from 'react';
import { ArrowLeft } from 'feather-icons-react';

const Header = ({ activeTab, setActiveTab }) => {
  return (
    <header className="bg-natural8 text-white p-4">
      <div className="flex items-center justify-between">
        <div className="flex items-center">
          <button className="mr-2">
            <ArrowLeft size={20} />
          </button>
          <h1 className="text-xl font-medium">
            {activeTab === 'deposit' ? 'Deposit' : 'Withdrawal'}
          </h1>
        </div>
        <div className="flex items-center">
          <button className="mr-2">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <circle cx="12" cy="12" r="10"></circle>
              <line x1="2" y1="12" x2="22" y2="12"></line>
              <path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"></path>
            </svg>
          </button>
          <button>
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
              <path d="M13.73 21a2 2 0 0 1-3.46 0"></path>
            </svg>
          </button>
        </div>
      </div>

      <div className="flex mt-4">
        <button 
          className={`py-2 flex-1 text-center ${activeTab === 'deposit' ? 'border-b-2 border-white font-semibold' : 'text-gray-300'}`}
          onClick={() => setActiveTab('deposit')}
        >
          Deposit
        </button>
        <button 
          className={`py-2 flex-1 text-center ${activeTab === 'withdrawal' ? 'border-b-2 border-white font-semibold' : 'text-gray-300'}`}
          onClick={() => setActiveTab('withdrawal')}
        >
          Withdrawal
        </button>
      </div>
    </header>
  );
};

export default Header;
