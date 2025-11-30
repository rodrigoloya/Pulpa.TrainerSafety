# Phishing Trainer System Architecture Diagram

## High-Level Architecture

```mermaid
graph TB
    subgraph "Client Layer"
        PWA[Progressive Web App<br/>Blazor WebAssembly]
        Mobile[Mobile Devices<br/>PWA Installed]
    end
    
    subgraph "API Gateway"
        Gateway[API Gateway<br/>ASP.NET Core]
    end
    
    subgraph "Application Services"
        Auth[Authentication Service<br/>ASP.NET Core Identity]
        Campaign[Campaign Service]
        Analytics[Analytics Service]
        Notification[Notification Service<br/>Email/SMS Gateway]
        Billing[Billing Service<br/>Stripe Integration]
    end
    
    subgraph "Data Layer"
        SQL[(SQL Database<br/>Entity Framework Core)]
        Cache[(Redis Cache<br/>Session Management)]
    end
    
    subgraph "External Services"
        Email[Email Service<br/>SendGrid/Similar]
        SMS[SMS Service<br/>Twilio/Similar]
        Payment[Payment Gateway<br/>Stripe]
        CDN[CDN<br/>Static Assets]
    end
    
    PWA --> Gateway
    Mobile --> Gateway
    Gateway --> Auth
    Gateway --> Campaign
    Gateway --> Analytics
    Gateway --> Notification
    Gateway --> Billing
    
    Auth --> SQL
    Campaign --> SQL
    Analytics --> SQL
    Notification --> Cache
    
    Notification --> Email
    Notification --> SMS
    Billing --> Payment
    PWA --> CDN
```

## Authentication & Authorization Flow

```mermaid
sequenceDiagram
    participant User
    participant PWA
    participant Gateway
    participant Auth
    participant SQL
    participant Cache
    
    User->>PWA: Login Request
    PWA->>Gateway: POST /api/auth/login
    Gateway->>Auth: Validate Credentials
    Auth->>SQL: Check User
    SQL-->>Auth: User Data
    Auth->>Cache: Store Session
    Auth-->>Gateway: JWT Token + User Info
    Gateway-->>PWA: Authentication Response
    PWA-->>User: Login Success
    
    Note over User,Cache: User accesses protected resource
    
    User->>PWA: Request Protected Resource
    PWA->>Gateway: Request with JWT Token
    Gateway->>Auth: Validate Token
    Auth->>Cache: Check Session
    Cache-->>Auth: Session Valid
    Auth-->>Gateway: User Claims
    Gateway-->>PWA: Protected Resource
    PWA-->>User: Display Resource
```

## Campaign Execution Flow

```mermaid
sequenceDiagram
    participant User
    participant CampaignSvc
    participant NotificationSvc
    participant EmailSvc
    participant SMSSvc
    participant Tracking
    participant Analytics
    
    User->>CampaignSvc: Create Campaign
    CampaignSvc->>CampaignSvc: Schedule Campaign
    
    Note over CampaignSvc,Analytics: Campaign Execution
    
    CampaignSvc->>NotificationSvc: Send Campaign
    NotificationSvc->>EmailSvc: Send Phishing Email
    NotificationSvc->>SMSSvc: Send Phishing SMS
    
    Note over User,Analytics: User Interaction Tracking
    
    User->>EmailSvc: Open Email
    EmailSvc->>Tracking: Log Email Open
    Tracking->>Analytics: Update Results
    
    User->>EmailSvc: Click Link
    EmailSvc->>Tracking: Log Link Click
    Tracking->>Analytics: Update Results
    
    User->>EmailSvc: Submit Data
    EmailSvc->>Tracking: Log Data Submission
    Tracking->>Analytics: Update Results
    Analytics-->>CampaignSvc: Campaign Results
```

## Subscription Management Flow

```mermaid
stateDiagram-v2
    [*] --> Free
    Free --> Pro: Upgrade Request
    Pro --> Free: Cancel Subscription
    Pro --> Pro: Renew Subscription
    
    state Free {
        [*] --> LimitedFeatures
        LimitedFeatures --> [*]
    }
    
    state Pro {
        [*] --> FullFeatures
        FullFeatures --> [*]
    }
```

