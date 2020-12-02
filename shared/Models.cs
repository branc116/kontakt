using System;
using System.Diagnostics;
#nullable enable annotations
namespace Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public System.Collections.Generic.List<AddressIndirection>? Addresses { get; set; }
        public System.Collections.Generic.List<PhoneNumberIndirection>? PhoneNumbers { get; set; }
    }
    public class AddressIndirection
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Address { get; set; }
        public Customer? Customer { get; set; }
    }

    public class PhoneNumberIndirection
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public Customer? Customer { get; set; }
    }
}
