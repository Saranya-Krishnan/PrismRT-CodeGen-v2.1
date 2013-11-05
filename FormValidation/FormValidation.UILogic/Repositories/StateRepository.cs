using FormValidation.UILogic.Models;
using FormValidation.UILogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Repositories 
{
    public class StateRepository : IStateRepository
    {
        private readonly IStateServiceProxy _stateServiceProxy;

        public StateRepository(IStateServiceProxy stateServiceProxy) {
            _stateServiceProxy = stateServiceProxy;
        }

        // Get all States
        public async Task<CrudResult> GetStatesAsync() {
            CrudResult crudResult = await _stateServiceProxy.GetStatesAsync();
            return crudResult;
        }

        // Get a State by Id
        public async Task<CrudResult> GetStateAsync(int stateId) {
            CrudResult crudResult = await _stateServiceProxy.GetStateAsync(stateId);
            return crudResult;
        }

        // Create a new State
        public async Task<CrudResult> CreateStateAsync(State state) {
            CrudResult crudResult = await _stateServiceProxy.CreateStateAsync(state);
            return crudResult;
        }

        // Update an existing State
        public async Task<CrudResult> UpdateStateAsync(State state) {
            CrudResult crudResult = await _stateServiceProxy.UpdateStateAsync(state);
            return crudResult;
        }

        // Delete an existing State
        public async Task<CrudResult> DeleteStateAsync(int stateId) {
            CrudResult crudResult = await _stateServiceProxy.DeleteStateAsync(stateId);
            return crudResult;
        }
    }
}

