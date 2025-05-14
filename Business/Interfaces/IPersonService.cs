    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Entity.DTOs.Person;
    using Entity.Model;

    namespace Business.Interfaces
    {
        public interface IPersonService : IBaseService<PersonUpdateDto, Person>

        {
            Task<bool> PatchPersonAsync(PersonUpdateDto dto);
            Task AddFromCreateDtoAsync(PersonDto dto);

    }
}
