# Progressive Web App (PWA) Configuration & Deployment Strategy

## Overview
The Phishing Trainer application is designed as a Progressive Web App (PWA) using Blazor WebAssembly, providing a native app-like experience across all devices while maintaining web accessibility and discoverability.

## PWA Configuration

### Service Worker Implementation
```javascript
// wwwroot/service-worker.js
const CACHE_NAME = 'phishing-trainer-v1.0.0';
const urlsToCache = [
    '/',
    '/index.html',
    '/manifest.json',
    '/_framework/blazor.webassembly.js',
    '/_framework/blazor.boot.json',
    '/css/app.css',
    '/css/bootstrap.min.css',
    '/js/app.js',
    '/images/icons/icon-192x192.png',
    '/images/icons/icon-512x512.png'
];

// Install event - cache resources
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
    );
});

// Fetch event - serve from cache when offline
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // Cache hit - return response
                if (response) {
                    return response;
                }

                // Clone the request
                const fetchRequest = event.request.clone();

                return fetch(fetchRequest).then(response => {
                    // Check if valid response
                    if (!response || response.status !== 200 || response.type !== 'basic') {
                        return response;
                    }

                    // Clone the response
                    const responseToCache = response.clone();

                    caches.open(CACHE_NAME)
                        .then(cache => {
                            cache.put(event.request, responseToCache);
                        });

                    return response;
                }).catch(() => {
                    // Return offline page for navigation requests
                    if (event.request.destination === 'document') {
                        return caches.match('/offline.html');
                    }
                });
            })
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

// Background sync for offline actions
self.addEventListener('sync', event => {
    if (event.tag === 'background-sync-campaigns') {
        event.waitUntil(syncCampaignData());
    }
});

async function syncCampaignData() {
    // Sync offline campaign data when back online
    const offlineData = await getOfflineData();
    for (const data of offlineData) {
        try {
            await fetch('/api/campaigns/sync', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            await removeOfflineData(data.id);
        } catch (error) {
            console.error('Failed to sync data:', error);
        }
    }
}
```

### Web App Manifest
```json
{
  "name": "Phishing Trainer - Security Awareness Training",
  "short_name": "Phishing Trainer",
  "description": "Learn to identify phishing attacks through safe simulations",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#007bff",
  "orientation": "portrait-primary",
  "scope": "/",
  "lang": "en",
  "categories": ["education", "security", "productivity"],
  "icons": [
    {
      "src": "images/icons/icon-72x72.png",
      "sizes": "72x72",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-96x96.png",
      "sizes": "96x96",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-128x128.png",
      "sizes": "128x128",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-144x144.png",
      "sizes": "144x144",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-152x152.png",
      "sizes": "152x152",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-192x192.png",
      "sizes": "192x192",
      "type": "image/png",
      "purpose": "any maskable"
    },
    {
      "src": "images/icons/icon-384x384.png",
      "sizes": "384x384",
      "type": "image/png"
    },
    {
      "src": "images/icons/icon-512x512.png",
      "sizes": "512x512",
      "type": "image/png",
      "purpose": "any maskable"
    }
  ],
  "shortcuts": [
    {
      "name": "Create Campaign",
      "short_name": "New Campaign",
      "description": "Create a new phishing simulation campaign",
      "url": "/campaigns/create",
      "icons": [
        {
          "src": "images/icons/shortcut-create.png",
          "sizes": "96x96"
        }
      ]
    },
    {
      "name": "View Dashboard",
      "short_name": "Dashboard",
      "description": "View analytics and campaign results",
      "url": "/analytics/dashboard",
      "icons": [
        {
          "src": "images/icons/shortcut-dashboard.png",
          "sizes": "96x96"
        }
      ]
    }
  ],
  "screenshots": [
    {
      "src": "images/screenshots/desktop-dashboard.png",
      "sizes": "1280x720",
      "type": "image/png",
      "form_factor": "wide",
      "label": "Dashboard view on desktop"
    },
    {
      "src": "images/screenshots/mobile-campaign.png",
      "sizes": "375x667",
      "type": "image/png",
      "form_factor": "narrow",
      "label": "Campaign creation on mobile"
    }
  ]
}
```

