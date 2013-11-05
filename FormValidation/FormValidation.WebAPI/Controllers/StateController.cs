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
    public class StateController : ApiController
    {
        
        private IStateRepository _stateRepository;

        public StateController()
            : this(new StateRepository())
        { }

        public StateController(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        // GET /api/State
        public HttpResponseMessage Get() {
            try {
                IEnumerable<State> stateList = _stateRepository.GetAll();
                return Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, stateList.Count(), stateList));
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        // GET /api/State/Id 
        public HttpResponseMessage Get(int Id) {
            try {
                State state = _stateRepository.GetById(Id);
                return Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, 1, new List<State> { state }));
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        // POST /api/State
        public HttpResponseMessage Post(State state)
        {
            if (ModelState.IsValid) {
                try {
                    state = _stateRepository.Create(state);
                    var response = Request.CreateResponse<CrudResult>(HttpStatusCode.Created, new CrudResult(CrudStatusCode.Success, 1, new List<State> { state }));
                    string uri = Url.Link("DefaultApi", new { Id = state.Id });
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

        // PUT /api/State/3
        public HttpResponseMessage Put(int Id, State state)
        {
            if (ModelState.IsValid) {
                try {
                    int numRowsAffected = _stateRepository.Update(state);
                    var response = Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, numRowsAffected, new List<State> { state }));
                    string uri = Url.Link("DefaultApi", new { Id = state.Id });
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

        // DELETE /api/State/3
        public HttpResponseMessage Delete(int Id)
        {
            try {
                State state = _stateRepository.GetById(Id);
                int numRowsAffected = _stateRepository.Delete(Id);
                var response = Request.CreateResponse<CrudResult>(HttpStatusCode.OK, new CrudResult(CrudStatusCode.Success, numRowsAffected, new List<State> { state }));
                string uri = Url.Link("DefaultApi", new { Id = state.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, ex.Message);
            }
        }
    }
}
