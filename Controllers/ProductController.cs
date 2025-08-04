using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuleWay.ProductApi.Data;
using RuleWay.ProductApi.Dtos;
using RuleWay.ProductApi.Entities;

namespace RuleWay.ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .ToListAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 200)
                return BadRequest("Başlık gereklidir ve 200 karakterden kısa olmalıdır.");

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Kategori bulunamadı.");

            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 200)
                return BadRequest("Başlık gereklidir ve 200 karakterden kısa olmalıdır.");

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Kategori bulunuamadı.");

            product.Title = dto.Title;
            product.Description = dto.Description;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Product>>> Filter([FromQuery] ProductFilterDto filter)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string keyword = filter.Search.ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(keyword) ||
                    (p.Description != null && p.Description.ToLower().Contains(keyword)) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(keyword)));
            }

            if (filter.MinStock.HasValue)
                query = query.Where(p => p.StockQuantity >= filter.MinStock.Value);

            if (filter.MaxStock.HasValue)
                query = query.Where(p => p.StockQuantity <= filter.MaxStock.Value);

            var result = await query.ToListAsync();
            return Ok(result);
        }
    }
}
