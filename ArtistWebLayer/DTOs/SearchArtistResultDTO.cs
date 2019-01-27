using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistWebLayer.DTOs
{
    public class SearchArtistResultDTO
    {
        public List<ArtistDTO> results { get; set; }
        public int numberOfSearchResults { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int numberOfPages { get; set; }

    }
}