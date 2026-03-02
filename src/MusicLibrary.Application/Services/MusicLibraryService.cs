using MusicLibrary.Application.Interfaces;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Interfaces;

namespace MusicLibrary.Application.Services;

public class MusicLibraryService : IMusicLibraryService
{
    private readonly IMusicFileRepository _musicFileRepository;
    private readonly IScanDirectoryRepository _scanDirectoryRepository;
    private readonly IMusicScannerService _musicScannerService;

    public MusicLibraryService(
        IMusicFileRepository musicFileRepository,
        IScanDirectoryRepository scanDirectoryRepository,
        IMusicScannerService musicScannerService)
    {
        _musicFileRepository = musicFileRepository;
        _scanDirectoryRepository = scanDirectoryRepository;
        _musicScannerService = musicScannerService;
    }

    public async Task<IEnumerable<MusicFile>> GetAllMusicFilesAsync()
    {
        return await _musicFileRepository.GetAllAsync();
    }

    public async Task<MusicFile?> GetMusicFileByIdAsync(int id)
    {
        return await _musicFileRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ScanDirectory>> GetScanDirectoriesAsync()
    {
        return await _scanDirectoryRepository.GetAllAsync();
    }

    public async Task<ScanDirectory> AddScanDirectoryAsync(string path, string name)
    {
        if (await _scanDirectoryRepository.ExistsAsync(path))
            throw new ArgumentException("Directory already exists in the scan list.");

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"Directory '{path}' does not exist.");

        var directory = new ScanDirectory
        {
            Path = path,
            Name = name,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        return await _scanDirectoryRepository.AddAsync(directory);
    }

    public async Task RemoveScanDirectoryAsync(int id)
    {
        await _scanDirectoryRepository.DeleteAsync(id);
    }

    public async Task<int> ScanAllDirectoriesAsync()
    {
        return await _musicScannerService.ScanAllDirectoriesAsync();
    }
}