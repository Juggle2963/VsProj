using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Web.Services;

public class PlayerState
{
    public MusicFile? CurrentTrack { get; private set; }
    public bool IsPlaying { get; set; }
    public double CurrentTime { get; set; }
    public double Duration { get; set; }
    public double Volume { get; set; } = 0.7;
    public List<MusicFile> Queue { get; set; } = new();
    public int QueueIndex { get; set; } = -1;

    public event Action? OnChange;

    public void SetTrack(MusicFile track)
    {
        CurrentTrack = track;
        CurrentTime = 0;
        Duration = 0;
        IsPlaying = true;
        NotifyStateChanged();
    }

    public void PlayQueue(List<MusicFile> queue, int startIndex)
    {
        Queue = new List<MusicFile>(queue);
        QueueIndex = startIndex;
        if (startIndex >= 0 && startIndex < queue.Count)
        {
            SetTrack(queue[startIndex]);
        }
    }

    public MusicFile? NextTrack()
    {
        if (Queue.Count == 0) return null;
        QueueIndex++;
        if (QueueIndex >= Queue.Count) 
        {
            QueueIndex = 0; // Loop
        }
        var track = Queue[QueueIndex];
        SetTrack(track);
        return track;
    }

    public MusicFile? PreviousTrack()
    {
        if (Queue.Count == 0) return null;
        QueueIndex--;
        if (QueueIndex < 0)
        {
            QueueIndex = Queue.Count - 1; // Loop
        }
        var track = Queue[QueueIndex];
        SetTrack(track);
        return track;
    }

    public void NotifyStateChanged() => OnChange?.Invoke();
}
