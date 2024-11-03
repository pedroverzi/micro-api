using Microsoft.AspNetCore.Mvc;
using CRUDCadastroDeProdutos.Models; // Certifique-se de que o namespace do seu contexto esteja correto
using Microsoft.EntityFrameworkCore;

namespace CRUDCadastroDeProdutos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutosContext _context; // Injeção de dependências do contexto

        public ProdutosController(ProdutosContext context) // Construtor
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // POST: api/Produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> CreateProduto(Produto produto)
        {
            var existingProduto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Nome == produto.Nome);

            if (existingProduto != null)
            {
                // Verifica se os preços de custo e venda são os mesmos
                if (existingProduto.PrecoCusto != produto.PrecoCusto || existingProduto.PrecoVenda != produto.PrecoVenda)
                {
                    return BadRequest(new 
                    {
                        message = "Um produto com o mesmo nome já existe, mas os preços são diferentes.",
                        PrecoCustoExistente = existingProduto.PrecoCusto,
                        PrecoVendaExistente = existingProduto.PrecoVenda
                    });
                }

                // Atualiza a quantidade do produto existente
                existingProduto.Quantidade += produto.Quantidade; // Adiciona a nova quantidade
                await _context.SaveChangesAsync();

                return Ok(existingProduto); // Retorna o produto atualizado
            }

            // Se não existir, adiciona o novo produto
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdutoById", new { id = produto.Id }, produto);
        }

        // PUT: api/Produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
