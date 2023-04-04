using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return Ok(_uof.ProdutoRepository.GetProdutosPorPreco().ToList());
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            if (produtos != null)
            {
                return produtos;
            }
            else
            {
                return NotFound("Produtos não encontrados!");
            }
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto != null)
            {
                return produto;
            }
            else
            {
                return NotFound("Produto com o id informado não encontrado!");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto != null)
            {
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit(); 
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            else
            {
                return BadRequest("Não foi possível cadastrar o produto");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit(); 

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 

            if (produto != null)
            {
                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit(); 
                return Ok();
            }
            else
            {
                return NotFound("Produto não localizado!");
            }
        }
    }
}
