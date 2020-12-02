using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DB;
namespace api.Controllers
{
    public record AddressVM(System.Guid Id, string Address, string CustomerName, string CustomerName2);
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly Db _db;

        public AddressController(DB.Db db)
        {
            _db  = db;
        }
        [HttpGet]
        public IEnumerable<AddressVM> GetAll(int page = 0)
        {
            return _db.Set<Models.AddressIndirection>()
                .Include(i => i.Customer)
                .OrderBy(i => i.Customer.FirstName)
                .ThenBy(i => i.Address)
                .Skip(page * 10)
                .Take(10)
                .ToList()
                .Select(i => new AddressVM(i.Id, i.Address, i.Customer.FirstName, i.Customer.LastName));
        }
        [HttpGet]
        [Route("{customerId}")]
        public IEnumerable<AddressVM> GetByOneCustomer([FromRoute]System.Guid customerId, int page = 0)
        {
            return _db.Set<Models.AddressIndirection>()
                .Include(i => i.Customer)
                .Where(i => i.CustomerId == customerId)
                .OrderBy(i => i.Address)
                .Skip(page * 10)
                .Take(10)
                .ToList()
                .Select(i => new AddressVM(i.Id, i.Address, i.Customer.FirstName, i.Customer.LastName));
        }
        [HttpPost]
        public async Task<Models.AddressIndirection> PostAsync(Models.AddressIndirection AddressIndirection)
        {
            var a = await _db.AddAsync(new Models.AddressIndirection {
                CustomerId = AddressIndirection.CustomerId,
                Address = AddressIndirection.Address
            });
            await _db.SaveChangesAsync();
            return new Models.AddressIndirection {
                Id = a.Entity.Id,
                Address = a.Entity.Address,
                CustomerId = a.Entity.CustomerId,
                Customer = new Models.Customer()
            };
        }
        [HttpPatch]
        public async Task<Models.AddressIndirection> PatchAsync(System.Guid AddressIndirectionId, Models.AddressIndirection AddressIndirection)
        {
            var a = await _db.FindAsync<Models.AddressIndirection>(AddressIndirectionId);
            if (a == null)
                throw new KeyNotFoundException();
            a.Address = AddressIndirection.Address ?? a.Address;
            await _db.SaveChangesAsync();
            return a;
        }
        [HttpDelete]
        public async Task DeleteAsync(System.Guid AddressIndirectionId) {
            var cust =  await _db.FindAsync<Models.AddressIndirection>(AddressIndirectionId);
            _db.Remove(cust);
            await _db.SaveChangesAsync();
            return;
        }
    }
}
