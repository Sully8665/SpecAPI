# SpecAPI 🚀

A modern, extensible API testing framework built with .NET 8 that allows you to define and execute HTTP API tests using YAML or JSON specifications. SpecAPI provides comprehensive validation, authentication support, detailed reporting capabilities, and enterprise-grade CI/CD integration.

## ✨ Features

### Core Functionality
- **Multi-format Support**: Define tests in YAML or JSON format
- **HTTP Methods**: Support for GET, POST, PUT, DELETE, PATCH, and more
- **Request Configuration**: Headers, request bodies, and custom configurations
- **Response Validation**: Status codes, response bodies, headers, and response time validation
- **Authentication**: Built-in support for Basic Auth, Bearer Token, and API Key authentication
- **Variable Support**: Use variables in your test specifications for dynamic testing
- **Detailed Reporting**: Console output and comprehensive Markdown reports

### Architecture & Design
- **SOLID Principles**: Clean, maintainable code following SOLID design principles
- **Dependency Injection**: Modular architecture with IoC container
- **Extensible Framework**: Easy to extend with custom validators, authentication handlers, and reporters
- **Async/Await**: Full async support for better performance
- **Error Handling**: Comprehensive error handling and logging

### Validation Capabilities
- **Status Code Validation**: Verify expected HTTP status codes
- **Response Body Validation**: JSON comparison and content validation
- **Header Validation**: Verify expected response headers
- **Response Time Validation**: Performance testing with configurable timeouts
- **Custom Validators**: Extensible validation framework

## 🚀 CI/CD Integration

SpecAPI comes with comprehensive CI/CD integration out of the box, providing automated testing, security scanning, and deployment capabilities.

### 🔄 Automated Pipeline

Our GitHub Actions workflows provide:

#### **CI/CD Pipeline** (`.github/workflows/ci.yml`)
- **Build & Test**: Automated building and testing on every push/PR
- **Code Quality**: SonarCloud analysis and Snyk security scanning
- **Multi-platform Builds**: Linux, Windows, and macOS binaries
- **Docker Images**: Automated container builds and pushes
- **Release Management**: Automatic GitHub releases with artifacts

#### **Deployment Pipeline** (`.github/workflows/deploy.yml`)
- **Staging Deployment**: Automatic deployment to staging environment
- **Production Deployment**: Manual approval-based production deployment
- **Smoke Tests**: Automated health checks after deployment
- **Rollback Capability**: Automatic rollback on deployment failures
- **Slack Notifications**: Real-time deployment status updates

#### **Security Scanning** (`.github/workflows/security.yml`)
- **Dependency Scanning**: OWASP Dependency Check for vulnerabilities
- **Code Analysis**: CodeQL and Semgrep for security issues
- **Container Security**: Trivy and Hadolint for Docker security
- **Secrets Detection**: TruffleHog and Gitleaks for exposed secrets
- **Compliance Checks**: License and security policy validation

#### **Documentation** (`.github/workflows/docs.yml`)
- **Auto-generation**: API documentation from code comments
- **Link Validation**: Automated broken link detection
- **Spell Checking**: Documentation quality assurance
- **Multi-platform Deployment**: GitHub Pages and Netlify deployment

### 🐳 Docker Support

```bash
# Build and run with Docker
docker build -t specapi .
docker run specapi Examples/example-test.yaml

# Use Docker Compose for development
docker-compose up specapi

# Run with full stack (database, cache, etc.)
docker-compose --profile full up
```

### 🔧 Environment Configuration

The CI/CD pipeline supports multiple environments:

```yaml
# Staging Environment
- name: Deploy to Staging
  environment: staging
  secrets:
    - AZURE_WEBAPP_PUBLISH_PROFILE_STAGING
    - STAGING_API_URL

# Production Environment  
- name: Deploy to Production
  environment: production
  secrets:
    - AZURE_WEBAPP_PUBLISH_PROFILE_PRODUCTION
    - PRODUCTION_API_URL
```

### 🔒 Security Features

