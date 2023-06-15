using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcStore.Data;

namespace PcStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly Somkin1Context _context;

        public ChartController(Somkin1Context context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var items = _context.Items.ToList();
            var lst = new List<object>
            {
                new[] { "Item name", "Item count" }
            };
            foreach (var i in items)
            {
                var order = _context.Orders.Count(o => o.Items.Contains(i));
                lst.Add(new object[] { i.Name, order });
            }
            return new JsonResult(lst);

        }
    }
}