### Blazor PWA Configuration
```csharp
// Program.cs
builder.Services.AddServiceWorker();

// In your main layout or app component
public class App : ComponentBase
{
    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] public ServiceWorkerContainer ServiceWorkerContainer { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RegisterServiceWorker();
            await SetupOfflineSupport();
        }
    }

    private async Task RegisterServiceWorker()
    {
        await JSRuntime.InvokeVoidAsync("navigator.serviceWorker.register", "/service-worker.js");
    }

    private async Task SetupOfflineSupport()
    {
        // Check if app is running standalone (PWA mode)
        var isStandalone = await JSRuntime.InvokeAsync<bool>("eval", 
            "window.matchMedia('(display-mode: standalone)').matches");

        if (isStandalone)
        {
            // Setup PWA-specific features
            await SetupPushNotifications();
            await SetupBackgroundSync();
        }
    }

    private async Task SetupPushNotifications()
    {
        // Request permission for push notifications
        var permission = await JSRuntime.InvokeAsync<string>("eval", 
            "Notification.requestPermission()");

        if (permission == "granted")
        {
            // Subscribe to push notifications
            await JSRuntime.InvokeVoidAsync("subscribeToPushNotifications");
        }
    }

    private async Task SetupBackgroundSync()
    {
        // Register background sync for offline actions
        await JSRuntime.InvokeVoidAsync("registerBackgroundSync");
    }
}
```

## Offline Support Implementation

### Offline Data Storage
```csharp
public class OfflineStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<OfflineStorageService> _logger;

    public async Task StoreOfflineDataAsync<T>(string key, T data)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", 
                key, JsonSerializer.Serialize(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to store offline data for key: {Key}", key);
        }
    }

    public async Task<T> GetOfflineDataAsync<T>(string key)
    {
        try
        {
            var data = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve offline data for key: {Key}", key);
            return default;
        }
    }

    public async Task RemoveOfflineDataAsync(string key)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove offline data for key: {Key}", key);
        }
    }

    public async Task<List<T>> GetAllOfflineDataAsync<T>(string prefix)
    {
        try
        {
            var keys = await _jsRuntime.InvokeAsync<string[]>("Object.keys", 
                "localStorage");
            
            var results = new List<T>();
            
            foreach (var key in keys.Where(k => k.StartsWith(prefix)))
            {
                var data = await GetOfflineDataAsync<T>(key);
                if (data != null)
                {
                    results.Add(data);
                }
            }
            
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve all offline data with prefix: {Prefix}", prefix);
            return new List<T>();
        }
    }
}
```

### Offline Campaign Management
```csharp
public class OfflineCampaignService
{
    private readonly OfflineStorageService _offlineStorage;
    private readonly ICampaignService _campaignService;
    private readonly IConnectivityService _connectivityService;

    public async Task<CampaignResult> CreateCampaignAsync(CreateCampaignRequest request)
    {
        var campaign = new OfflineCampaign
        {
            Id = Guid.NewGuid().ToString(),
            Request = request,
            CreatedAt = DateTime.UtcNow,
            SyncStatus = SyncStatus.Pending
        };

        if (await _connectivityService.IsOnlineAsync())
        {
            // Try to sync immediately if online
            try
            {
                var result = await _campaignService.CreateCampaignAsync(request, request.OwnerId);
                campaign.SyncStatus = SyncStatus.Synced;
                campaign.ServerId = result.Campaign.Id;
            }
            catch
            {
                // Store for later sync if server is unavailable
                await _offlineStorage.StoreOfflineDataAsync($"campaign_{campaign.Id}", campaign);
            }
        }
        else
        {
            // Store for later sync when back online
            await _offlineStorage.StoreOfflineDataAsync($"campaign_{campaign.Id}", campaign);
        }

        return CampaignResult.Success(campaign.ToCampaign());
    }

    public async Task SyncOfflineCampaignsAsync()
    {
        if (!await _connectivityService.IsOnlineAsync())
            return;

        var offlineCampaigns = await _offlineStorage.GetAllOfflineDataAsync<OfflineCampaign>("campaign_");
        
        foreach (var campaign in offlineCampaigns.Where(c => c.SyncStatus == SyncStatus.Pending))
        {
            try
            {
                var result = await _campaignService.CreateCampaignAsync(campaign.Request, campaign.Request.OwnerId);
                
                campaign.SyncStatus = SyncStatus.Synced;
                campaign.ServerId = result.Campaign.Id;
                campaign.SyncedAt = DateTime.UtcNow;
                
                await _offlineStorage.StoreOfflineDataAsync($"campaign_{campaign.Id}", campaign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync campaign {CampaignId}", campaign.Id);
            }
        }
    }
}

public class OfflineCampaign
{
    public string Id { get; set; }
    public CreateCampaignRequest Request { get; set; }
    public DateTime CreatedAt { get; set; }
    public SyncStatus SyncStatus { get; set; }
    public string ServerId { get; set; }
    public DateTime? SyncedAt { get; set; }
}

public enum SyncStatus
{
    Pending,
    Synced,
    Failed
}
```

