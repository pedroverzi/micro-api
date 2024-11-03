using Microsoft.EntityFrameworkCore;
using CRUDCadastroDeProdutos.Models;

namespace CRUDCadastroDeProdutos.Models
{
    public class ProdutosContext : DbContext
    {
        public ProdutosContext(DbContextOptions<ProdutosContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}