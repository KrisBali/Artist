using Artist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistBusinessLayer
{
    public interface IArtistBLL
    {
        void GetArtists(string artistName, int page, int pageSize,
            out List<ArtistDomainModel> lstArtDomain, out paginationDomain pageInfoDomain);

    }
}
