using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;

namespace FormValidation.WebAPI.Repositories
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person GetById(int id);
        Person Create(Person person);
        int Update(Person person);
        int Delete(int id);
        void Reset();
    }
}
