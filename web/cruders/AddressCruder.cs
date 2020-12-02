using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contacts
{
    public class AddressCruder : ICruder
    {

        public Guid CustomerId { get; }

        public Type TypeInfo => typeof(contacts.AddressVM);

        private readonly swaggerClient _sc;
        public AddressCruder(swaggerClient sc, System.Guid customerId)
        {
            CustomerId = customerId;
            _sc = sc;
        }

        public async Task<List<object>> GetAll(int page)
        {
            if (_sc is null)
                return null;
            return (await _sc?.Address4Async(CustomerId, page))
                ?.Cast<object>().ToList();
        }

        public async Task Add(Dictionary<string, string> ss)
        {
            await _sc.AddressAsync(new AddressIndirection {
                CustomerId = CustomerId,
                Customer = new Customer(),
                Address = ss[nameof(AddressIndirection.Address)]
            });
        }

        public async Task Update(Dictionary<string, string> old, Dictionary<string, string> @new)
        {
            await _sc.Address2Async(Guid.Parse(old["Id"]), new AddressIndirection{
                Address = @new["Address"]
            });
        }

        public async Task Delete(object val)
        {
            if (val is contacts.AddressVM vm)
                await _sc.Address3Async(vm.Id);
        }
    }
}