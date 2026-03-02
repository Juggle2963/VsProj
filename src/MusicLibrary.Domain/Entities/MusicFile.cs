namespace MusicLibrary.Domain.Entities;

public class MusicFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int? Track { get; set; }
    public TimeSpan? Duration { get; set; }
    public long FileSize { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime LastModified { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public int? Bitrate { get; set; }
    
    // Navigation properties
    public int? ScanDirectoryId { get; set; }
    public ScanDirectory? ScanDirectory { get; set; }
}