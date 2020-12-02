using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contacts
{
    public class PhoneNumberCruder : ICruder
    {

        public Guid CustomerId { get; }

        public Type TypeInfo => typeof(contacts.PhoneNumberVM);

        private readonly swaggerClient _sc;
        public PhoneNumberCruder(swaggerClient sc, System.Guid customerId)
        {
            CustomerId = customerId;
            _sc = sc;
        }

        public async Task<List<object>> GetAll(int page)
        {
            if (_sc is null)
                return null;
            return (await _sc?.PhoneNumbers4Async(CustomerId, page))
                ?.Cast<object>().ToList();
        }

        public async Task Add(Dictionary<string, string> ss)
        {
            await _sc.PhoneNumbersAsync(new PhoneNumberIndirection {
                CustomerId = CustomerId,
                Customer = new Customer(),
                PhoneNumber = ss[nameof(PhoneNumberIndirection.PhoneNumber)]
            });
        }

        public async Task Update(Dictionary<string, string> old, Dictionary<string, string> @new)
        {
            await _sc.PhoneNumbers2Async(Guid.Parse(old["Id"]), new PhoneNumberIndirection{
                PhoneNumber = @new["PhoneNumber"]
            });
        }

        public async Task Delete(object val)
        {
            if (val is PhoneNumberVM vm)
                await _sc.PhoneNumbers3Async(vm.Id);
        }
    }
}