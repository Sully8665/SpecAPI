# SpecAPI

SpecAPI is a simple and extensible API testing tool that runs HTTP tests defined in YAML or JSON files, and generates a Markdown report with test results.

---

## Features

- Define API tests with requests and expected responses in YAML
- Supports HTTP methods, headers, request bodies, and expected status codes and response bodies
- Reports test results in console and outputs a detailed Markdown report (`Output/result.md`)
- Basic validation of status code, response body, and optional max response time
- Easily extensible to support more advanced testing features

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later installed on your system

### Build and Run

1. Clone the repository or download the source code.

2. Open a terminal in the project directory.

3. Build the project:

   ```bash
   dotnet build

4. Run tests by specifying a YAML or JSON test file path:
	dotnet run -- input/sample-test.yaml

Writing Tests
Tests are defined as a list in YAML or JSON format. Each test case consists of:

name: a descriptive test name

request: HTTP request details

method: HTTP method (GET, POST, etc.)

url: full URL of the API endpoint

headers: optional dictionary of HTTP headers

body: optional request body as a string

expect: expected results

statusCode: expected HTTP status code (e.g., 200)

body: optional expected JSON object or string to compare response body

headers: optional expected headers (key-value pairs)

maxResponseTimeMs: optional max response time in milliseconds

Example YAML test file:
- name: Get Google Homepage
  request:
    method: GET
    url: "https://www.google.com"
  expect:
    statusCode: 200

- name: Get GitHub API root
  request:
    method: GET
    url: "https://api.github.com"
    headers:
      User-Agent: "SpecAPI-Client"
  expect:
    statusCode: 200
    body:
      current_user_url: "https://api.github.com/user"

Output
Test results are shown in the console and saved in Output/result.md as a Markdown report with detailed information about each test.

Extending SpecAPI
Future improvements can include:

Parallel test execution

A web-based UI to create, run, and view tests

CI/CD integration support
