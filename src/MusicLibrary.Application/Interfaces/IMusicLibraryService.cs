using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Interfaces;

public interface IMusicLibraryService
{
    Task<IEnumerable<MusicFile>> GetAllMusicFilesAsync();
    Task<MusicFile?> GetMusicFileByIdAsync(int id);
    Task<IEnumerable<ScanDirectory>> GetScanDirectoriesAsync();
    Task<ScanDirectory> AddScanDirectoryAsync(string path, string name);
    Task RemoveScanDirectoryAsync(int id);
    Task<int> ScanAllDirectoriesAsync();
}