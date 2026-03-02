// Audio player interop for Blazor
window.audioPlayer = {
    audio: null,
    dotNetRef: null,
    updateInterval: null,

    init: function (dotNetRef) {
        this.dotNetRef = dotNetRef;
        if (!this.audio) {
            this.audio = new Audio();
            this.audio.volume = 0.7;

            this.audio.addEventListener('ended', () => {
                this.dotNetRef.invokeMethodAsync('OnTrackEnded');
            });

            this.audio.addEventListener('loadedmetadata', () => {
                this.dotNetRef.invokeMethodAsync('OnDurationChanged', this.audio.duration);
            });

            this.audio.addEventListener('error', (e) => {
                console.error('Audio error:', e);
                this.dotNetRef.invokeMethodAsync('OnPlaybackError', 'Failed to load audio file');
            });
        }
    },

    play: function (url) {
        if (!this.audio) return;
        this.audio.src = url;
        this.audio.play().catch(err => console.error('Play error:', err));
        this.startProgressUpdates();
    },

    resume: function () {
        if (!this.audio) return;
        this.audio.play().catch(err => console.error('Resume error:', err));
        this.startProgressUpdates();
    },

    pause: function () {
        if (!this.audio) return;
        this.audio.pause();
        this.stopProgressUpdates();
    },

    stop: function () {
        if (!this.audio) return;
        this.audio.pause();
        this.audio.currentTime = 0;
        this.stopProgressUpdates();
    },

    seek: function (time) {
        if (!this.audio) return;
        this.audio.currentTime = time;
    },

    setVolume: function (volume) {
        if (!this.audio) return;
        this.audio.volume = Math.max(0, Math.min(1, volume));
    },

    getCurrentTime: function () {
        return this.audio ? this.audio.currentTime : 0;
    },

    getDuration: function () {
        return this.audio ? this.audio.duration : 0;
    },

    startProgressUpdates: function () {
        this.stopProgressUpdates();
        this.updateInterval = setInterval(() => {
            if (this.audio && this.dotNetRef && !this.audio.paused) {
                this.dotNetRef.invokeMethodAsync('OnTimeUpdate', this.audio.currentTime);
            }
        }, 500);
    },

    stopProgressUpdates: function () {
        if (this.updateInterval) {
            clearInterval(this.updateInterval);
            this.updateInterval = null;
        }
    },

    dispose: function () {
        this.stopProgressUpdates();
        if (this.audio) {
            this.audio.pause();
            this.audio.src = '';
        }
        this.dotNetRef = null;
    }
};
