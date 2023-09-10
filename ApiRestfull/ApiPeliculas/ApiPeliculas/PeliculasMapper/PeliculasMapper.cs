using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
            CreateMap <Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap <Pelicula, CrearPeliculaDTO>().ReverseMap();
            CreateMap <Pelicula, ActualizaPeliculaDTO>().ReverseMap();
        }
    }
}
