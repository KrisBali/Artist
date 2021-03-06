﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistWebLayer.DTOs
{
    public class ArtistReleasesDTO
    {
        public string releaseID { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string label { get; set; }
        public int numberOfTracks { get; set; }
        public List<clsArtist> otherArtists { get; set; }

        public class clsArtist
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}