//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Artist.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblArtistAlias
    {
        public int idAlias { get; set; }
        public Nullable<System.Guid> Guid { get; set; }
        public string Alias { get; set; }
    
        public virtual tblArtist tblArtist { get; set; }
    }
}
