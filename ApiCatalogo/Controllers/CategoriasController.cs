using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.Versioning;

namespace ApiCatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper; 

        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper; 
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos(); 
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);


                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.Get().ToListAsync();
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria); 

                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria); 

                if (categoria is null)
                {
                    return NotFound();
                }

                return Ok(categoriaDto);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                if (categoria is null)
                {
                    return BadRequest();
                }

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto); 

                if (categoria.CategoriaId != id)
                {
                    return BadRequest();
                }

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit(); 

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

    }
}
