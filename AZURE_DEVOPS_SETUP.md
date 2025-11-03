# Azure DevOps Pipeline Setup Guide

## Pipeline Overview

The `azure-pipelines.yml` file is configured for a complete CI/CD pipeline for the GiftOfGivers Web Application.

## Pipeline Stages

### BuildAndTest Stage (Active)
- Installs .NET 8.0 SDK
- Restores NuGet packages
- Builds the solution in Release configuration
- Runs all 75 tests (unit, integration, load, stress)
- Publishes code coverage reports
- Creates deployment artifacts
- Publishes test results

**Status**: ✅ Active and configured

## Configuration

### Required Variables

Create these variables in Azure DevOps (Pipeline > Library > Variable groups):

**GiftOfGivers-Variables:**
- `AzureServiceConnection` - Azure service connection name
- `AppServiceName` - Your Azure App Service name
- `SqlConnectionString` - SQL Database connection string
- `AzureAppServiceUrl` - Base URL for load testing

### Manual Setup Steps

1. **Create Service Connection:**
   ```
   Project Settings > Service connections > New service connection
   Type: Azure Resource Manager
   Connection name: AzureServiceConnection
   ```

2. **Create Variable Group:**
   ```
   Pipelines > Library > + Variable group
   Name: GiftOfGivers-Variables
   Add variables as listed above
   ```

3. **Link Variable Group:**
   ```
   azure-pipelines.yml > Edit
   The group is already referenced in variables section
   ```

## Environment Setup

### Required Azure Resources

1. **Azure App Service**
   - Runtime stack: .NET 8
   - Operating System: Windows
   - Hosting plan: Basic or higher

2. **Azure SQL Database**
   - Service tier: Basic or higher
   - Connection pooling: Enabled
   - Firewall rules: Allow Azure services

3. **Application Settings** (App Service)
   ```
   ConnectionStrings:DefaultConnection = <Your Connection String>
   ASPNETCORE_ENVIRONMENT = Production
   ```

## Pipeline Triggers

**Current Configuration:**
✅ **Active**: BuildAndTest Stage (tests, coverage, and artifacts)  
✅ **Tests**: All 75 tests run automatically  
✅ **Coverage**: Code coverage published  
✅ **Artifacts**: Ready for deployment

**Automatic Builds:**
- Push to `main` branch
- Push to `develop` branch
- Pull requests to `main` or `develop`

**Excluded Paths:**
- `*.md` files (documentation changes won't trigger builds)

## Testing

### Code Coverage
The pipeline publishes code coverage reports using Coverlet.

**Configuration File:** `coverlet.runsettings`

**Coverage:**
- Includes: Application code only
- Excludes: Tests, Migrations

### Test Results
- All 75 tests must pass
- Tests run automatically on every build
- Results published to Azure DevOps

## Deployment

### Production Deployment
- Only deploys on `main` branch
- Runs after successful build and tests
- Applies database migrations automatically

### Rollback
If deployment fails:
1. Previous version remains active
2. Check App Service logs
3. Revert problematic changes
4. Redeploy

## Load Testing

### Python Scripts
- `Tests/LoadTesting/LoadTestScript.py`
- `Tests/StressTesting/StressTestScript.py`

### Setup
```bash
# Install Python 3.x on build agent
pip install requests
```

### Results
- Load test results available in build logs
- Performance metrics tracked over time

## Monitoring

### Azure DevOps
- Build history and logs
- Test results and coverage
- Deployment status

### Azure Portal
- App Service metrics
- Application Insights (if configured)
- Database performance

## Troubleshooting

### Build Failures

**Tests Failing:**
```bash
dotnet test  # Run locally to reproduce
```

**Package Restore Issues:**
```bash
dotnet restore --force
dotnet nuget locals all --clear
```

### Deployment Failures

**Database Migration Errors:**
- Check connection string
- Verify database exists
- Review migration history

**Application Startup Errors:**
- Check App Service logs
- Verify environment variables
- Review connection strings

## Security

### Best Practices
- ✅ Use Key Vault for secrets
- ✅ Enable HTTPS only
- ✅ Configure managed identity
- ✅ Regular security updates
- ✅ Monitor for vulnerabilities

## CI/CD Workflow

```
┌─────────────────┐
│  Code Push/PR   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Build & Test    │
│ - Build         │
│ - Run 75 tests  │
│ - Publish       │
│ - Code Coverage │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Artifacts Ready │
│ for deployment  │
└─────────────────┘
```

## YAML Pipeline Reference

**File:** `azure-pipelines.yml`

**Features:**
- Multi-stage pipeline
- Test execution and reporting
- Code coverage
- Automated deployment
- Load testing integration

## Manual Deployment

If you need to deploy manually:

```bash
# Build and publish
dotnet publish -c Release -o ./publish

# Deploy to Azure
az webapp deploy \
  --resource-group YourResourceGroup \
  --name YourAppServiceName \
  --src-path ./publish.zip \
  --type zip
```

## Cost Optimization

- Use Basic tier for development
- Enable auto-shutdown during off-hours
- Configure auto-scale rules
- Monitor resource usage

## Support

For issues:
1. Check pipeline logs in Azure DevOps
2. Review application logs in App Service
3. Consult troubleshooting section above
4. Check DOCUMENTATION.md for more details

---

**Pipeline Status**: ✅ Configured  
**Last Updated**: November 2024  
**Version**: 1.0