## Data Flow Architecture

```mermaid
graph LR
    subgraph "Input Layer"
        WebUI[Web Interface]
        MobileUI[Mobile Interface]
        API[API Requests]
    end
    
    subgraph "Processing Layer"
        Validation[Input Validation]
        BusinessLogic[Business Logic]
        Security[Security Checks]
        Authorization[Authorization]
    end
    
    subgraph "Data Access Layer"
        ORM[Entity Framework Core]
        CacheLayer[Redis Cache]
        Queries[Optimized Queries]
    end
    
    subgraph "Storage Layer"
        PrimaryDB[(Primary SQL Database)]
        BackupDB[(Backup Database)]
        FileStorage[File Storage]
    end
    
    WebUI --> Validation
    MobileUI --> Validation
    API --> Validation
    
    Validation --> BusinessLogic
    BusinessLogic --> Security
    Security --> Authorization
    
    Authorization --> ORM
    Authorization --> CacheLayer
    
    ORM --> Queries
    CacheLayer --> Queries
    
    Queries --> PrimaryDB
    Queries --> BackupDB
    Queries --> FileStorage
```

## Microservices Communication

```mermaid
graph TB
    subgraph "Core Services"
        UserService[User Service]
        CampaignService[Campaign Service]
        AnalyticsService[Analytics Service]
    end
    
    subgraph "Support Services"
        NotificationService[Notification Service]
        BillingService[Billing Service]
        ReportService[Report Service]
    end
    
    subgraph "Infrastructure"
        MessageQueue[Message Queue<br/>RabbitMQ/Azure Service Bus]
        EventBus[Event Bus]
        Logging[Logging Service]
    end
    
    UserService --> EventBus
    CampaignService --> EventBus
    AnalyticsService --> EventBus
    
    NotificationService --> MessageQueue
    BillingService --> MessageQueue
    ReportService --> MessageQueue
    
    EventBus --> Logging
    MessageQueue --> Logging
    
    UserService --> CampaignService
    CampaignService --> AnalyticsService
    AnalyticsService --> ReportService
```

## Security Architecture

```mermaid
graph TB
    subgraph "Security Layers"
        Network[Network Security<br/>Firewall/DDoS Protection]
        Application[Application Security<br/>Input Validation/Rate Limiting]
        Data[Data Security<br/>Encryption/Access Control]
        Infrastructure[Infrastructure Security<br/>Secrets Management]
    end
    
    subgraph "Security Features"
        MFA[Multi-Factor Authentication]
        RBAC[Role-Based Access Control]
        Audit[Audit Logging]
        Monitoring[Security Monitoring]
    end
    
    Network --> Application
    Application --> Data
    Data --> Infrastructure
    
    MFA --> Application
    RBAC --> Data
    Audit --> Monitoring
    Monitoring --> Network
```

## Deployment Architecture

```mermaid
graph TB
    subgraph "Production Environment"
        subgraph "Web Tier"
            LB[Load Balancer]
            App1[App Instance 1]
            App2[App Instance 2]
            App3[App Instance 3]
        end
        
        subgraph "Data Tier"
            SQLPrimary[(SQL Primary)]
            SQLSecondary[(SQL Secondary)]
            RedisCluster[(Redis Cluster)]
        end
        
        subgraph "Services"
            EmailService[Email Service]
            SMSService[SMS Service]
            PaymentService[Payment Service]
        end
    end
    
    subgraph "CDN & Static"
        CDN[CDN]
        StaticAssets[Static Assets]
    end
    
    LB --> App1
    LB --> App2
    LB --> App3
    
    App1 --> SQLPrimary
    App2 --> SQLPrimary
    App3 --> SQLPrimary
    
    SQLPrimary --> SQLSecondary
    App1 --> RedisCluster
    App2 --> RedisCluster
    App3 --> RedisCluster
    
    App1 --> EmailService
    App2 --> SMSService
    App3 --> PaymentService
    
    CDN --> StaticAssets