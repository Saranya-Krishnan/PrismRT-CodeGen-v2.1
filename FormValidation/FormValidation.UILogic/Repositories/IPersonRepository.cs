using FormValidation.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Repositories
{
    public interface IPersonRepository
    {
        Task<CrudResult> GetPeopleAsync();
        Task<CrudResult> GetPersonAsync(int personId);
        Task<CrudResult> CreatePersonAsync(Person person);
        Task<CrudResult> UpdatePersonAsync(Person person);
        Task<CrudResult> DeletePersonAsync(int personId);
    }
}
