using Artist.Domain;
using ArtistWebLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artist.WebLayer.xUnitTests
{

    public class AssemblyIntialize : IDisposable

    {
        public AssemblyIntialize()
        {

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ArtistDomainModel, ArtistDTO>()
                        .ForMember(dest => dest.artistAliases,
                                   opts => opts.MapFrom(
                                       r => r.artistAliases
                                       .Where(y => !string.IsNullOrEmpty(y.ToString().Trim()))
                                       .Select(x => x.Alias.Trim()).ToArray()))
                         .ForMember(dest => dest.linkToArtistAlbums,
                                    opts => opts.MapFrom(
                                      r => r.linkToArtistAlbums.Where(
                                          x => x.title != null && !string.IsNullOrEmpty(x.title.ToString().Trim()))
                                          .Select(z => z.title.Trim()).Distinct().ToArray()));


                cfg.CreateMap<ArtistReleaseDomain, ArtistReleasesDTO>();
            });

        }

        public void Dispose()
        {
            Mapper.Reset(); ;
        }
    }
}
