using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos != null)
            {
                return produtos; 
            }
            else
            {
                return NotFound("Produtos não encontrados!"); 
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto != null)
            {
                return produto;
            }
            else
            {
                return NotFound("Produto com o id informado não encontrado!"); 
            }
        }

        [HttpGet("{nome}")]
        public ActionResult<Produto> Get(string nome)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Nome == nome);   
            if(produto != null )
            {
                return produto; 
            }
            {
                return NotFound("Produto com o nome informado não encontrado!");
            }
        }
    }
}
