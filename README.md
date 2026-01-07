# 🛡️ Pulpa.TrainerSafety

A Progressive Web Application (PWA) built with Blazor WebAssembly that provides anti-phishing training campaigns for non-technical users through simulated email/SMS attacks.

[![License:  MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/. NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)

🌐 **Live Demo**: [https://pulpatrainersafety20251201193553-fda0h5ckfvdwf8dg.centralus-01.azurewebsites.net/](https://pulpatrainersafety20251201193553-fda0h5ckfvdwf8dg. centralus-01.azurewebsites.net/)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Subscription Model](#subscription-model)
- [Security](#security)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

**Pulpa.TrainerSafety** empowers individuals and families to develop security awareness through safe, realistic phishing simulations. The application provides a controlled environment where users can learn to recognize and respond to phishing attempts without real-world risks.

### Vision & Mission

**Vision**: Create a safer digital world by empowering individuals with practical security awareness training.

**Mission**: Provide accessible, engaging, and effective phishing awareness training through realistic simulations and personalized feedback.

### Target Audience

- **Primary**: Non-technical individuals aged 25-65
- **Secondary**: Families seeking collective security education
- **Tertiary**: Small businesses looking for employee training

## ✨ Features

### 🎪 Campaign Management
- **Template Library**: Pre-built phishing simulations with varying difficulty levels
- **Custom Campaigns**: Create your own scenarios (Pro tier)
- **Multi-channel**:  Email and SMS delivery options
- **Scheduling**: Advanced timing options (Pro tier)
- **Target Management**: Individual and family group targeting

### 📊 Tracking & Analytics
- **Real-time Monitoring**: Live campaign progress tracking
- **Comprehensive Metrics**: Open rates, click rates, submission rates
- **Progress Analysis**: User improvement tracking over time
- **Family Analytics**: Group performance comparison
- **Detailed Reports**: Exportable analytics in multiple formats

### 📚 Educational Content
- **Instant Feedback**: Post-campaign educational explanations
- **Security Tips**: Contextual advice based on user actions
- **Learning Center**: Comprehensive security awareness resources
- **Personalized Recommendations**: Targeted training suggestions
- **Knowledge Assessment**: Quizzes and skill evaluations

### 👨‍👩‍👧‍👦 Family Protection
- **Group Training**: Coordinate security education for entire household
- **Family Analytics**: Track collective progress
- **Member Management**: Easy invite and management system
- **Shared Templates**: Collaborative campaign creation

### 📱 PWA Features
- **Offline Capabilities**: Cache essential application shell
- **Mobile Optimization**: Touch-friendly, responsive design
- **Install Prompt**: Native app-like experience
- **Push Notifications**: Campaign results and updates

## 🛠️ Technology Stack

### Frontend
- **Framework**: Blazor WebAssembly (. NET 10.0)
- **UI Components**: Bootstrap 5
- **State Management**: Blazored. LocalStorage
- **Authentication**: ASP.NET Core Identity with JWT

### Backend
- **API**:  ASP.NET Core Web API (. NET 10.0)
- **ORM**: Entity Framework Core 10.0
- **Database**: SQL Server
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Scalar (OpenAPI)

### DevOps & Hosting
- **.NET Aspire**: Application orchestration
- **Azure App Service**: Cloud hosting
- **CI/CD**: Azure DevOps / GitHub Actions ready

## 🏗️ Architecture

The application follows a clean architecture pattern with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│     Blazor WebAssembly (PWA)           │
│     - UI Components                     │
│     - Client-side State Management     │
└─────────────────┬───────────────────────┘
                  │ HTTPS/JWT
┌─────────────────▼───────────────────────┐
│     ASP.NET Core Web API               │
│     - Authentication                    │
│     - Business Logic                    │
│     - Campaign Management              │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│     Entity Framework Core               │
│     - Data Access Layer                 │
│     - Migrations                        │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│     SQL Server Database                 │
│     - User Data                         │
│     - Campaigns & Results               │
└─────────────────────────────────────────┘
```

### Role-Based Access Control

The application implements a comprehensive permission system with three user roles:

- **Admin**:  Full system access
- **Member**: Family group members with limited permissions
- **User**: Standard individual users

Permissions are mapped to subscription types and include:
- Campaign management (`create_campaign`, `edit_campaign`, `delete_campaign`)
- Template operations (`view_templates`, `create_custom_template`)
- Analytics access (`view_results`, `advanced_analytics`, `export_reports`)
- Family management (`manage_family`, `invite_family_members`)
- Subscription control (`manage_subscription`, `view_billing`)

## 🚀 Getting Started

### Prerequisites

- [. NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB for development)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/rodrigoloya/Pulpa.TrainerSafety.git
   cd Pulpa.TrainerSafety
   ```

2. **Configure the database connection**
   
   Update the connection string in `Pulpa.TrainerSafety.AppHost/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PulpaTrainerSafety;Trusted_Connection=True;"
     }
   }
   ```

3. **Set up User Secrets for JWT**
   ```bash
   cd Pulpa.TrainerSafety. Api
   dotnet user-secrets set "Jwt:SecretKey" "your-super-secret-key-min-32-characters-long"
   dotnet user-secrets set "Jwt: Issuer" "PulpaTrainerSafety"
   dotnet user-secrets set "Jwt:Audience" "PulpaTrainerSafetyUsers"
   dotnet user-secrets set "Jwt:ExpirationInMinutes" "60"
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project Pulpa.TrainerSafety.Data --startup-project Pulpa.TrainerSafety.Api
   ```

5. **Run the application**
   
   Using . NET Aspire AppHost:
   ```bash
   cd Pulpa.TrainerSafety.AppHost
   dotnet run
   ```

   Or run API and frontend separately:
   ```bash
   # Terminal 1 - API
   cd Pulpa.TrainerSafety.Api
   dotnet run

   # Terminal 2 - Frontend
   cd Pulpa.TrainerSafety
   dotnet run
   ```

6. **Access the application**
   - Frontend: `https://localhost:5001`
   - API: `https://localhost:7184`
   - API Documentation: `https://localhost:7184/scalar/v1`

## 📁 Project Structure

```
Pulpa.TrainerSafety/
├── Pulpa.TrainerSafety/              # Blazor WebAssembly Frontend
│   ├── Pages/                        # Razor pages
│   ├── Layout/                       # Layout components
│   ├── wwwroot/                      # Static assets
│   └── Program.cs                    # Client entry point
├── Pulpa. TrainerSafety.Api/          # ASP.NET Core Web API
│   ├── Feature/                      # Feature-based endpoints
│   ├── Identity/                     # Authentication/Authorization
│   ├── AppStart/                     # Startup configuration
│   └── Program.cs                    # API entry point
├── Pulpa. TrainerSafety.Data/         # Data Access Layer
│   ├── Data/                         # DbContext
│   ├── Entities/                     # Entity models
│   └── Migrations/                   # EF Core migrations
├── Pulpa.TrainerSafety. Domain/       # Domain Models & Business Logic
│   ├── Permissions. cs                # Permission constants
│   ├── Roles.cs                      # Role definitions
│   └── CustomClaimTypes.cs           # Custom claim types
├── Pulpa.TrainerSafety.AppHost/      # . NET Aspire AppHost
└── Pulpa.TrainerSafety.ServiceDefaults/ # Shared service configuration
```

## 💰 Subscription Model

### Free Tier ($0)
- ✅ 1 active campaign at a time
- ✅ Personal use only
- ✅ Basic templates
- ✅ Limited analytics (7 days)
- ❌ No family groups

### Pro Tier ($10/month)
- ✅ Unlimited campaigns
- ✅ Up to 5 family members
- ✅ Custom templates
- ✅ Advanced analytics (90 days)
- ✅ Campaign scheduling
- ✅ Priority support
- ✅ Export reports

## 🔒 Security

- **Authentication**: JWT Bearer tokens with secure secret key management
- **Authorization**: Role and permission-based access control
- **Data Protection**: Encrypted data storage and transmission
- **Password Security**: ASP.NET Core Identity with secure hashing
- **CORS**:  Configured for production environments
- **HTTPS**: Enforced in production

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. 

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Rodrigo Loya** - [@rodrigoloya](https://github.com/rodrigoloya)

## 🙏 Acknowledgments

- Built with [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- Powered by [. NET 10.0](https://dotnet.microsoft.com/)
- Orchestrated with [. NET Aspire](https://learn.microsoft.com/dotnet/aspire/)
- UI components from [Bootstrap](https://getbootstrap.com/)

---

**⚠️ Disclaimer**: This application is designed for educational purposes only. Always obtain proper consent before running phishing simulations on others. 
