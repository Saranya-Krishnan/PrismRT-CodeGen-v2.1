using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Models;

namespace FormValidation.WebAPI.Repositories
{
    public class StateRepositoryInMemory : IStateRepository
    {
        private static List<State> _states = PopulateStates();
        private static int _nextId = 3;

        private static List<State> PopulateStates() {
            var states = new List<State>();
            states.Add(new State
            {
                Id = 1
            });
            states.Add(new State
            {
                Id = 2
            });
            return states;
        }

        public IEnumerable<State> GetAll() {
            lock (_states) {
                // Return new collection so callers can iterate independently on separate threads
                return _states.ToArray();
            }
        }

        public State GetById(int id) {
            lock (_states) {
                return _states.FirstOrDefault(m => m.Id == id);
            }
        }

        public State Create(State state) {
            if (state == null) {
                throw new ArgumentNullException("state");
            }
            state.Id = _nextId++;
            _states.Add(state);
            return state;
        }

        public int Update(State state) {
            if (state == null) {
                throw new ArgumentNullException("state");
            }
            int index = _states.FindIndex(e => e.Id == state.Id);
            if (index == -1) {
                return 0;
            }
            _states.RemoveAt(index);
            _states.Add(state);
            return 1;
        }

        public int Delete(int stateId) {
            return _states.RemoveAll(e => e.Id == stateId);
        }

        public void Reset() {
            throw new NotImplementedException();
        }
    }
}
