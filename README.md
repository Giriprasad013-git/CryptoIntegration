# Crypto Deposit App

A comprehensive cryptocurrency deposit and withdrawal platform supporting USDT and USDC across Ethereum, Polygon, and Tron networks.

## Prerequisites

- A TronGrid API key for Tron network integration
- An Infura API key for Ethereum and Polygon network integration
- .NET 7 SDK or later
- Node.js (for running the React frontend)

## How to Run the Project Locally

### Step 1: Clone the Repository

```
git clone [repository-url]
cd crypto-deposit-app
```

### Step 2: Set up API Keys

Before running the application, you need to set up your API keys as environment variables:

#### On Windows:
```
setx INFURA_API_KEY "your_infura_api_key"
setx TRONGRID_API_KEY "your_trongrid_api_key"
```

#### On macOS/Linux:
```
export INFURA_API_KEY="your_infura_api_key"
export TRONGRID_API_KEY="your_trongrid_api_key"
```

Or create a `appsettings.Development.json` file in the project root with:
```json
{
  "INFURA_API_KEY": "your_infura_api_key",
  "TRONGRID_API_KEY": "your_trongrid_api_key"
}
```

### Step 3: Run the Backend API

```
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The backend API will be available at `http://localhost:5000`

### Step 4: Run the React Frontend (Optional)

```
# Navigate to the ClientApp directory
cd ClientApp

# Install dependencies
npm install

# Start the React development server
npm start
```

The frontend will be available at `http://localhost:3000`

### Step 5: Test API Endpoints

Once the application is running, you can access the Swagger UI at:
```
http://localhost:5000
```

This allows you to test the following API endpoints:

- `/api/tokens/list` - Get a list of supported tokens
- `/api/tokens/networks` - Get available blockchain networks
- `/api/crypto/generate-address` - Generate a deposit address
- `/api/crypto/verify-deposit` - Verify a deposit transaction
- `/api/crypto/balance` - Check wallet balance

## How to Run in Replit

### Step 1: Set up API Keys

Add the required API keys as environment secrets in the Replit secrets panel:

```
INFURA_API_KEY=your_infura_api_key
TRONGRID_API_KEY=your_trongrid_api_key
```

### Step 2: Run the Backend API

1. Click the "Run" button in Replit
2. This will start the CryptoDepositApp workflow which hosts the backend API 
3. Once started, you can access the Swagger UI at the URL provided in the webview

## Project Structure

- `Services/` - Contains blockchain integration services
  - `EthereumService.cs` - Handles Ethereum and EVM-compatible networks
  - `TronService.cs` - Handles Tron network
  - `TokenListingService.cs` - Manages token listings
  - `CryptoService.cs` - Coordinates between different blockchain services
  
- `Models/` - Data models
  - `TokenListing.cs` - Token information
  - `DepositRequest.cs` - Deposit request format
  - `WithdrawalRequest.cs` - Withdrawal request format
  - `Transaction.cs` - Transaction details
  
- `Controllers/` - API controllers
  - `TokenController.cs` - Endpoints for token management
  - `CryptoController.cs` - Endpoints for crypto operations

## Technologies Used

- .NET 7 backend
- Nethereum for Ethereum integration
- TronNet for Tron network integration
- React.js frontend
- Swagger for API documentation and testing# CryptoIntegration
