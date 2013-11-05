using FormValidation.UILogic.Models;
using FormValidation.UILogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Repositories 
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IPersonServiceProxy _personServiceProxy;

        public PersonRepository(IPersonServiceProxy personServiceProxy) {
            _personServiceProxy = personServiceProxy;
        }

        // Get all People
        public async Task<CrudResult> GetPeopleAsync() {
            CrudResult crudResult = await _personServiceProxy.GetPeopleAsync();
            return crudResult;
        }

        // Get a Person by Id
        public async Task<CrudResult> GetPersonAsync(int personId) {
            CrudResult crudResult = await _personServiceProxy.GetPersonAsync(personId);
            return crudResult;
        }

        // Create a new Person
        public async Task<CrudResult> CreatePersonAsync(Person person) {
            CrudResult crudResult = await _personServiceProxy.CreatePersonAsync(person);
            return crudResult;
        }

        // Update an existing Person
        public async Task<CrudResult> UpdatePersonAsync(Person person) {
            CrudResult crudResult = await _personServiceProxy.UpdatePersonAsync(person);
            return crudResult;
        }

        // Delete an existing Person
        public async Task<CrudResult> DeletePersonAsync(int personId) {
            CrudResult crudResult = await _personServiceProxy.DeletePersonAsync(personId);
            return crudResult;
        }
    }
}

