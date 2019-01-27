using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artist.Domain;

namespace ArtistBusinessLayer
{
    public class ArtistReleasesBLL : IArtistReleasesBLL
    {
        #region Constants

        const int iDEFAULT_RELEASE_LIMIT = 0;

        #endregion

        #region Public methods

        /// <summary>
        /// Get releases by artist id.
        /// </summary>
        /// <param name="sGuid">The guid of the artist to search for.</param>
        /// <returns>List of releases.</returns>
        public List<ArtistReleaseDomain> GetReleasesByArtist(string sGuid)
        {
            return GetReleasesByArtist(sGuid, iDEFAULT_RELEASE_LIMIT);
        }

        /// <summary>
        /// Get first 10 albums by artist id.
        /// </summary>
        /// <param name="sGuid">The guid of the artist to search for.</param>
        /// <returns>List of albums.</returns>
        public List<ArtistReleaseDomain> GetReleasesByArtist(string sGuid, int iNumReleases)
        {
            return pGetReleasesFromMusicBrainz(sGuid, iNumReleases);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get N number of releases for the specified artist from MusicBrainz.
        /// </summary>
        /// <param name="sArtistGuid">Get the releases for this specific artist ID.</param>
        /// <param name="iNumberOfReleases">Number of releases to limit retrieval to.</param>
        /// <returns>List of DTO containing release details.</returns>
        private List<ArtistReleaseDomain> pGetReleasesFromMusicBrainz(string sArtistGuid, int iNumberOfReleases)
        {
            // From musicBrainz API, get all the releases for the artist id provided.
            MusicBrainz.Data.Release objRelease;
            if (iNumberOfReleases == 0)
                objRelease = MusicBrainz.Search.Release(arid: sArtistGuid);
            else
                objRelease = MusicBrainz.Search.Release(arid: sArtistGuid, limit: iNumberOfReleases);

            List<ArtistReleaseDomain> lstRelease = new List<ArtistReleaseDomain>();

            if (objRelease != null)
            {
                // Fill up a list of the releaseDTO which contains all the required release information.
                foreach (MusicBrainz.Data.ReleaseData objMusicBrainzRelease in objRelease.Data)
                {
                    lstRelease.Add(new ArtistReleaseDomain()
                    {
                        releaseID = objMusicBrainzRelease.Id,
                        title = objMusicBrainzRelease.Title,
                        status = objMusicBrainzRelease.Status,
                        label = objMusicBrainzRelease.Labelinfolist.Count > 0 ?
                                objMusicBrainzRelease.Labelinfolist.First().Label.Name : null,
                        numberOfTracks = objMusicBrainzRelease.Mediumlist.Trackcount,
                        otherArtists = objMusicBrainzRelease.Artistcredit
                                         .Select(r => new ArtistReleaseDomain.clsArtist()
                                         { id = r.Artist.Id, name = r.Artist.Name }).ToList()

                    });
                }
            }

            return lstRelease;
        }

        #endregion

    }
}
