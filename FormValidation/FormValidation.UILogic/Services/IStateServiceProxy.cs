using FormValidation.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Services
{
    public interface IStateServiceProxy
    {
        Task<CrudResult> GetStatesAsync();
        Task<CrudResult> GetStateAsync(int stateId);
        Task<CrudResult> CreateStateAsync(State stateId);
        Task<CrudResult> UpdateStateAsync(State phoneCall);
        Task<CrudResult> DeleteStateAsync(int stateId);
    }
}
