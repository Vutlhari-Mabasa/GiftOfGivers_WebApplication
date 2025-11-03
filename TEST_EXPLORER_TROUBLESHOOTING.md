# Test Explorer Troubleshooting Guide

## Issue: Tests Not Showing in Visual Studio Test Explorer

### Current Configuration
✅ **Framework**: xUnit (correct)  
✅ **Test SDK**: Microsoft.NET.Test.Sdk 17.12.0  
✅ **Test Runner**: xunit.runner.visualstudio 2.8.2  
✅ **Total Tests**: 75 tests detected by dotnet CLI

### Verification Steps

1. **Verify Tests Are Discovered**
   ```bash
   dotnet test --list-tests
   ```
   ✅ Should show 75 tests

2. **Run Tests via CLI**
   ```bash
   dotnet test
   ```
   ✅ Should show 69 passed, 6 failed

### Fix Test Explorer Issues

#### Solution 1: Rebuild Solution
```
1. Close Visual Studio
2. Delete bin and obj folders in both projects
3. Open Visual Studio
4. Build > Rebuild Solution
5. Test > Run All Tests
```

#### Solution 2: Clean NuGet Cache
```
1. Tools > NuGet Package Manager > Package Manager Console
2. Run: dotnet nuget locals all --clear
3. Rebuild Solution
```

#### Solution 3: Manually Refresh Test Explorer
```
1. View > Test Explorer (or Ctrl+E, T)
2. Click "Refresh" button in Test Explorer toolbar
3. Or right-click test project > Run Tests
```

#### Solution 4: Verify Project Configuration
Check that `Tests/GiftOfGivers_WebApplication.Tests/GiftOfGivers_WebApplication.Tests.csproj` contains:

```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <IsTestProject>true</IsTestProject>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
  <PackageReference Include="xunit" Version="2.9.2" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
</ItemGroup>
```

#### Solution 5: Restore Packages
```
1. Right-click Solution > Restore NuGet Packages
2. Tools > NuGet Package Manager > Manage NuGet Packages for Solution
3. Click Restore button
```

#### Solution 6: Check for Test Adapter
```
1. Extensions > Manage Extensions
2. Search for "xUnit.net Test Adapter"
3. Install if not present
4. Restart Visual Studio
```

### Quick Commands

**List all tests:**
```bash
dotnet test --list-tests
```

**Run all tests:**
```bash
dotnet test
```

**Run specific test category:**
```bash
# Unit tests only
dotnet test --filter "FullyQualifiedName~Models"

# Integration tests only
dotnet test --filter "FullyQualifiedName~Controllers"

# Load tests only
dotnet test --filter "FullyQualifiedName~Integration.ApiLoad"

# Stress tests only
dotnet test --filter "FullyQualifiedName~Integration.Stress"
```

**Verbose output:**
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Verification

After trying the above, verify with:
```bash
dotnet test --list-tests | Measure-Object -Line
```
Should show **75 tests**.

### Alternative: Use Test Explorer++

If Visual Studio Test Explorer still doesn't work:
1. Install "Test Explorer++" extension
2. Or use Rider/VS Code with C# extension
3. Or continue using `dotnet test` CLI

### Project Files Status

✅ GiftOfGivers_WebApplication.csproj - Correctly excludes Tests folder  
✅ GiftOfGivers_WebApplication.Tests.csproj - Correctly configured for xUnit  
✅ Solution file - References both projects correctly

---

**Last Verified**: Tests detected and running via dotnet CLI  
**Framework**: xUnit 2.9.2  
**Test Adapter**: xunit.runner.visualstudio 2.8.2

