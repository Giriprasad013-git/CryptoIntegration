import React, { useState } from 'react';
// import Header from './components/Header';
// import Deposit from './components/Deposit';
// import Withdrawal from './components/Withdrawal';

function Header({ activeTab, setActiveTab }) {
  return (
    <header className="header">
      <div className="tabs">
        <div 
          className={`tab ${activeTab === 'deposit' ? 'active' : ''}`}
          onClick={() => setActiveTab('deposit')}
        >
          Deposit
        </div>
        <div 
          className={`tab ${activeTab === 'withdrawal' ? 'active' : ''}`}
          onClick={() => setActiveTab('withdrawal')}
        >
          Withdrawal
        </div>
      </div>
    </header>
  );
}

function Deposit() {
  return (
    <div>
      <h2>Deposit Crypto</h2>
      <div className="form-group">
        <label className="label">Network</label>
        <select className="input">
          <option>Ethereum</option>
          <option>Polygon</option>
          <option>Tron</option>
        </select>
      </div>
      <div className="form-group">
        <label className="label">Token</label>
        <select className="input">
          <option>USDT</option>
          <option>USDC</option>
        </select>
      </div>
      <button className="button">Continue</button>
    </div>
  );
}

function Withdrawal() {
  return (
    <div>
      <h2>Withdraw Crypto</h2>
      <div className="form-group">
        <label className="label">Network</label>
        <select className="input">
          <option>Ethereum</option>
          <option>Polygon</option>
          <option>Tron</option>
        </select>
      </div>
      <div className="form-group">
        <label className="label">Token</label>
        <select className="input">
          <option>USDT</option>
          <option>USDC</option>
        </select>
      </div>
      <div className="form-group">
        <label className="label">Destination Address</label>
        <input className="input" type="text" placeholder="Enter wallet address" />
      </div>
      <div className="form-group">
        <label className="label">Amount</label>
        <input className="input" type="text" placeholder="0.00" />
      </div>
      <button className="button">Withdraw</button>
    </div>
  );
}

function App() {
  const [activeTab, setActiveTab] = useState('deposit');

  return (
    <div className="min-h-screen flex flex-col">
      <Header activeTab={activeTab} setActiveTab={setActiveTab} />
      <main className="flex-grow p-4 max-w-md mx-auto w-full">
        {activeTab === 'deposit' ? <Deposit /> : <Withdrawal />}
      </main>
    </div>
  );
}

export default App;
