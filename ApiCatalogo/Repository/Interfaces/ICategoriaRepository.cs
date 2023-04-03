using ApiCatalogo.Models;

namespace ApiCatalogo.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
