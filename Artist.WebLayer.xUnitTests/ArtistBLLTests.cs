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
    /// <summary>
    /// Artist BLL test class.
    /// </summary>
    [Collection("AssemblyInit")] //To initialize the automapper for static environment.
    public class ArtistBLLTests : IDisposable
    {
        #region Variables

        ArtistBLL objArtist;
        List<ArtistDomainModel> lstArtDom;
        paginationDomain pagedList;

        #endregion

        #region Constructor     

        /// <summary>
        /// Cosntructor.
        /// </summary>
        public ArtistBLLTests() { objArtist = new ArtistBLL(); }

        public void Dispose() { }

        #endregion

        /// <summary>
        /// Artist with names who don't exist.
        /// </summary>
        /// <param name="name">Name of artist.</param>
        [Theory]
        [InlineData("samuel etoo")] // artist does not exist
        [InlineData(" zak toto")] // artist does not exist
        [InlineData(" 123 & * %")] // artist does not exist
        public void GetArtists_ArtistNotFound(string name)
        {
            objArtist.GetArtists(name, 1, 2, out lstArtDom, out pagedList);

            Assert.True(lstArtDom.Count() == 0);
        }

        /// <summary>
        /// Artists who exists.
        /// </summary>
        /// <param name="name">Name of artist.</param>
        [Theory]
        [InlineData("Metallica")]
        [InlineData("Lady Gaga")]
        [InlineData("Mumford & Sons")]
        public void GetArtists_ArtistFound(string name)
        {
            objArtist.GetArtists(name, 1, 2, out lstArtDom, out pagedList);

            Assert.True(lstArtDom.Count() > 0);
        }

        /// <summary>
        /// Search for artist with only alias beginning with search name.
        /// </summary>
        /// <param name="alias">Alias to search for.</param>
        [Theory]
        [InlineData("reg")] // Reginald Kenneth Dwight => alias for "Elton John".
        [InlineData(" reg")]
        [InlineData(" REG")]
        [InlineData("REG")]
        [InlineData("Reginald Kenneth Dwight")]
        public void GetArtists_AliasInNameSearch(string alias)
        {
            objArtist.GetArtists(alias, 1, 2, out lstArtDom, out pagedList);

            Assert.Contains(lstArtDom.First().name, "Elton John");
        }

        /// <summary>
        /// Artist having different characters in name.
        /// </summary>
        /// <param name="name">Name of artist to look for.</param>
        [Theory]
        [InlineData("메탈리카")] // corresponds to Metallica.
        [InlineData(" 메탈리카 ")] // corresponds to Metallica.
        public void GetArtists_ArtistSpecialCharacters(string name)
        {
            const string associatedArtist = "Metallica";

            objArtist.GetArtists(name, 1, 2, out lstArtDom, out pagedList);

            Assert.Contains(lstArtDom.First().name, associatedArtist);
        }

        /// <summary>
        /// Test valid pagination returns with known records returned.
        /// </summary>      
        [Theory]
        [InlineData("j", 1, 1)] // j returns 6 results
        [InlineData("j", 1, 2)]
        [InlineData("j", 1, 3)]
        [InlineData("j", 1, 4)]
        [InlineData("j", 1, 5)]
        [InlineData("j", 1, 6)]
        [InlineData("j", 2, 2)]
        [InlineData("j", 3, 2)]
        [InlineData("j", 2, 3)]
        [InlineData("j", 2, 4)]
        public void GetArtists_ValidPagination(string name, int pageNumber, int pageSize)
        {
            const int iKNOWN_RECORDS_RETURNED = 6;

            objArtist.GetArtists(name, pageNumber, pageSize, out lstArtDom, out pagedList);
            // Page 2, page size 4.                                  

            Assert.True(pagedList.numberOfSearchResults == iKNOWN_RECORDS_RETURNED);
            Assert.True(pagedList.numberOfPages ==
                (int)Math.Ceiling(pagedList.numberOfSearchResults / (double)pageSize));
            Assert.True(pagedList.page == pageNumber);
            Assert.True(pagedList.pagesize == pageSize);
        }


        /// <summary>
        /// Test invalid pagination returns with known records returned.
        /// </summary>      
        [Theory]
        [InlineData("j", 3, 3)] // j returns 6 results => page num 3 > total pages 2     
        [InlineData("j", 4, 3)]
        [InlineData("j", 10, 3)]
        [InlineData("j", 2, 6)]
        public void GetArtists_InValidPagination_PageNumGreaterThanTotalPages(string name, int pageNumber, int pageSize)
        {
            const int iKNOWN_RECORDS_RETURNED = 6;

            objArtist.GetArtists(name, pageNumber, pageSize, out lstArtDom, out pagedList);
            // Page 2, page size 4.                                  

            Assert.True(pagedList.numberOfSearchResults == iKNOWN_RECORDS_RETURNED);
            Assert.True(pagedList.numberOfPages ==
                (int)Math.Ceiling(pagedList.numberOfSearchResults / (double)pageSize));
            Assert.True(pagedList.page == pagedList.numberOfPages); // PAge number = num pages.
            Assert.True(pagedList.pagesize == pageSize);
        }


    }

}

