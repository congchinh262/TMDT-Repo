using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Data;
using MobieStoreWeb.DTO;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductCommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ProductComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCommentDTO>>> GetProductComments(int? productId)
        {
            var comments = _context.ProductComments.AsQueryable();
            if (productId.HasValue)
            {
                comments = comments.Where(c => c.ProductId == productId);
            }
            comments = comments.Include(c => c.User);
            return await comments.Select(c => new ProductCommentDTO
            {
                Id = c.Id,
                ProductId = c.ProductId,
                UserId = c.UserId,
                UserName = c.User.UserName,
                Content = c.Content,
                Rating = c.Rating,
                CreatedDate = c.CreatedDate,
            }).ToListAsync();
        }

        // GET: api/ProductComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductComment>> GetProductComment(int id)
        {
            var productComment = await _context.ProductComments.FindAsync(id);

            if (productComment == null)
            {
                return NotFound();
            }

            return productComment;
        }

        // PUT: api/ProductComments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductComment(int id, ProductComment productComment)
        {
            if (id != productComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(productComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCommentExists(id))
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

        // POST: api/ProductComments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductCommentDTO>> PostProductComment([FromBody]ProductComment productComment)
        {
            var user = await _userManager.GetUserAsync(User);
            productComment.UserId = user.Id;
            productComment.CreatedDate = DateTime.UtcNow;
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                _context.ProductComments.Add(productComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductComment", new { id = productComment.Id }, new ProductCommentDTO
            {
                Id = productComment.Id,
                ProductId = productComment.ProductId,
                UserId = productComment.UserId,
                UserName = user.UserName,
                Content = productComment.Content,
                Rating = productComment.Rating,
                CreatedDate = productComment.CreatedDate,
            });
        }

        // DELETE: api/ProductComments/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductComment>> DeleteProductComment(int id)
        {
            var productComment = await _context.ProductComments.FindAsync(id);
            if (productComment == null)
            {
                return NotFound();
            }

            _context.ProductComments.Remove(productComment);
            await _context.SaveChangesAsync();

            return productComment;
        }

        private bool ProductCommentExists(int id)
        {
            return _context.ProductComments.Any(e => e.Id == id);
        }
    }
}
