using ApiCatalogo.Repository.Interfaces;

namespace ApiCatalogo.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        void Commit(); 
    }
}
