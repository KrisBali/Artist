using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artist.Data;
using Artist.Domain;
using AutoMapper;

namespace ArtistBusinessLayer
{
    /// <summary>
    /// Business Logic for artist search.
    /// </summary>
    public class ArtistBLL : IArtistBLL
    {
        #region Public Methods
        /// <summary>
        /// Get the artists according to the specified artist name and paging information.
        /// </summary>
        /// <param name="artistName">The artist name to search for.</param>
        /// <param name="page">The page number to display.</param>
        /// <param name="pageSize">The size of each pages.</param>
        /// <param name="lstArtistDomain">List containing the domain information of the artists.</param>
        /// <param name="pageDomain">Object containing the paging details to be displayed.</param>
        public void GetArtists(string artistName, int page, int pageSize,
                                       out List<ArtistDomainModel> lstArtistDomain,
                                       out paginationDomain pageDomain)
        {

            using (ArtistDBEntities dbContext = new ArtistDBEntities())
            {
                // Search for artists having the supplied name starting in their names or aliases. (deferred execution)               
                IOrderedQueryable<tblArtist> iqArtists = FindArtistsByName(dbContext, artistName);

                PagingListClass<tblArtist> pagedList;

                // Create a paged list based on the page number and page size. (executed here)
                pagedList = new PagingListClass<tblArtist>(iqArtists, page, pageSize);

                // Fill the artist domain with the paged list details. Bring all the info, in case we need it in DTO someday.
                lstArtistDomain = pagedList.Select(r => new ArtistDomainModel()
                {
                    name = r.Name,
                    country = r.Country,
                    artistAliases = r.tblArtistAliases
                                   .Select(x => new ArtistAliasDomainModel()
                                   {
                                       Alias = x.Alias,
                                       idAlias = x.idAlias,
                                       Guid = x.Guid
                                   }).ToList(),
                    Guid = r.Guid,
                }).ToList();

                // Fill the pagination information domain.
                pageDomain = new paginationDomain()
                {
                    numberOfPages = pagedList.TotalPages,
                    page = pagedList.PageNumber,
                    pagesize = pagedList.PageSize,
                    numberOfSearchResults = pagedList.TotalNumSearchResults
                };
            }
        }


        #endregion

        #region Private methods

        /// <summary>
        /// Find the artist in the entity according to the specified artist name.
        /// </summary>
        /// <param name="objEntities">Entity data.</param>
        /// <param name="artistName">Artist name to search for in artist name and alias.</param>
        /// <returns>Deferred execution list of tblArtist which matches the criteria.</returns>
        private IOrderedQueryable<tblArtist> FindArtistsByName(ArtistDBEntities objEntities, string artistName)
        {
            // Name starts with Name of artist or starts as one of aliases of the artist.
            return objEntities.tblArtists
                                .AsQueryable().Where(r => r.Name.Trim().ToUpper().StartsWith(artistName.Trim().ToUpper())
                               ||
                               (
                               r.tblArtistAliases.Any(objAlias => objAlias.Alias.Trim().ToUpper().StartsWith(artistName.Trim().ToUpper())))
                               ).OrderBy(r => r.Name);
        }


        #endregion               
    }
}
