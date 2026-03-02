<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

This project is a music library application built with Blazor Server and follows the Onion Architecture pattern.

## Project Structure
- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Use cases and business logic
- **Infrastructure Layer**: Database access, file I/O, external services
- **Presentation Layer**: Blazor Server web application

## Technologies Used
- .NET 8.0
- Blazor Server
- Entity Framework Core
- SQL Server
- TagLibSharp for MP3 metadata extraction

## Key Features
- Scans directories for MP3 and music files
- Extracts metadata (title, artist, album, duration, etc.)
- Stores music information in SQL Server database
- Displays music collection in web interface
- Supports adding new scan directories

## Development Guidelines
- Follow SOLID principles and clean architecture
- Use dependency injection for all services
- Implement proper error handling and logging
- Keep UI components focused and reusable
- Use async/await for all I/O operations