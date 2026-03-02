# Music Library Application

A Blazor Server web application for managing music collections with automatic metadata extraction and database storage. Built using Onion Architecture principles with .NET 7.

## Features

- 🎵 **Music File Scanning**: Automatically scans directories for music files (MP3, FLAC, WAV, M4A, AAC, OGG, WMA)
- 🎯 **Metadata Extraction**: Extracts ID3 tags and music metadata using TagLibSharp
- 🗄️ **Database Storage**: Stores music information in SQL Server database using Entity Framework Core
- 🌐 **Web Interface**: Clean Blazor Server UI for browsing your music collection
- 📁 **Directory Management**: Add and manage multiple scan directories
- 🏗️ **Clean Architecture**: Follows Onion Architecture pattern for maintainability

## Architecture

The project follows the **Onion Architecture** pattern with clear separation of concerns:

### Layers

- **🎯 Domain Layer** (`MusicLibrary.Domain`): Core business entities and interfaces
  - Entities: `MusicFile`, `ScanDirectory`
  - Interfaces: `IMusicFileRepository`, `IScanDirectoryRepository`

- **📋 Application Layer** (`MusicLibrary.Application`): Use cases and business logic
  - Services: `MusicLibraryService`
  - Interfaces: `IMusicLibraryService`, `IMusicScannerService`

- **🔧 Infrastructure Layer** (`MusicLibrary.Infrastructure`): Database access and external services
  - DbContext: `MusicLibraryDbContext`
  - Repositories: `MusicFileRepository`, `ScanDirectoryRepository`
  - Services: `MusicScannerService`

- **🌐 Presentation Layer** (`MusicLibrary.Web`): Blazor Server web application
  - Pages: `Index.razor`, `MusicLibrary.razor`
  - Shared components and layouts

## Technologies Used

- **.NET 7.0**: Framework and runtime
- **Blazor Server**: Web framework for interactive UI
- **Entity Framework Core 7**: Object-relational mapping
- **SQL Server**: Database (using LocalDB for development)
- **TagLibSharp**: Music metadata extraction library
- **Bootstrap**: CSS framework for responsive design

## Getting Started

### Prerequisites

- .NET 7.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio Code (recommended extensions: C#, C# Dev Kit)

### Setup Instructions

1. **Clone and navigate to the project**:
   ```bash
   cd VsProj
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Update database**:
   ```bash
   dotnet ef database update --project src/MusicLibrary.Infrastructure --startup-project src/MusicLibrary.Web
   ```

4. **Run the application**:
   ```bash
   dotnet run --project src/MusicLibrary.Web
   ```

5. **Access the application**:
   Open your browser and navigate to `http://localhost:5124`

## Usage

### Adding Music Directories

1. Navigate to the **Music Library** page
2. Click **"Add Scan Directory"**
3. Enter a name and the full path to your music folder
4. Click **"Add Directory"**

### Scanning for Music

1. Click **"Scan All Directories"** to search for music files
2. The application will:
   - Recursively scan all configured directories
   - Extract metadata from supported music files
   - Store information in the database
   - Display results in the web interface

### Supported File Formats

- MP3 (.mp3)
- FLAC (.flac)
- WAV (.wav)
- M4A (.m4a)
- AAC (.aac)
- OGG (.ogg)
- WMA (.wma)

## Database Schema

### MusicFiles Table
- **Id**: Primary key
- **FileName**, **FilePath**: File information
- **Title**, **Artist**, **Album**, **Genre**: Metadata
- **Year**, **Track**: Numeric metadata
- **Duration**: Track length
- **FileSize**, **Bitrate**: Technical info
- **DateAdded**, **LastModified**: Timestamps
- **ScanDirectoryId**: Foreign key to scan directory

### ScanDirectories Table
- **Id**: Primary key
- **Path**, **Name**: Directory information
- **IsActive**: Enable/disable scanning
- **CreatedDate**, **LastScanDate**: Timestamps

## Development

### Project Structure
```
├── src/
│   ├── MusicLibrary.Domain/          # Core business entities
│   ├── MusicLibrary.Application/     # Business logic and use cases
│   ├── MusicLibrary.Infrastructure/  # Data access and external services
│   └── MusicLibrary.Web/            # Blazor Server web application
├── MusicLibrary.sln                  # Solution file
└── README.md                         # This file
```

### Key Design Patterns

- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Service registration and resolution
- **Clean Architecture**: Separation of concerns across layers
- **CQRS-like**: Separate interfaces for different operations

## Configuration

The application uses `appsettings.json` for configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MusicLibraryDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes following the existing architecture patterns
4. Add appropriate tests
5. Submit a pull request

## License

This project is created as a demonstration of clean architecture principles with Blazor and Entity Framework Core.