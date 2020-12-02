using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DB;
namespace api.Controllers
{
    public record PhoneNumberVM(System.Guid Id, string PhoneNumber, string CustomerName, string CustomerName2);
    [ApiController]
    [Route("[controller]")]
    public class PhoneNumbersController : ControllerBase
    {
        private readonly Db _db;

        public PhoneNumbersController(DB.Db db)
        {
            _db  = db;
        }
        [HttpGet]
        public IEnumerable<PhoneNumberVM> GetAll(int page = 0)
        {
            return _db.Set<Models.PhoneNumberIndirection>()
                .Include(i => i.Customer)
                .OrderBy(i => i.Customer.FirstName)
                .ThenBy(i => i.PhoneNumber)
                .Skip(page * 10)
                .Take(10)
                .ToList()
                .Select(i => new PhoneNumberVM(i.Id, i.PhoneNumber, i.Customer.FirstName, i.Customer.LastName));
        }
        [HttpGet]
        [Route("{customerId}")]
        public IEnumerable<PhoneNumberVM> GetByOneCustomer([FromRoute]System.Guid customerId, int page = 0)
        {
            return _db.Set<Models.PhoneNumberIndirection>()
                .Include(i => i.Customer)
                .Where(i => i.CustomerId == customerId)
                .OrderBy(i => i.PhoneNumber)
                .Skip(page * 10)
                .Take(10)
                .ToList()
                .Select(i => new PhoneNumberVM(i.Id, i.PhoneNumber, i.Customer.FirstName, i.Customer.LastName));
        }
        [HttpPost]
        public async Task<Models.PhoneNumberIndirection> PostAsync(Models.PhoneNumberIndirection PhoneNumberIndirection)
        {
            var a = await _db.AddAsync(new Models.PhoneNumberIndirection {
                CustomerId = PhoneNumberIndirection.CustomerId,
                PhoneNumber = PhoneNumberIndirection.PhoneNumber
            });
            await _db.SaveChangesAsync();
            return new Models.PhoneNumberIndirection {
                Id = a.Entity.Id,
                CustomerId = a.Entity.CustomerId,
                PhoneNumber = a.Entity.PhoneNumber,
                Customer = new Models.Customer()
            };
        }
        [HttpPatch]
        public async Task<Models.PhoneNumberIndirection> PatchAsync(System.Guid PhoneNumberIndirectionId, Models.PhoneNumberIndirection PhoneNumberIndirection)
        {
            var a = await _db.FindAsync<Models.PhoneNumberIndirection>(PhoneNumberIndirectionId);
            if (a == null)
                throw new KeyNotFoundException();
            a.PhoneNumber = PhoneNumberIndirection.PhoneNumber ?? a.PhoneNumber;
            await _db.SaveChangesAsync();
            return a;
        }
        [HttpDelete]
        public async Task DeleteAsync(System.Guid PhoneNumberIndirectionId) {
            var cust =  await _db.FindAsync<Models.PhoneNumberIndirection>(PhoneNumberIndirectionId);
            _db.Remove(cust);
            await _db.SaveChangesAsync();
            return;
        }
    }
}
