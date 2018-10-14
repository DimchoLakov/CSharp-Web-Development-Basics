using System;

namespace IRunes.Models
{
    public class AlbumTrack : BaseModel<string>
    {
        public AlbumTrack()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public string TrackId { get; set; }
        public virtual Track Track { get; set; }
    }
}
