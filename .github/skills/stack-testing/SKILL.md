---
name: stack-testing
description: "Write and run tests for PayBille v2 stack: XUnit for .NET backend, Vitest for Nuxt frontend. Use for unit tests, integration tests, or test strategy planning."
---

# Testing PayBille v2 Stack

## When to Use
- Writing unit tests for services or components
- Planning test strategy for a feature
- Setting up integration tests
- Debugging failing tests

## Procedure

### 1. Backend Testing (.NET XUnit)
- Use [XUnit test template](./assets/xunit-test-template.cs)
- Test services, controllers, and business logic
- Mock repositories and dependencies
- Run with: `dotnet test`

### 2. Frontend Testing (Nuxt Vitest)
- Use [Vitest component template](./assets/vitest-component-template.ts)
- Test components, composables, and stores
- Mock API calls
- Run with: `npm run test`

### 3. Run All Tests
Execute [test script](./scripts/run-tests.ps1) to run full suite.

## Best Practices
- Write tests before implementation (TDD)
- Test behavior, not implementation
- Keep tests focused and isolated
- Use meaningful test names
- Mock external dependencies
- Aim for high coverage on critical paths

## References
- XUnit: https://xunit.net/
- Vitest: https://vitest.dev/
- Testing Library: https://testing-library.com/
