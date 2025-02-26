
# Mock Booking System API  

## Overview  
This project is a **Mock Booking System API** built using **ASP.NET Core Web API**. It simulates a booking system for the travel industry, allowing users to:  

- **Search** for holiday options (hotels and flights).  
- **Book** a selected option.  
- **Check** the status of their booking.  

---

## Features  

### 🔍 Search Endpoint (`/api/Search`)  
Allows users to search for:  
- **Hotels only**  
- **Hotels and flights**  
- **Last-minute hotels** (if the `FromDate` is within 45 days of the current date)  

Utilizes external APIs:  
- `SearchHotels API`  
- `SearchFlights API`  

Access API Endpoint host:

http://localhost:5000/{endpoint}

### 📌 Book Endpoint (`/api/Book`)  
- Allows users to book a selected option from the search results.  
- Generates a random **BookingCode** and stores booking details in memory.  

### 🔄 Check Status Endpoint (`/api/CheckStatus`)  
- Allows users to check the status of their booking.  
- **Booking status transitions:**  
  - **Pending** → After a random delay (30–60 seconds), it changes to:  
    - ✅ **Success** (for `HotelOnly` and `HotelAndFlight` searches).  
    - ❌ **Failed** (for `LastMinuteHotels` searches).  


## 🐳 Deployment Environment  
This application is deployed within a **Docker container**. The Docker setup ensures a **consistent and isolated environment** for running the application.  

---
## Project structure
The project follows a clean architecture with well-organized folders: 


## 📂 Project Structure  

```plaintext
MockBookingSystem/
│
├── Controllers/
│   ├── SearchController.cs        # Handles search requests
│   ├── BookController.cs          # Handles booking requests
│   └── CheckStatusController.cs   # Handles status checks
│
├── Interfaces/
│   └── IManager.cs                # Defines the manager interface for handling business logic
│
├── Managers/
│   ├── BaseManager.cs             # Abstract base class for managers
│   ├── HotelOnlyManager.cs        # Handles "Hotel Only" searches
│   ├── HotelAndFlightManager.cs   # Handles "Hotel + Flight" searches
│   ├── LastMinuteHotelsManager.cs # Handles last-minute hotel searches
│   └── ManagerFactory.cs          # Factory for creating appropriate managers based on request type
│
├── Middleware/
│   └── GlobalExceptionHandlingMiddleware.cs  # Handles global exceptions
│
├── Models/
│   ├── SearchReq.cs               # Request model for search endpoint
│   ├── SearchRes.cs               # Response model for search endpoint
│   ├── BookReq.cs                 # Request model for book endpoint
│   ├── BookRes.cs                 # Response model for book endpoint
│   ├── CheckStatusReq.cs          # Request model for check status endpoint
│   ├── CheckStatusRes.cs          # Response model for check status endpoint
│   ├── Hotel.cs                   # Model representing hotel data from external API
│   └── Flight.cs                  # Model representing flight data from external API
│
├── Program.cs                     # Configures services, middleware, and application pipeline
└── README.md                      # Documentation (this file)
```

## How It Works

### Search Flow (`/api/Search`)

- The user sends a POST request with parameters like destination, departureAirport, fromDate, and toDate.
- The system determines the type of search:
  - HotelOnly: If departureAirport is not provided in the request body.
  - HotelAndFlight: If departureAirport is provided.
  - LastMinuteHotels: If fromDate is within 45 days of today and departureAirport is not provided in the request body.
- The appropriate manager (HotelOnlyManager, HotelAndFlightManager, or LastMinuteHotelsManager) handles the request.
- External APIs are called to fetch data:
  - Hotels: SearchHotels API
  - Flights: SearchFlights API
- Results are returned as an array of options.

### Book Flow (`/api/Book`)

- The user sends a POST request with an OptionCode (from the search results) and the original search request.
- A random BookingCode is generated, along with a random delay (SleepTime) between 30–60 seconds.
- Response result is the generated BookingCode which can be used in the checkstatus call.

### Check Status Flow (`/api/CheckStatus`)

- The user sends a POST request with their unique BookingCode.
- The system checks if enough time has passed since the booking:
  - If not, the status is Pending and result 2 is returned as a response.
  - If yes:
    - For HotelOnly and HotelAndFlight bookings: Status becomes Success.
    - For LastMinuteHotels bookings: Status becomes Failed.

## Testing Endpoints


### 📌 API Endpoints Example Requests  

### 🔍 Search Endpoint (`/api/Search`)  

```json
{
  "destination": "SKP",
  "departureAirport": "CPH",
  "fromDate": "2025-02-25",
  "toDate": "2025-02-26"
}
```

### 📌 Book Endpoint (/api/Book)

```json
{
  "optionCode": "exampleOptionCode",
  "searchReq": {
    "destination": "SKP",
    "departureAirport": "BCN",
    "fromDate": "2025-02-25",
    "toDate": "2025-02-27"
  }
}
```

### 🔄 Check Status Endpoint (/api/CheckStatus)

``` json
{
  "bookingCode": "exampleBookingCode"
}
```
