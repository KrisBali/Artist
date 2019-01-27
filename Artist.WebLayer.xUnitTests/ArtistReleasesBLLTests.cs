using Artist.Domain;
using ArtistBusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitArtistTests
{
    [Collection("AssemblyInit")]
    public class ArtistReleasesBLLTest : IDisposable
    {
        #region Variables

        ArtistReleasesBLL objReleases;
        List<ArtistReleaseDomain> lstReleases;

        #endregion

        #region "Constructor & Dispose"

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArtistReleasesBLLTest() { objReleases = new ArtistReleasesBLL(); }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() { }

        #endregion

        /// <summary>
        /// Artist with less than 10 albums since we searching for 10 albums.
        /// </summary>
        /// <param name="sGuid">Guid of artist to search for.</param>
        [Theory]
        [InlineData("a9d625f3-af2e-4842-8303-9c1523d28711")] // Returns artist "kaya" having 2 releases only.
        public void GetAlbumsByArtist_ArtistLessThan10Albums(string sGuid)
        {
            lstReleases = objReleases.GetReleasesByArtist(sGuid, 10);
            Assert.True(lstReleases.Count() > 0 && lstReleases.Count < 10);
        }


        /// <summary>
        /// Search for artist with incorrect GUID.
        /// </summary>
        /// <param name="sGuid">Guid of artist to search for.</param>
        [Theory]
        [InlineData("d700b3f5-45fd-4d02-95ed-57d301bda93e")]
        [InlineData("144ef525-85e9-40c3-8335-02c32d086xxx")]
        public void GetReleasesByArtist_NoReleasesFound_IncorrectGuid(string sGuid)
        {
            lstReleases = objReleases.GetReleasesByArtist(sGuid);
            Assert.True(lstReleases.Count() == 0);
        }

        /// <summary>
        /// Artist with correct GUID but having no releases.
        /// </summary>
        /// <param name="sGuid">Guid of artist to search for.</param>
        [Theory]
        [InlineData("2cd03feb-f6be-4f27-ae25-184310829b39")] // Laura toti no releases.
        public void GetReleasesByArtist_NoReleasesFound_CorrectGuid(string sGuid)
        {
            lstReleases = objReleases.GetReleasesByArtist(sGuid);
            Assert.True(lstReleases.Count() == 0);
        }

        /// <summary>
        /// Artists with > 10 releases, check if only first 10 returned.
        /// </summary>
        /// <param name="sGuid">Guid of artist to search for.</param>
        [Theory]
        [InlineData("d700b3f5-45af-4d02-95ed-57d301bda93e")]
        [InlineData("144ef525-85e9-40c3-8335-02c32d0861f3")]
        public void GetAlbumsByArtist_onlyFirst10Returned(string sGuid)
        {
            lstReleases = objReleases.GetReleasesByArtist(sGuid, 10);
            Assert.True(lstReleases.Count() == 10);
        }

        /// <summary>
        /// Artists having releases to their names.
        /// </summary>
        /// <param name="sGuid">Guid of artist to search for.</param>
        [Theory]
        [InlineData("65f4f0c5-ef9e-490c-aee3-909e7ae6b2ab")]
        [InlineData("650e7db6-b795-4eb5-a702-5ea2fc46c848")]
        [InlineData("c44e9c22-ef82-4a77-9bcd-af6c958446d6")]
        [InlineData("435f1441-0f43-479d-92db-a506449a686b")]
        [InlineData("a9044915-8be3-4c7e-b11f-9e2d2ea0a91e")]
        [InlineData("b625448e-bf4a-41c3-a421-72ad46cdb831")]
        [InlineData("d700b3f5-45af-4d02-95ed-57d301bda93e")]
        [InlineData("144ef525-85e9-40c3-8335-02c32d0861f3")]
        public void GetReleasesByArtist_artistsHavingReleases(string sGuid)
        {
            lstReleases = objReleases.GetReleasesByArtist(sGuid);
            Assert.True(lstReleases.Count() > 1);
        }
    }

}

