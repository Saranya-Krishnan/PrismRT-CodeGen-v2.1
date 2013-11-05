using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrismTable.WebAPI.Models;

namespace PrismTable.WebAPI.Repositories
{
    public interface IEntityRepository
    {
        IEnumerable<Entity> GetAll();
        Entity GetById(int id);
        Entity Create(Entity entity);
        int Update(Entity entity);
        int Delete(int id);
        void Reset();
    }
}