## Push Notifications

### Push Notification Service
```csharp
public class PushNotificationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IConfiguration _config;

    public async Task<bool> RequestPermissionAsync()
    {
        var permission = await _jsRuntime.InvokeAsync<string>("Notification.requestPermission");
        return permission == "granted";
    }

    public async Task SubscribeAsync(string userId)
    {
        try
        {
            var subscription = await _jsRuntime.InvokeAsync<object>("subscribeToPush", 
                _config["VapidPublicKey"]);

            if (subscription != null)
            {
                await StoreSubscriptionAsync(userId, subscription);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to subscribe to push notifications");
        }
    }

    public async Task SendCampaignUpdateAsync(string userId, string campaignName, string status)
    {
        var subscription = await GetSubscriptionAsync(userId);
        if (subscription == null) return;

        var payload = new
        {
            title = "Campaign Update",
            body = $"Campaign '{campaignName}' status: {status}",
            icon = "/images/icons/icon-192x192.png",
            badge = "/images/icons/badge-72x72.png",
            tag = $"campaign-update-{campaignName}",
            data = new
            {
                url = $"/campaigns/{campaignName}"
            }
        };

        await SendPushNotificationAsync(subscription, payload);
    }

    private async Task StoreSubscriptionAsync(string userId, object subscription)
    {
        // Store subscription in database
        var pushSubscription = new PushSubscription
        {
            UserId = userId,
            Endpoint = subscription.endpoint,
            P256DH = subscription.keys.p256dh,
            Auth = subscription.keys.auth,
            CreatedAt = DateTime.UtcNow
        };

        _context.PushSubscriptions.Add(pushSubscription);
        await _context.SaveChangesAsync();
    }
}
```

### Client-side Push Notification Handler
```javascript
// wwwroot/js/push-notifications.js
async function subscribeToPush(vapidPublicKey) {
    if (!('serviceWorker' in navigator) || !('PushManager' in window)) {
        console.log('Push messaging is not supported');
        return null;
    }

    try {
        const registration = await navigator.serviceWorker.ready;
        const subscription = await registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: urlB64ToUint8Array(vapidPublicKey)
        });

        console.log('Push subscription:', subscription);
        return subscription;
    } catch (error) {
        console.error('Failed to subscribe to push:', error);
        return null;
    }
}

// Handle incoming push messages
self.addEventListener('push', event => {
    const options = {
        body: event.data.text(),
        icon: '/images/icons/icon-192x192.png',
        badge: '/images/icons/badge-72x72.png',
        vibrate: [100, 50, 100],
        data: {
            dateOfArrival: Date.now(),
            primaryKey: 1
        },
        actions: [
            {
                action: 'explore',
                title: 'View Campaign',
                icon: '/images/icons/checkmark.png'
            },
            {
                action: 'close',
                title: 'Close',
                icon: '/images/icons/xmark.png'
            }
        ]
    };

    if (event.data) {
        const data = event.data.json();
        options.title = data.title;
        options.body = data.body;
        options.data = data.data;
        options.tag = data.tag;
    }

    event.waitUntil(
        self.registration.showNotification(options.title, options)
    );
});

// Handle notification clicks
self.addEventListener('notificationclick', event => {
    console.log('Notification click received.');

    event.notification.close();

    if (event.action === 'explore') {
        // Open the app to the specific campaign
        event.waitUntil(
            clients.openWindow(event.notification.data.url)
        );
    } else if (event.action === 'close') {
        // Just close the notification
        event.notification.close();
    } else {
        // Default action - open the app
        event.waitUntil(
            clients.openWindow('/')
        );
    }
});

function urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/-/g, '+')
        .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}
```

