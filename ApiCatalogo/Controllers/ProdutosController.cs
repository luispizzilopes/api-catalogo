using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper; 
        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList(); 
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos); 

            if (produtosDto != null)
            {
                return Ok(produtosDto);
            }
            else
            {
                return NotFound("Produtos não encontrados!");
            }
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            if (produtoDto != null)
            {
                return Ok(produtoDto);
            }
            else
            {
                return NotFound("Produto com o id informado não encontrado!");
            }
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

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
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto); 

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
