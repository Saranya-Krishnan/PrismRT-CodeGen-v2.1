using FormValidation.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Repositories
{
    public interface IStateRepository
    {
        Task<CrudResult> GetStatesAsync();
        Task<CrudResult> GetStateAsync(int stateId);
        Task<CrudResult> CreateStateAsync(State state);
        Task<CrudResult> UpdateStateAsync(State state);
        Task<CrudResult> DeleteStateAsync(int stateId);
    }
}
