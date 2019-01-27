using Artist.Domain;
using ArtistWebLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistWebLayer.Infrastructure
{
    public class AutoMapperWebProfile : AutoMapper.Profile
    {
        public AutoMapperWebProfile()
        {
            CreateMap<ArtistDomainModel, ArtistDTO>()
                .ForMember(dest => dest.artistAliases,
                           opts => opts.MapFrom(
                               r => r.artistAliases
                               .Where(y => !string.IsNullOrEmpty(y.ToString().Trim()))
                               .Select(x => x.Alias.Trim()).ToArray()))
                 .ForMember(dest => dest.linkToArtistAlbums,
                            opts => opts.MapFrom(
                              r=> r.linkToArtistAlbums.Where(
                                  x => x.title != null && !string.IsNullOrEmpty(x.title.ToString().Trim()))
                                  .Select(z => z.title.Trim()).Distinct().ToArray()));


            CreateMap<ArtistReleaseDomain, ArtistReleasesDTO>();

        }

        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWebProfile>();
            }
            );
        }       

    }
}