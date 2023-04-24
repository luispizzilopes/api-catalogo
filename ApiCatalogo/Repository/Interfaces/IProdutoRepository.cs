using ApiCatalogo.Models;

namespace ApiCatalogo.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorPreco(); 
    }
}
