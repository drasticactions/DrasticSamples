using System;

namespace TvSample
{
    public class Song
    {
        public string ItemId { get; set; }
        public string Title { get; set; }
        public string StreamUrl { get; set; }
        public string Artist { get; set; }
        public string AlbumArt { get; set; }
        public string AlbumArtMedium { get; set; }
        public string AlbumArtLarge { get; set; }
        public int Duration { get; set; }
    }
}