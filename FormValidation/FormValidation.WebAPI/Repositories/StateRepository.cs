using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;
using EDM = FormValidation.EFxDataModel;

namespace FormValidation.WebAPI.Repositories
{
    public class StateRepository : RepositoryBase, IStateRepository
    {
        // GetAll
        public IEnumerable<State> GetAll() {
            var  edmStates = default(IEnumerable<EDM.State>);

            edmStates = _dbContext.States
                .OrderBy(edm => edm.Id);
            IEnumerable<State> mStates = StateMapper.MapAllEDMtoDM(edmStates);

            return mStates;
        }

        // GetById
        public State GetById(int id) {
            EDM.State edmState = default(EDM.State);

            edmState = _dbContext.States
               .Where(edm => edm.Id == id).FirstOrDefault();

            return StateMapper.MapOneEDMtoDM(edmState);
        }

        // Create a new State
        public State Create(State mState) {
            var result = 0;

            EDM.State edmState = StateMapper.MapOneDMtoEDM(mState);

            try {
                this._dbContext.States.Add(edmState);
                result = this._dbContext.SaveChanges();

                // DbContext sets Id to the new Identity value.
                mState.Id = edmState.Id;
            }
            catch (System.Data.UpdateException ex) {
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException
                   && ((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == 8152)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            return mState;
        }

        // Update an existing State
        public int Update(State mState) {
            var result = 0;
            EDM.State edmLoadedState = null;
            EDM.State edmState = StateMapper.MapOneDMtoEDM(mState);

            try {
                // Load object into context (entity framework) 
                edmLoadedState = _dbContext.States.Where(edm => edm.Id == mState.Id).FirstOrDefault();

                if (edmLoadedState == null) { //not found?
                    throw new Exception("State not found to update");
                }
                else {
                    // Update
                    _dbContext.Entry(edmLoadedState).CurrentValues.SetValues(edmState);
                }

                // Save in data access (entity framework)
                result = this._dbContext.SaveChanges();
            }
            catch (System.Data.UpdateException ex) {
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException
                   && ((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == 8152)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            return result;
        }

        // Delete an existing State
        public int Delete(int id) {
            var result = 0;
            EDM.State edmLoadedState = null;

            try {
                // Load object into context (entity framework) 
                edmLoadedState = _dbContext.States.Where(edm => edm.Id == id).FirstOrDefault();

                // Modify the context
                if (edmLoadedState == null) { //not found?
                    throw new Exception("State not found to delete");
                }
                else {
                    // Delete
                    this._dbContext.States.Remove(edmLoadedState);
                }

                // Save in data access (entity framework)
                result = this._dbContext.SaveChanges();
            }
            catch (System.Data.UpdateException ex) {
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException
                   && ((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == 8152)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            return result;
        }

        public void Reset() {
            throw new NotImplementedException();
        }
    }
}
