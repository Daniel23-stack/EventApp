# Contributing to EvenApp Inventory Management System

Thank you for your interest in contributing to EvenApp! This document provides guidelines and instructions for contributing.

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/your-username/EventApp.git`
3. Create a feature branch: `git checkout -b feature/your-feature-name`
4. Make your changes
5. Commit your changes: `git commit -m "Add: your feature description"`
6. Push to your fork: `git push origin feature/your-feature-name`
7. Open a Pull Request

## Development Setup

### Backend
```bash
cd EvenApp
dotnet restore
dotnet build
dotnet run --project src/EvenApp.API
```

### Frontend
```bash
cd frontend
npm install
npm run dev
```

## Code Style

### C# (.NET)
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and small
- Use async/await for I/O operations

### Vue.js
- Follow Vue.js style guide
- Use Composition API with `<script setup>`
- Keep components focused and reusable
- Use TypeScript for type safety (if applicable)

## Commit Messages

Use clear, descriptive commit messages:

- `Add: Feature description` - New features
- `Fix: Bug description` - Bug fixes
- `Update: Change description` - Updates to existing features
- `Refactor: Refactoring description` - Code refactoring
- `Docs: Documentation changes` - Documentation updates
- `Test: Test additions/changes` - Test-related changes

## Pull Request Process

1. Ensure your code follows the project's style guidelines
2. Update documentation if needed
3. Add tests for new features
4. Ensure all tests pass
5. Update the README if you've added new features
6. Request review from maintainers

## Testing

- Write unit tests for new features
- Ensure all existing tests pass
- Add integration tests for API endpoints
- Test on multiple browsers (for frontend changes)

## Questions?

Feel free to open an issue for any questions or clarifications.

