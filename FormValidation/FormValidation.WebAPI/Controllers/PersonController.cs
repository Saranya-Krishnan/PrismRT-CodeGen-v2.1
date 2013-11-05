using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using FormValidation.WebAPI.Models;
using FormValidation.WebAPI.Repositories;
using FormValidation.WebAPI.Strings.en_US;

namespace FormValidation.WebAPI.Controllers
{
    public class PersonController : ApiController
    {
        
        private IPersonRepository _personRepository;

        public PersonController()
            : this(new PersonRepository())
        { }

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        // GET /api/Person
        public HttpResponseMessage Get() {
            try {
                IEnumerable<Person> personList = _personRepository.GetAll();
                return Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, personList.Count(), personList));
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        // GET /api/Person/Id 
        public HttpResponseMessage Get(int Id) {
            try {
                Person person = _personRepository.GetById(Id);
                return Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, 1, new List<Person> { person }));
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        // POST /api/Person
        public HttpResponseMessage Post(Person person)
        {
            if (ModelState.IsValid) {
                try {
                    person = _personRepository.Create(person);
                    var response = Request.CreateResponse<CrudResult>(HttpStatusCode.Created, new CrudResult(CrudStatusCode.Success, 1, new List<Person> { person }));
                    string uri = Url.Link("DefaultApi", new { Id = person.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                catch (Exception ex) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified, ex.Message);
                }
            }
            else {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);                
            }
        }

        // PUT /api/Person/3
        public HttpResponseMessage Put(int Id, Person person)
        {
            if (ModelState.IsValid) {
                try {
                    int numRowsAffected = _personRepository.Update(person);
                    var response = Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, numRowsAffected, new List<Person> { person }));
                    string uri = Url.Link("DefaultApi", new { Id = person.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                catch (Exception ex) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified, ex.Message);
                }
            }
            else {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE /api/Person/3
        public HttpResponseMessage Delete(int Id)
        {
            try {
                Person person = _personRepository.GetById(Id);
                int numRowsAffected = _personRepository.Delete(Id);
                var response = Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, numRowsAffected, new List<Person> { person }));
                string uri = Url.Link("DefaultApi", new { Id = person.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, ex.Message);
            }
        }
    }
}
