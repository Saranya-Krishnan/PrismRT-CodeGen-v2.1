using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrismTable.WebAPI.Models;
using EDM = PrismTable.EFxDataModel;

namespace PrismTable.WebAPI.Repositories
{
    public class EntityRepository : RepositoryBase, IEntityRepository
    {
        // GetAll
        public IEnumerable<Entity> GetAll() {
            var  edmEntities = default(IEnumerable<EDM.Entity>);

            edmEntities = _dbContext.Entities
                .OrderBy(edm => edm.Id);
            IEnumerable<Entity> mEntities = EntityMapper.MapAllEDMtoDM(edmEntities);

            return mEntities;
        }

        // GetById
        public Entity GetById(int id) {
            EDM.Entity edmEntity = default(EDM.Entity);

            edmEntity = _dbContext.Entities
               .Where(edm => edm.Id == id).FirstOrDefault();

            return EntityMapper.MapOneEDMtoDM(edmEntity);
        }

        // Create a new Entity
        public Entity Create(Entity mEntity) {
            var result = 0;

            EDM.Entity edmEntity = EntityMapper.MapOneDMtoEDM(mEntity);

            try {
                this._dbContext.Entities.Add(edmEntity);
                result = this._dbContext.SaveChanges();

                // DbContext sets Id to the new Identity value.
                mEntity.Id = edmEntity.Id;
            }
            catch (System.Data.UpdateException ex) {
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException
                   && ((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == 8152)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            return mEntity;
        }

        // Update an existing Entity
        public int Update(Entity mEntity) {
            var result = 0;
            EDM.Entity edmLoadedEntity = null;
            EDM.Entity edmEntity = EntityMapper.MapOneDMtoEDM(mEntity);

            try {
                // Load object into context (entity framework) 
                edmLoadedEntity = _dbContext.Entities.Where(edm => edm.Id == mEntity.Id).FirstOrDefault();

                if (edmLoadedEntity == null) { //not found?
                    throw new Exception("Entity not found to update");
                }
                else {
                    // Update
                    _dbContext.Entry(edmLoadedEntity).CurrentValues.SetValues(edmEntity);
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

        // Delete an existing Entity
        public int Delete(int id) {
            var result = 0;
            EDM.Entity edmLoadedEntity = null;

            try {
                // Load object into context (entity framework) 
                edmLoadedEntity = _dbContext.Entities.Where(edm => edm.Id == id).FirstOrDefault();

                // Modify the context
                if (edmLoadedEntity == null) { //not found?
                    throw new Exception("Entity not found to delete");
                }
                else {
                    // Delete
                    this._dbContext.Entities.Remove(edmLoadedEntity);
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
