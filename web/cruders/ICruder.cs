using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace contacts
{
    public interface ICruder {
        Task<List<object>> GetAll(int page);
        Task Add(Dictionary<string, string> ss);
        Task Update(Dictionary<string, string> old, Dictionary<string, string> @new);
        Task Delete(object val);
        Type TypeInfo { get; }
    }
}