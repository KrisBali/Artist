using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistWebLayer.DTOs
{
    public class ArtistDTO
    {

        public string name { get; set; }

        public string country { get; set; }

        public string[] artistAliases { get; set; }

        public string[] linkToArtistAlbums { get; set; }
    }
}