## Deployment Strategy

### Azure DevOps Pipeline
```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
    - main
    - develop

variables:
  buildConfiguration: 'Release'
  azureSubscription: 'PhishingTrainer-Prod'
  webAppName: 'phishing-trainer-app'

stages:
- stage: Build
  displayName: 'Build Stage'
  jobs:
  - job: Build
    displayName: 'Build Job'
    pool:
      vmImage: 'windows-latest'
    steps:
    - task: UseDotNet@2
      displayName: 'Use .NET 8'
      inputs:
        packageType: 'sdk'
        version: '8.x'

    - task: DotNetCoreCLI@2
      displayName: 'Restore NuGet packages'
      inputs:
        command: 'restore'
        projects: '**/*.csproj'

    - task: DotNetCoreCLI@2
      displayName: 'Build solution'
      inputs:
        command: 'build'
        projects: '**/*.csproj'
        arguments: '--configuration $(buildConfiguration) --no-restore'

    - task: DotNetCoreCLI@2
      displayName: 'Run tests'
      inputs:
        command: 'test'
        projects: '**/*Tests/*.csproj'
        arguments: '--configuration $(buildConfiguration) --no-build --collect:"XPlat Code Coverage"'

    - task: PublishCodeCoverageResults@1
      displayName: 'Publish code coverage'
      inputs:
        codeCoverageTool: Cobertura
        summaryFileLocation: '$(Agent.TempDirectory)/**/coverage.cobertura.xml'

    - task: DotNetCoreCLI@2
      displayName: 'Publish Blazor WASM'
      inputs:
        command: 'publish'
        projects: 'PhishingTrainer/PhishingTrainer.csproj'
        arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory) --no-build'

    - task: PublishBuildArtifacts@1
      displayName: 'Publish artifacts'
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'drop'

- stage: Deploy
  displayName: 'Deploy Stage'
  dependsOn: Build
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - deployment: Deploy
    displayName: 'Deploy Job'
    environment: 'production'
    pool:
      vmImage: 'windows-latest'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            displayName: 'Deploy to Azure Web App'
            inputs:
              azureSubscription: '$(azureSubscription)'
              appName: '$(webAppName)'
              package: '$(Pipeline.Workspace)/drop/**/*.zip'
              runtimeStack: 'DOTNETCORE|8.0'
```

### Docker Configuration
```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PhishingTrainer/PhishingTrainer.csproj", "PhishingTrainer/"]
COPY ["PhishingTrainer.Shared/PhishingTrainer.Shared.csproj", "PhishingTrainer.Shared/"]
RUN dotnet restore "PhishingTrainer/PhishingTrainer.csproj"
COPY . .
WORKDIR "/src/PhishingTrainer"
RUN dotnet build "PhishingTrainer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhishingTrainer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Enable compression
RUN apt-get update && apt-get install -y \
    gzip \
    && rm -rf /var/lib/apt/lists/*

# Configure nginx for static file compression
COPY nginx.conf /etc/nginx/nginx.conf

ENTRYPOINT ["dotnet", "PhishingTrainer.dll"]
```

