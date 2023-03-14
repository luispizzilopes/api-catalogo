using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categorias.Include(p => p.Produtos).ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.SingleOrDefault(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (categoria.CategoriaId != id)
                {
                    return BadRequest();
                }

                _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }

                _context.Remove(categoria);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação!");
            }
        }

    }
}
