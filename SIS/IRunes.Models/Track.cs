using System;
using System.Collections.Generic;

namespace IRunes.Models
{
    public class Track : BaseModel<string>
    {
        public Track()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TrackAlbums = new HashSet<AlbumTrack>();
        }

        public string Name { get; set; }

        // Link to video
        public string Link { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<AlbumTrack> TrackAlbums { get; set; }
    }
}