### Nginx Configuration for Static Files
```nginx
# nginx.conf
events {
    worker_connections 1024;
}

http {
    include       /etc/nginx/mime.types;
    default_type  application/octet-stream;

    # Gzip compression
    gzip on;
    gzip_vary on;
    gzip_min_length 1024;
    gzip_proxied expired no-cache no-store private must-revalidate auth;
    gzip_types
        text/plain
        text/css
        text/xml
        text/javascript
        application/javascript
        application/xml+rss
        application/json;

    server {
        listen 80;
        server_name localhost;
        root /app/wwwroot;
        index index.html;

        # Static file caching
        location ~* \.(css|js|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
            expires 1y;
            add_header Cache-Control "public, immutable";
            add_header X-Content-Type-Options nosniff;
        }

        # Service worker
        location /service-worker.js {
            expires off;
            add_header Cache-Control "no-cache, no-store, must-revalidate";
            add_header Pragma "no-cache";
        }

        # Blazor WASM files
        location /_framework/ {
            expires 1y;
            add_header Cache-Control "public, immutable";
        }

        # API routes
        location /api/ {
            proxy_pass http://localhost:5000;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }

        # Fallback to Blazor routing
        location / {
            try_files $uri $uri/ /index.html;
        }

        # Security headers
        add_header X-Frame-Options "SAMEORIGIN" always;
        add_header X-Content-Type-Options "nosniff" always;
        add_header X-XSS-Protection "1; mode=block" always;
        add_header Referrer-Policy "strict-origin-when-cross-origin" always;
        add_header Content-Security-Policy "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:; connect-src 'self' https://api.stripe.com;" always;
    }
}
```

### Environment Configuration

#### Development Environment
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PhishingTrainer_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Stripe": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_...",
    "WebhookSecret": "whsec_...",
    "ProPriceId": "price_..."
  },
  "SendGrid": {
    "ApiKey": "SG.dev..."
  },
  "Twilio": {
    "AccountSid": "AC...",
    "AuthToken": "...",
    "FromNumber": "+15017122661"
  },
  "App": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

#### Production Environment
```json
// appsettings.Production.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:phishing-trainer-db.database.windows.net,1433;Initial Catalog=PhishingTrainer_Prod;Persist Security Info=False;User ID=phishing_trainer;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "Stripe": {
    "PublishableKey": "pk_live_...",
    "SecretKey": "sk_live_...",
    "WebhookSecret": "whsec_...",
    "ProPriceId": "price_..."
  },
  "SendGrid": {
    "ApiKey": "SG.prod..."
  },
  "Twilio": {
    "AccountSid": "AC...",
    "AuthToken": "...",
    "FromNumber": "+15017122661"
  },
  "App": {
    "BaseUrl": "https://phishing-trainer.azurewebsites.net"
  },
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=..."
  }
}
```

## Performance Optimization

### Bundle Optimization
```xml
<!-- PhishingTrainer.csproj -->
<PropertyGroup>
  <BlazorWebAssemblyPwa>true</BlazorWebAssemblyPwa>
  <ServiceWorkerAssetsManifest>service-worker-assets.js</ServiceWorkerAssetsManifest>
  <CompressionEnabled>true</CompressionEnabled>
  <TrimUnusedDependencies>true</TrimUnusedDependencies>
  <PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>

<ItemGroup>
  <BlazorWebAssemblyLazyLoad Include="PhishingTrainer.Shared.dll" />
</ItemGroup>
```

### CDN Configuration
```csharp
// Startup.cs or Program.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();
    services.AddBlazorWebView();

    // Configure CDN for static assets
    services.Configure<StaticFileOptions>(options =>
    {
        options.OnPrepareResponse = ctx =>
        {
            if (ctx.File.Name.EndsWith(".css") || 
                ctx.File.Name.EndsWith(".js") || 
                ctx.File.Name.EndsWith(".png") || 
                ctx.File.Name.EndsWith(".jpg") || 
                ctx.File.Name.EndsWith(".svg"))
            {
                ctx.Context.Response.Headers.Add("Cache-Control", "public, max-age=31536000");
            }
        };
    });
}
```

This PWA configuration and deployment strategy provides:

1. **Complete PWA functionality** with service workers and offline support
2. **Cross-platform compatibility** working on desktop and mobile devices
3. **Offline data synchronization** for seamless user experience
4. **Push notifications** for real-time campaign updates
5. **Automated CI/CD pipeline** for reliable deployments
6. **Performance optimization** with compression and caching
7. **Environment-specific configurations** for development and production
8. **Security headers** and content security policies
9. **Docker containerization** for scalable deployment
10. **Monitoring and analytics** integration

The strategy ensures the application provides a native app-like experience while maintaining the accessibility and reach of a web application.