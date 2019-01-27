using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artist.Domain
{
    public class paginationDomain
    {
        public int numberOfSearchResults { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
        public int numberOfPages { get; set; }      

    }

}
