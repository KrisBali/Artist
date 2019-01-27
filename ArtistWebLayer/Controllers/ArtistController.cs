using Artist.Domain;
using ArtistBusinessLayer;
using ArtistWebLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ArtistWebLayer.Controllers
{
    /// <summary>
    /// Artist controller class. 
    /// </summary>
    /// <remarks></remarks>
    [RoutePrefix("Artist")]
    public class ArtistController : ApiController
    {
        #region Repository patterns for DI.

        IArtistBLL _ArtistBusiness;
        IArtistReleasesBLL _ArtistReleases;

        /// <summary>
        /// Constructor for artist controller.
        /// </summary>
        /// <param name="ArtistBusiness">Artist business instance.</param>
        /// <param name="ArtistReleases">Artist release instance.</param>
        public ArtistController(IArtistBLL ArtistBusiness, IArtistReleasesBLL ArtistReleases)
        {
            _ArtistBusiness = ArtistBusiness;
            _ArtistReleases = ArtistReleases;
        }

        #endregion

        #region Constants

        private const int iNUMBER_OF_RELEASES_BY_ALBUM = 10; // get only first 10 records.
        private const string sREVIEW_GUID_RELEASES_MSG = "Please review the provided artist id for the releases search";
        private const string sREVIEW_GUID_ALBUMS_MSG = "Please review the provided artist id for the albums search";
        private const string sERROR_SERVER = "Error occurred on the server while processing your request. Please try again later.";

        #endregion

        #region Public Methods End point: /artist/search/<search_criteria>/<page_number>/<page_size>

        /// <summary>
        /// Endpoint 1: Artist/{name}/{pagenumber}/{pagesize}.
        /// Search for the artists starting with given name or alias and do pagination of the results.
        /// </summary>
        /// <param name="name">name of artist to search for in name and alias names.</param>
        /// <param name="page">page number to position to.</param>
        /// <param name="pagesize">number of results displayed on one page.</param>
        /// <returns>
        /// Ok => results
        /// NotFound => no results found for the search.
        /// BadRequest => parameters incorrect / server calculation error.
        /// </returns>
        /// <remarks> Endpoint 1: Artist/{name}/{pagenumber}/{pagesize}. </remarks>
        [HttpGet]
        [Route("search/{name}/{page}/{pagesize}")]
        public IHttpActionResult Search(string name, int page, int pagesize)
        {
            paginationDomain pagination = null;
            List<ArtistDomainModel> lstArtDomainMod;
            List<ArtistDTO> lstArtistDTO = new List<ArtistDTO>();
            string sErrorParameters;

            try
            {
                // First level check of parameters. If not OK, return.
                if (!pVerifyParamsOK(name, page, pagesize, out sErrorParameters)) return BadRequest(sErrorParameters);

                // Search for the artists meeting criteria. Return the artists found in a list + the pagination.
                _ArtistBusiness.GetArtists(name, page, pagesize, out lstArtDomainMod, out pagination);
                if (lstArtDomainMod.Count == 0) return NotFound();

                // Fill the artists releases from the music Brainz API. Done here with the smaller paginated list.
                FillArtistsLinkToAlbums(ref lstArtDomainMod);

                // Convert the results from artist domain to artist DTO for display. 
                AutoMapper.Mapper.Map(lstArtDomainMod, lstArtistDTO);

            }
            catch (Exception Ex)
            {
                return BadRequest(sERROR_SERVER + Environment.NewLine + Ex.ToString());
            }

            // Return OK with artist information + pagination information in the body.
            return Ok(new SearchArtistResultDTO()
            {
                results = lstArtistDTO,
                numberOfSearchResults = pagination.numberOfSearchResults,
                page = pagination.page,
                pageSize = pagination.pagesize,
                numberOfPages = pagination.numberOfPages
            });
        }

        /// <summary>
        /// Endpoint 2: artist/<artist_id>/releases
        /// Get all releases of the specified artist.
        /// </summary>
        /// <param name="sGuid">guid entered.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{sGuid}/releases")]
        public IHttpActionResult GetAllReleasesArtist(string sGuid)
        {
            // Check if GUID entered is valid.
            if (!isGuidValid(sGuid)) return BadRequest(sREVIEW_GUID_RELEASES_MSG);

            // Retrieve all the releases of the Artist.
            List<ArtistReleaseDomain> lstReleases = _ArtistReleases.GetReleasesByArtist(sGuid);
            List<ArtistReleasesDTO> lstReleasesDTO = new List<ArtistReleasesDTO>();

            // Map the domain onto the DTO.
            AutoMapper.Mapper.Map(lstReleases, lstReleasesDTO);

            // Check if at least a release found for the artist.
            if (lstReleasesDTO.Count > 0)
                return Ok(new ReturnReleasesDTO { releases = lstReleasesDTO });
            else
                return NotFound();
        }

        /// <summary>
        /// Endpoint 3: artist/<artist_id>/albums
        /// Search 10 first albums (order returned by api). 
        /// </summary>
        /// <param name="sGuid">guid entered.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{sGuid}/albums")]
        public IHttpActionResult GetNFirstAlbumsArtist(string sGuid)
        {
            // Check if GUID entered is valid.
            if (!isGuidValid(sGuid)) return BadRequest(sREVIEW_GUID_ALBUMS_MSG);

            // Retrieve the N specified albums of the artist Guid specified.
            List<ArtistReleaseDomain> lstReleases = _ArtistReleases.GetReleasesByArtist(sGuid, iNUMBER_OF_RELEASES_BY_ALBUM);
            List<ArtistReleasesDTO> lstReleasesDTO = new List<ArtistReleasesDTO>();

            // Map the domain onto the DTO.
             AutoMapper.Mapper.Map(lstReleases, lstReleasesDTO);

            // Check if at least a release found for the artist
            if (lstReleases.Count > 0)
                return Ok(new ReturnReleasesDTO{ releases = lstReleasesDTO });
            else
                return NotFound();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Complete the artist's link to the album releases. Added distinct album names for readability.
        /// </summary>
        /// <param name="lstArtDomainMod"></param>
        private void FillArtistsLinkToAlbums(ref List<ArtistDomainModel> lstArtDomainMod)
        {
            foreach (ArtistDomainModel objArtistDomain in lstArtDomainMod)
                objArtistDomain.linkToArtistAlbums = _ArtistReleases.GetReleasesByArtist(objArtistDomain.Guid.ToString());
        }

        /// <summary>
        /// Is a valid guid entered? if not, no need to continue.
        /// </summary>
        /// <param name="sGuid">guid entered.</param>
        /// <returns></returns>
        private bool isGuidValid(string sGuid)
        {
            System.Guid tempGuid;
            return Guid.TryParse(sGuid, out tempGuid);
        }

        /// <summary>
        /// Verify if the parameters entered are OK.
        /// </summary>
        /// <param name="sName">Name to search for.</param>
        /// <param name="pagenumber">Page on which to position the search.</param>
        /// <param name="pagesize">The page size.</param>
        /// <param name="sErrorMessage">Error messages returned depending on the case of error.</param>
        /// <returns>Error message to display back for incorrect parameter.</returns>
        private bool pVerifyParamsOK(string sName, int pagenumber, int pagesize, out string sErrorMessage)
        {
            sErrorMessage = null;

            // Check that string name is filled.
            if (string.IsNullOrEmpty(sName) || string.IsNullOrEmpty(sName.Trim()))
            {
                sErrorMessage = "Please review the name provided for the search." + sName;
                return false;
            }

            // Check that page number is not zero.
            else if (pagenumber == 0)
            {
                sErrorMessage = "Please enter a valid page number.";
                return false;
            }

            // Check that page size is not zero.
            else if (pagesize == 0)
            {
                sErrorMessage = "Please enter a valid page size.";
                return false;
            }

            return true;
        }

        #endregion

    }
}
