using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artist.Domain
{
    public class ArtistDomainModel
    {
        public System.Guid Guid { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public virtual ICollection<ArtistReleaseDomain> linkToArtistAlbums { get; set; }

        public virtual ICollection<ArtistAliasDomainModel> artistAliases { get; set; }
        
    }

}
