﻿namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public sealed class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            // Requests to queries or commands.

            // ReadModels to responses.
            this.CreateMap<MovieReadModel, MovieResponse>();

            this.CreateMap<PaginatedCollectionOutputDto<MovieReadModel>, IEnumerable<MovieResponse>>()
                .ConvertUsing((src, dest, context) => context.Mapper.Map<IEnumerable<MovieResponse>>(src.Items));

            this.CreateMap<Result<PaginatedCollectionOutputDto<MovieReadModel>>, Result<IEnumerable<MovieResponse>>>();
        }
    }
}