- **Automated Security Scanning**: Daily vulnerability assessments
- **Secret Detection**: Prevents accidental secret exposure
- **Container Security**: Docker image vulnerability scanning
- **Compliance Monitoring**: License and policy enforcement
- **Security Notifications**: Real-time security alerts

### 📊 Monitoring & Reporting

- **Build Status**: Real-time pipeline status tracking
- **Test Coverage**: Code coverage reporting with Codecov
- **Security Metrics**: Vulnerability trend analysis
- **Deployment History**: Complete deployment audit trail
- **Performance Monitoring**: Response time and reliability metrics

## 🚀 Quick Start

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later

### Installation & Usage

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SpecAPI
   ```

2. **Build the project**
   ```bash
   dotnet build src/SpecAPI/SpecAPI.csproj
   ```

3. **Run tests**
   ```bash
   dotnet run --project src/SpecAPI/SpecAPI.csproj Examples/example-test.yaml
   ```

## 📝 Test Specification Format

### Basic Test Structure
```yaml
- name: "Test Description"
  request:
    method: "GET"
    url: "https://api.example.com/endpoint"
    headers:
      Accept: "application/json"
      Authorization: "Bearer your-token"
    body:
      key: "value"
  expect:
    statusCode: 200
    body:
      expected_field: "expected_value"
    headers:
      Content-Type: "application/json"
    maxResponseTimeMs: 5000
```

### Authentication Examples

#### Basic Authentication
```yaml
- name: "Basic Auth Test"
  request:
    method: "GET"
    url: "https://api.example.com/protected"
    auth:
      type: "basic"
      username: "your-username"
      password: "your-password"
  expect:
    statusCode: 200
```

#### Bearer Token Authentication
```yaml
- name: "Bearer Token Test"
  request:
    method: "GET"
    url: "https://api.example.com/protected"
    auth:
      type: "bearer"
      token: "your-jwt-token"
  expect:
    statusCode: 200
```

#### API Key Authentication
```yaml
- name: "API Key Test"
  request:
    method: "GET"
    url: "https://api.example.com/protected"
    auth:
      type: "apikey"
      in: "header"  # or "query"
      name: "X-API-Key"
      value: "your-api-key"
  expect:
    statusCode: 200
```

### Variables Support
```yaml
variables:
  base_url: "https://api.example.com"
  api_key: "your-api-key"

- name: "Test with Variables"
  request:
    method: "GET"
    url: "{{base_url}}/users"
    auth:
      type: "apikey"
      in: "header"
      name: "X-API-Key"
      value: "{{api_key}}"
  expect:
    statusCode: 200
```

## 📊 Output & Reporting

### Console Output
SpecAPI provides real-time console output showing:
- Test execution progress
- Pass/fail status with emojis
- Response times
- Error details

### Markdown Reports
Detailed reports are generated in `Output/result.md` containing:
- Test summary with pass/fail counts
- Individual test results with request/response details
- Response times and validation results
- Error messages and stack traces

Example report structure:
```markdown
# Test Results

