using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Person;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IPersonRepository: IRepository<Person>
    {
        Task<bool> PatchPersonAsync(PersonUpdateDto dto);
        Task<Person> GetByDocumentAsync(long numberIdentification);
    }
}
