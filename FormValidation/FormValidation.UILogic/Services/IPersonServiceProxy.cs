using FormValidation.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Services
{
    public interface IPersonServiceProxy
    {
        Task<CrudResult> GetPeopleAsync();
        Task<CrudResult> GetPersonAsync(int personId);
        Task<CrudResult> CreatePersonAsync(Person personId);
        Task<CrudResult> UpdatePersonAsync(Person phoneCall);
        Task<CrudResult> DeletePersonAsync(int personId);
    }
}
