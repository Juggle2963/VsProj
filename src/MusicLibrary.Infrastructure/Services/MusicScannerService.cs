using Microsoft.Extensions.Logging;
using MusicLibrary.Application.Interfaces;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Interfaces;
using TagLib;
using File = TagLib.File;

namespace MusicLibrary.Infrastructure.Services;

public class MusicScannerService : IMusicScannerService
{
    private readonly IMusicFileRepository _musicFileRepository;
    private readonly IScanDirectoryRepository _scanDirectoryRepository;
    private readonly ILogger<MusicScannerService> _logger;
    
    private static readonly string[] SupportedExtensions = { ".mp3", ".flac", ".wav", ".m4a", ".aac", ".ogg", ".wma" };

    public MusicScannerService(
        IMusicFileRepository musicFileRepository,
        IScanDirectoryRepository scanDirectoryRepository,
        ILogger<MusicScannerService> logger)
    {
        _musicFileRepository = musicFileRepository;
        _scanDirectoryRepository = scanDirectoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MusicFile>> ScanDirectoryAsync(string directoryPath)
    {
        var musicFiles = new List<MusicFile>();
        
        if (!Directory.Exists(directoryPath))
        {
            _logger.LogWarning("Directory does not exist: {DirectoryPath}", directoryPath);
            return musicFiles;
        }

        try
        {
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(file => IsSupportedMusicFile(file));

            foreach (var filePath in files)
            {
                try
                {
                    var musicFile = await ScanFileAsync(filePath);
                    if (musicFile != null)
                    {
                        musicFiles.Add(musicFile);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning file: {FilePath}", filePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning directory: {DirectoryPath}", directoryPath);
        }

        return musicFiles;
    }

    public async Task<MusicFile?> ScanFileAsync(string filePath)
    {
        if (!System.IO.File.Exists(filePath) || !IsSupportedMusicFile(filePath))
        {
            return null;
        }

        // Check if file already exists in database
        if (await _musicFileRepository.ExistsAsync(filePath))
        {
            return null;
        }

        try
        {
            var fileInfo = new FileInfo(filePath);
            using var tagFile = File.Create(filePath);

            var musicFile = new MusicFile
            {
                FileName = fileInfo.Name,
                FilePath = filePath,
                FileSize = fileInfo.Length,
                DateAdded = DateTime.UtcNow,
                LastModified = fileInfo.LastWriteTime,
                FileExtension = fileInfo.Extension.ToLowerInvariant(),
                Title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath),
                Artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist",
                Album = tagFile.Tag.Album ?? "Unknown Album",
                Genre = tagFile.Tag.FirstGenre ?? "Unknown",
                Year = (int?)tagFile.Tag.Year,
                Track = (int?)tagFile.Tag.Track,
                Duration = tagFile.Properties.Duration,
                Bitrate = tagFile.Properties.AudioBitrate
            };

            return await _musicFileRepository.AddAsync(musicFile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
            return null;
        }
    }

    public async Task<int> ScanAllDirectoriesAsync()
    {
        var directories = await _scanDirectoryRepository.GetAllAsync();
        var totalFilesScanned = 0;

        foreach (var directory in directories.Where(d => d.IsActive))
        {
            try
            {
                _logger.LogInformation("Scanning directory: {DirectoryPath}", directory.Path);
                
                var musicFiles = await ScanDirectoryAsync(directory.Path);
                
                // Update scan directory relationship
                foreach (var musicFile in musicFiles)
                {
                    musicFile.ScanDirectoryId = directory.Id;
                    await _musicFileRepository.UpdateAsync(musicFile);
                }
                
                totalFilesScanned += musicFiles.Count();
                
                // Update last scan date
                directory.LastScanDate = DateTime.UtcNow;
                await _scanDirectoryRepository.UpdateAsync(directory);
                
                _logger.LogInformation("Scanned {FileCount} files from {DirectoryPath}", 
                    musicFiles.Count(), directory.Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning directory: {DirectoryPath}", directory.Path);
            }
        }

        _logger.LogInformation("Total files scanned: {TotalFiles}", totalFilesScanned);
        return totalFilesScanned;
    }

    public Task<bool> IsSupportedMusicFileAsync(string filePath)
    {
        return Task.FromResult(IsSupportedMusicFile(filePath));
    }

    private static bool IsSupportedMusicFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return SupportedExtensions.Contains(extension);
    }
}