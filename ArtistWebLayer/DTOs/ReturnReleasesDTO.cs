using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistWebLayer.DTOs
{
    public class ReturnReleasesDTO
    {
        public ICollection<ArtistReleasesDTO> releases { get; set; }
    }
}