## Test Name
- **URL**: https://api.example.com/endpoint
- **Method**: GET
- **Expected Status**: 200
- **Actual Status**: 200
- **Response Time**: 245ms
- **Result**: ✅ PASSED
```

## 🏗️ Architecture

### Core Components

#### Test Execution Pipeline
```
Test Specification → Test Loader → Test Runner → Validators → Reporter
```

#### Key Interfaces
- `ITestLoader`: Loads test specifications from YAML/JSON
- `ITestRunner`: Executes HTTP requests
- `IValidator`: Validates responses
- `IAuthenticationHandler`: Handles authentication
- `IResultReporter`: Generates reports

#### Service Architecture
- **TestExecutionService**: Orchestrates the entire test execution
- **HttpTestRunner**: Executes HTTP requests using HttpClient
- **ValidationManager**: Coordinates validation across multiple validators
- **AuthenticationManager**: Manages different authentication types
- **ServiceContainer**: Dependency injection container

### Extensibility Points

#### Custom Validators
```csharp
public class CustomValidator : IValidator
{
    public ValidationResult Validate(TestResult result, Expect expect)
    {
        // Custom validation logic
        return new ValidationResult { IsValid = true };
    }
}
```

#### Custom Authentication Handlers
```csharp
public class CustomAuthHandler : IAuthenticationHandler
{
    public void ApplyAuthentication(HttpRequestMessage request, Auth auth)
    {
        // Custom authentication logic
    }
}
```

#### Custom Reporters
```csharp
public class CustomReporter : IResultReporter
{
    public void ReportResults(List<TestResult> results)
    {
        // Custom reporting logic
    }
}
```

## 🔧 Configuration

### Project Structure
```
SpecAPI/
├── src/SpecAPI/
│   ├── Authentication/     # Authentication handlers
│   ├── DependencyInjection/ # IoC container setup
│   ├── Interfaces/         # Core interfaces
│   ├── Loading/           # Test specification loaders
│   ├── Models/            # Data models
│   ├── Reporting/         # Report generators
│   ├── Running/           # Test execution
│   ├── Services/          # Core services
│   ├── Utils/             # Utilities
│   └── Validation/        # Response validators
├── Examples/              # Sample test files
├── .github/workflows/     # CI/CD pipelines
├── Dockerfile             # Container configuration
├── docker-compose.yml     # Development environment
└── Output/                # Generated reports
```

## 🔒 Security

### Security Policy
We take security seriously. Please see our [Security Policy](.github/SECURITY.md) for details on:
- Vulnerability reporting process
- Security best practices
- Security features and tools
- Compliance and standards

### Security Tools Integration
Our CI/CD pipeline includes comprehensive security scanning:
- **OWASP Dependency Check**: Vulnerability scanning in dependencies
- **CodeQL & Semgrep**: Static code analysis for security issues
- **Trivy & Hadolint**: Container security scanning
- **TruffleHog & Gitleaks**: Secrets detection
- **Compliance Checks**: License and policy validation

## 🚀 Future Enhancements

### Planned Features
- **Parallel Test Execution**: Run multiple tests concurrently
- **Web UI**: Browser-based interface for test creation and management
- **Test Suites**: Organize tests into suites with shared configurations
- **Data-Driven Testing**: Support for CSV/Excel data sources
- **Performance Testing**: Load testing and performance metrics
- **Mock Server**: Built-in mock server for testing
- **GraphQL Support**: Native GraphQL query testing
- **WebSocket Testing**: Real-time communication testing
- **Custom Assertions**: DSL for complex validation scenarios

### Advanced Features
- **Test Environment Management**: Multiple environment configurations
- **Test Data Management**: Dynamic test data generation
- **Scheduled Testing**: Automated test execution
- **Test History**: Historical test results and trends
- **Integration Testing**: Database and external service testing
- **Security Testing**: Built-in security vulnerability scanning
- **API Documentation**: Generate API documentation from tests
- **Contract Testing**: Consumer-driven contract testing

## 🤝 Contributing

We welcome contributions! Please see our contributing guidelines for details on:
- Code style and standards
- Testing requirements
- Pull request process
- Issue reporting

### Development Setup
```bash
# Clone the repository
git clone <repository-url>
cd SpecAPI

# Build the project
dotnet build src/SpecAPI/SpecAPI.csproj

# Run tests
dotnet test src/SpecAPI/SpecAPI.csproj

# Run with Docker Compose
docker-compose up specapi
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Issues**: Report bugs and feature requests on GitHub
- **Documentation**: Check the examples and inline code documentation
- **Community**: Join our community discussions
- **Security**: Report security vulnerabilities to [security@specapi.dev](mailto:security@specapi.dev)

## 📊 Project Status

![CI/CD Pipeline](https://github.com/your-repo/specapi/workflows/CI%2FCD%20Pipeline/badge.svg)
![Code Coverage](https://codecov.io/gh/your-repo/specapi/branch/main/graph/badge.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0-blue.svg)

---

**Built with ❤️ using .NET 8 and modern software engineering practices** 