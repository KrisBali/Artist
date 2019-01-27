using Artist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistBusinessLayer
{
    public interface IArtistReleasesBLL
    {
        List<ArtistReleaseDomain> GetReleasesByArtist(string sGuid);
        List<ArtistReleaseDomain> GetReleasesByArtist(string sGuid, int iNoReleases);

    }
}
