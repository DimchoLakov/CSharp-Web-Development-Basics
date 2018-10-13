using System;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.Models
{
    public class Album : BaseModel<string>
    {
        public Album()
        {
            this.Id = new Guid().ToString();
            this.AlbumTracks = new HashSet<AlbumTrack>();
        }

        public string Name { get; set; }

        // Link to image
        public string Cover { get; set; }

        public decimal Price => this.AlbumTracks.Select(at => at.Track.Price).DefaultIfEmpty(0).Sum() * 0.87m;
        
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<AlbumTrack> AlbumTracks { get; set; }
    }
}
