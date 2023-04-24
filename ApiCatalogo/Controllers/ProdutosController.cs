using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco(); 
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _uof.ProdutoRepository.Get().ToListAsync();
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
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
        public async Task<ActionResult> Post(ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            if (produto != null)
            {  
                _uof.ProdutoRepository.Add(produto);
                await _uof.Commit(); 
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            else
            {
                return BadRequest("Não foi possível cadastrar o produto");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto); 

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit(); 

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 

            if (produto != null)
            {
                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit(); 
                return Ok();
            }
            else
            {
                return NotFound("Produto não localizado!");
            }
        }
    }
}
