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
    public class PersonRepository : RepositoryBase, IPersonRepository
    {
        // GetAll
        public IEnumerable<Person> GetAll() {
            var  edmPeople = default(IEnumerable<EDM.Person>);

            edmPeople = _dbContext.People
                .OrderBy(edm => edm.Id);
            IEnumerable<Person> mPeople = PersonMapper.MapAllEDMtoDM(edmPeople);

            return mPeople;
        }

        // GetById
        public Person GetById(int id) {
            EDM.Person edmPerson = default(EDM.Person);

            edmPerson = _dbContext.People
               .Where(edm => edm.Id == id).FirstOrDefault();

            return PersonMapper.MapOneEDMtoDM(edmPerson);
        }

        // Create a new Person
        public Person Create(Person mPerson) {
            var result = 0;

            EDM.Person edmPerson = PersonMapper.MapOneDMtoEDM(mPerson);

            try {
                this._dbContext.People.Add(edmPerson);
                result = this._dbContext.SaveChanges();

                // DbContext sets Id to the new Identity value.
                mPerson.Id = edmPerson.Id;
            }
            catch (System.Data.UpdateException ex) {
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException
                   && ((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == 8152)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            return mPerson;
        }

        // Update an existing Person
        public int Update(Person mPerson) {
            var result = 0;
            EDM.Person edmLoadedPerson = null;
            EDM.Person edmPerson = PersonMapper.MapOneDMtoEDM(mPerson);

            try {
                // Load object into context (entity framework) 
                edmLoadedPerson = _dbContext.People.Where(edm => edm.Id == mPerson.Id).FirstOrDefault();

                if (edmLoadedPerson == null) { //not found?
                    throw new Exception("Person not found to update");
                }
                else {
                    // Update
                    _dbContext.Entry(edmLoadedPerson).CurrentValues.SetValues(edmPerson);
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

        // Delete an existing Person
        public int Delete(int id) {
            var result = 0;
            EDM.Person edmLoadedPerson = null;

            try {
                // Load object into context (entity framework) 
                edmLoadedPerson = _dbContext.People.Where(edm => edm.Id == id).FirstOrDefault();

                // Modify the context
                if (edmLoadedPerson == null) { //not found?
                    throw new Exception("Person not found to delete");
                }
                else {
                    // Delete
                    this._dbContext.People.Remove(edmLoadedPerson);
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
