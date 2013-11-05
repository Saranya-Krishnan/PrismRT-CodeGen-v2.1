using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;

namespace FormValidation.WebAPI.Repositories
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAll();
        State GetById(int id);
        State Create(State state);
        int Update(State state);
        int Delete(int id);
        void Reset();
    }
}
