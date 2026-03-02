using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Interfaces;

public interface IMusicScannerService
{
    Task<IEnumerable<MusicFile>> ScanDirectoryAsync(string directoryPath);
    Task<MusicFile?> ScanFileAsync(string filePath);
    Task<int> ScanAllDirectoriesAsync();
    Task<bool> IsSupportedMusicFileAsync(string filePath);
}