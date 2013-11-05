using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;

namespace FormValidation.WebAPI.Repositories
{
    public class PersonRepositoryInMemory : IPersonRepository
    {
        private static List<Person> _people = PopulatePeople();
        private static int _nextId = 3;

        private static List<Person> PopulatePeople() {
            var people = new List<Person>();
            people.Add(new Person
            {
                Id = 1
            });
            people.Add(new Person
            {
                Id = 2
            });
            return people;
        }

        public IEnumerable<Person> GetAll() {
            lock (_people) {
                // Return new collection so callers can iterate independently on separate threads
                return _people.ToArray();
            }
        }

        public Person GetById(int id) {
            lock (_people) {
                return _people.FirstOrDefault(m => m.Id == id);
            }
        }

        public Person Create(Person person) {
            if (person == null) {
                throw new ArgumentNullException("person");
            }
            person.Id = _nextId++;
            _people.Add(person);
            return person;
        }

        public int Update(Person person) {
            if (person == null) {
                throw new ArgumentNullException("person");
            }
            int index = _people.FindIndex(e => e.Id == person.Id);
            if (index == -1) {
                return 0;
            }
            _people.RemoveAt(index);
            _people.Add(person);
            return 1;
        }

        public int Delete(int personId) {
            return _people.RemoveAll(e => e.Id == personId);
        }

        public void Reset() {
            throw new NotImplementedException();
        }
    }
}
