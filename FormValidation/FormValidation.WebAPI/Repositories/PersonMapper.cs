using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;
using EDM = FormValidation.EFxDataModel;

namespace FormValidation.WebAPI.Repositories
{
    public class PersonMapper
    {
        #region Map EntityDataModel to Model

        public static IEnumerable<Person> MapAllEDMtoDM(IEnumerable<EDM.Person> edmPeople) {
            List<Person> mPeople = new List<Person>();

            foreach (EDM.Person edmPerson in edmPeople) {
                mPeople.Add(PersonMapper.MapOneEDMtoDM(edmPerson));
            }

            return mPeople;
        }

        public static Person MapOneEDMtoDM(EDM.Person edmPerson) {
            Person mPerson = new Person();

			mPerson.Id = edmPerson.Id;
			mPerson.SSN = edmPerson.SSN;
			mPerson.BirthDate = edmPerson.BirthDate.ToShortDateString();
			mPerson.State = edmPerson.State;
			mPerson.ZipCode = edmPerson.ZipCode;
			mPerson.MarkedToDelete = edmPerson.MarkedToDelete.ToString();

            return mPerson;
        }
        
        #endregion

        #region Map Model to EntityDataModel

        public static EDM.Person MapOneDMtoEDM(Person mPerson) {
            EDM.Person edmPerson = new EDM.Person();

			edmPerson.Id = mPerson.Id;
			edmPerson.SSN = mPerson.SSN;
			edmPerson.BirthDate = DateTime.Parse(mPerson.BirthDate);
			edmPerson.State = mPerson.State;
			edmPerson.ZipCode = mPerson.ZipCode;
			edmPerson.MarkedToDelete = bool.Parse(mPerson.MarkedToDelete);

            return edmPerson;
        }

        #endregion
    }
}
