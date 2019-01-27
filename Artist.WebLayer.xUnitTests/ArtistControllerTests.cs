using Artist.Domain;
using ArtistBusinessLayer;
using ArtistWebLayer.Controllers;
using ArtistWebLayer.DTOs;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Xunit;

namespace xUnitArtistTests
{
    /// <summary>
    /// Test class for artist controller class.
    /// </summary>
    [Collection("AssemblyInit")]
    public class ArtistControllerTests : IDisposable
    {
        #region Constructor & Dispose

        ArtistController objArtistController;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArtistControllerTests() { objArtistController = controllerInstance(); }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (objArtistController != null)
            {
                objArtistController.Dispose();
                objArtistController = null;
            }
        }

        #endregion

        #region Endpoint 1: /artist/search/<search_criteria>/<page_number>/<page_size>

        /// <summary>
        /// Test the catch for incorrect parameters to the api.
        /// </summary>
        /// <param name="name">name of artist.</param>
        /// <param name="page">page number.</param>
        /// <param name="pageSize">page size.</param>
        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("", 0, 1)]
        [InlineData("", 1, 0)]
        [InlineData("", 1, 1)]
        [InlineData("John Mayer", 0, 0)]
        [InlineData("John Mayer", 0, 1)]
        [InlineData("John Mayer", 1, 0)]
        public void ArtistSearchTest_IncorrectParameters(string name, int page, int pageSize)
        {

            // Act
            var result = objArtistController.Search(name, page, pageSize);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);

        }

        /// <summary>
        /// Test the incorrect parameters.
        /// </summary>
        /// <param name = "name" > name of artist.</param>
        /// <param name = "page" > page number.</param>
        /// <param name = "pageSize" > page size.</param>
        [Theory]
        [InlineData("J", 1, 4)]
        [InlineData("J", 2, 2)]
        [InlineData(" Joh", 2, 5)]
        public void ArtistSearchTest_CorrectParameters(string name, int page, int pageSize)
        {
            //  Act
            IHttpActionResult result = objArtistController.Search(name, page, pageSize);
            var response = result as OkNegotiatedContentResult<SearchArtistResultDTO>;

            Assert.NotNull(response);
        }


        #endregion

        #region "Endpoint 2 : /artist/<artist_id>/releases

        /// <summary>
        /// Test for incorrect guid when searching for releases.
        /// </summary>
        /// <param name="sGuid">guid entered.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("0")]
        [InlineData("asdfasdf")]
        [InlineData("65f4f0c5-ef9e-490c-aee3-909e7ae6bxxx")]
        public void ArtistGetReleases_IncorrectGuid(string sGuid)
        {
            // Act
            var result = objArtistController.GetAllReleasesArtist(sGuid);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        /// <summary>
        /// Test for correct GUID when searching for all the releases.
        /// </summary>
        /// <param name="sGuid">Guid of artist to look for.</param>
        [Theory]
        [InlineData("65f4f0c5-ef9e-490c-aee3-909e7ae6b2ab")]
        public void ArtistGetReleases_ArtistExist(string sGuid)
        {
            // Act
            IHttpActionResult result = objArtistController.GetAllReleasesArtist(sGuid);
            var response = result as OkNegotiatedContentResult<ReturnReleasesDTO>;

            // Assert   
            Assert.NotNull(response);
        }


        #endregion

        #region "Endpoint 3 : /artist/<artist_id>/albums


        /// <summary>
        /// Test for incorrect guid when searching for albums.
        /// </summary>
        /// <param name="sGuid">guid entered.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("0")]
        [InlineData("asdfasdf")]
        [InlineData("65f4f0c5-ef9e-490c-aee3-909e7ae6bxxx")]
        public void ArtistGetAlbums_InCorrectGuid(string sGuid)
        {

            // Act
            var result = objArtistController.GetNFirstAlbumsArtist(sGuid);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Real controller instance used.
        /// </summary>
        /// <returns></returns>
        private ArtistController controllerInstance()
        {
            var objArtist = new ArtistBusinessLayer.ArtistBLL();
            var objReleases = new ArtistReleasesBLL();

            return new ArtistController(objArtist, objReleases);
        }

        #endregion
    }

}