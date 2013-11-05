using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;
using EDM = FormValidation.EFxDataModel;

namespace FormValidation.WebAPI.Repositories
{
    public class StateMapper
    {
        #region Map EntityDataModel to Model

        public static IEnumerable<State> MapAllEDMtoDM(IEnumerable<EDM.State> edmStates) {
            List<State> mStates = new List<State>();

            foreach (EDM.State edmState in edmStates) {
                mStates.Add(StateMapper.MapOneEDMtoDM(edmState));
            }

            return mStates;
        }

        public static State MapOneEDMtoDM(EDM.State edmState) {
            State mState = new State();

			mState.Id = edmState.Id;
			mState.Name = edmState.Name;
			mState.Code = edmState.Code;
			mState.ZipCodeRange = edmState.ZipCodeRange;
			mState.MarkedToDelete = edmState.MarkedToDelete.ToString();

            return mState;
        }
        
        #endregion

        #region Map Model to EntityDataModel

        public static EDM.State MapOneDMtoEDM(State mState) {
            EDM.State edmState = new EDM.State();

			edmState.Id = mState.Id;
			edmState.Name = mState.Name;
			edmState.Code = mState.Code;
			edmState.ZipCodeRange = mState.ZipCodeRange;
			edmState.MarkedToDelete = bool.Parse(mState.MarkedToDelete);

            return edmState;
        }

        #endregion
    }
}
