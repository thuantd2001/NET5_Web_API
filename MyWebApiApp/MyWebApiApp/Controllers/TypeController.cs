using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.Linq;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : Controller
    {
        private readonly MyDbContext _context;

        public TypeController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var listType = _context.Types.ToList();
            return Ok(listType);

        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var type = _context.Types.SingleOrDefault(
                       t => t.IdType == id);
            if (type != null)
            {
                return Ok(type);
            }
            else return NotFound();
        }
        [HttpPost]
        public IActionResult CreateNew(TypeModel model)
        {
            try
            {
                var type = new Type
                {
                    NameType = model.Name
                };
                _context.Add(type);
                _context.SaveChanges();
                return Ok(type);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult updateById(int id, TypeModel model)
        {
            try
            {
                var type = _context.Types.SingleOrDefault(
                       t => t.IdType == id);
                if (type != null)
                {
                    type.NameType = model.Name;
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var type = _context.Types.SingleOrDefault(
                       t => t.IdType == id);
                if (type != null)
                {
                    _context.Remove(type);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
