using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artist.Domain
{
    public class ArtistAliasDomainModel
    {
        public int idAlias { get; set; }
        public Nullable<System.Guid> Guid { get; set; }
        public string Alias { get; set; }
     
    }

}
