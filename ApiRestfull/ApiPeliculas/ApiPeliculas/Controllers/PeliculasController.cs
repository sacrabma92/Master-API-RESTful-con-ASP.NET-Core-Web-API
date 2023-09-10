using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;

        private readonly IMapper _maper;

        public PeliculasController(IPeliculaRepositorio pelRepo , IMapper maper)
        {
            _pelRepo = pelRepo;
            _maper = maper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();

            var listaPeliculasDto = new List<PeliculaDTO>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_maper.Map<PeliculaDTO>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        [HttpGet("{peliculaId:int}", Name ="GetPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);

            if(itemPelicula == null)
            {
                return NotFound();
            }

            return Ok(itemPelicula);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPostPelicula([FromBody] CrearPeliculaDTO crearPeliculaDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(crearPeliculaDTO == null)
            {
                return BadRequest(ModelState);
            }

            if(_pelRepo.ExistePelicula(crearPeliculaDTO.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _maper.Map<Pelicula>(crearPeliculaDTO);
            if(!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.PeliculaId }, pelicula);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] ActualizaPeliculaDTO peliculaDTO)
        {
            if (_pelRepo.ExistePelicula(peliculaDTO.Nombre))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _maper.Map<Pelicula>(peliculaDTO);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if(!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);

            if(!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpGet("GetPeliculasEnCategorias/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategorias(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

            if (listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPelicula = new List<PeliculaDTO>();

            foreach (var item in listaPeliculas)
            {
                itemPelicula.Add(_maper.Map<PeliculaDTO>(item));
            }
            return Ok(itemPelicula);
        }

        [HttpGet("Buscar")]
        public IActionResult GetPeliculasEnCategorias(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre.Trim());

                if(resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }

    }
}
