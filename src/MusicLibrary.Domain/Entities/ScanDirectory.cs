namespace MusicLibrary.Domain.Entities;

public class ScanDirectory
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastScanDate { get; set; }
    
    // Navigation properties
    public ICollection<MusicFile> MusicFiles { get; set; } = new List<MusicFile>();
}