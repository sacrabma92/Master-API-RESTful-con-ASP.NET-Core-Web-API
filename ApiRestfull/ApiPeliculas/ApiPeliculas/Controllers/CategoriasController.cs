using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/Categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();

            if(listaCategorias == null)
            {
                return NotFound();
            }

            return Ok(listaCategorias);

        }

        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            return Ok(itemCategoria);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostCategoria([FromBody] CrearCategoriaDTO crearCategoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(crearCategoriaDTO == null)
            {
                return BadRequest(ModelState);
            }

            if(_ctRepo.ExisteCategoria(crearCategoriaDTO.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDTO);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.CategoriaId }, categoria);
        }

        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDTO categoriaDTO)
        {
            if(_ctRepo.ExisteCategoria(categoriaDTO.Nombre))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(categoriaDTO == null || categoriaId != categoriaDTO.CategoriaId)
            {
                return BadRequest(ModelState);
            }
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            if(!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{categoria:int}", Name ="BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if(!_ctRepo.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            var categoria = _ctRepo.GetCategoria(categoriaId);

            if(!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
