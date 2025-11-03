# GiftOfGivers Web Application

A comprehensive disaster relief management system for reporting incidents, managing resource donations, and coordinating volunteers.

## 🚀 Features

- **Disaster Incident Reporting**: Report and track disaster incidents with severity levels
- **Resource Donation Management**: Track donations of medical supplies, food, clothing, and more
- **Volunteer Management**: Register volunteers, assign tasks, and track contributions
- **User Authentication**: Secure login and registration with ASP.NET Core Identity
- **Admin Dashboard**: Comprehensive management interface

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server Express (or full SQL Server)
- Visual Studio 2022 or Visual Studio Code

## 🛠️ Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd GiftOfGivers_WebApplication
   ```

2. **Configure database connection**
   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.\\SQLEXPRESS;Database=GiftOfGiversDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open browser to `https://localhost:5001`
   - Or `http://localhost:5000`

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Test Results
- **Total Tests**: 75
- **Passed**: 69 (92%)
- **Load Tests**: ✅ All passing
- **Stress Tests**: ✅ All passing

See [TESTING_SUMMARY.md](TESTING_SUMMARY.md) for detailed test results.

### Load & Stress Testing
```bash
# Python load testing
cd Tests/LoadTesting
pip install requests
python LoadTestScript.py

# Python stress testing
cd Tests/StressTesting
python StressTestScript.py
```

## 📚 Documentation

- **Complete Documentation**: [DOCUMENTATION.md](DOCUMENTATION.md)
- **Testing Summary**: [TESTING_SUMMARY.md](TESTING_SUMMARY.md)
- **API Documentation**: Available at `/swagger` when running

## 🏗️ Architecture

### Technology Stack
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5.3.2 with Bootstrap Icons
- **Testing**: xUnit, Moq, InMemory Database

### Database Schema
The application uses independent entities with no foreign key relationships:

- `Donors` - Donor information
- `IncidentReports` - Disaster incidents
- `ReliefProjects` - Relief projects
- `Donations` - Donation records
- `ResourceTracking` - Resource donations
- `Deliveries` - Delivery records
- `Volunteers` - Volunteer information
- `VolunteerTasks` - Volunteer tasks
- `VolunteerAssignments` - Task assignments

## 🔐 Security

- Password hashing with ASP.NET Core Identity
- Cookie authentication with sliding expiration
- CSRF protection on all forms
- SQL injection prevention via EF Core
- HTTPS enforced in production

## 📈 Performance

- **Concurrent Users**: 1000+
- **Response Time**: <400ms average
- **Success Rate**: >98% under load
- **95th Percentile**: <1000ms

## 🚢 Deployment

### Production Build
```bash
dotnet publish -c Release -o ./publish
```

### Environment Configuration
- **Development**: Uses local SQL Server Express
- **Production**: Configure connection string in appsettings.json

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests to ensure everything passes
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 👥 Authors

Development Team

## 🙏 Acknowledgments

- Bootstrap team for the excellent UI framework
- Microsoft for ASP.NET Core documentation
- xUnit for the testing framework

---

**Version**: 1.0  
**Last Updated**: November 2024  
**Status**: ✅ Production Ready

