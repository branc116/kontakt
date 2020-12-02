using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DB;
using Microsoft.AspNetCore.Cors;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly Db _db;

        public ContactsController(DB.Db db)
        {
            _db  = db;
        }

        [HttpGet]
        public IAsyncEnumerable<Models.Customer> Get(int page = 0)
        {
            return _db.Set<Models.Customer>()
                .OrderBy(i => i.FirstName)
                .ThenBy(i => i.LastName)
                .Skip(page * 10)
                .Take(10)
                .AsAsyncEnumerable();
        }
        [HttpGet]
        [Route("{customerId}")]
        public async Task<Models.Customer> GetOne(System.Guid customerId)
        {
            return await _db.Set<Models.Customer>()
                .Where(i => i.Id == customerId)
                .FirstOrDefaultAsync();
        }
        [HttpPost]
        public async Task<Models.Customer> PostAsync(Models.Customer customer)
        {
            var a = await _db.AddAsync(customer);
            await _db.SaveChangesAsync();
            return a.Entity;
        }
        [HttpPatch]
        public async Task<Models.Customer> PatchAsync(System.Guid CustomerId, Models.Customer customer)
        {
            var a = await _db.FindAsync<Models.Customer>(CustomerId);
            if (a == null)
                throw new KeyNotFoundException();
            a.FirstName = customer.FirstName ?? a.FirstName;
            a.LastName = customer.LastName ?? a.LastName;
            a.DateOfBirth = customer.DateOfBirth ?? a.DateOfBirth;
            await _db.SaveChangesAsync();
            return a;
        }
        [HttpDelete]
        public async Task DeleteAsync(System.Guid CustomerId) {
            var cust =  await _db.FindAsync<Models.Customer>(CustomerId);
            _db.Remove(cust);
            await _db.SaveChangesAsync();
            return;
        }
    }
}
