using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrismTable.WebAPI.Models;
using EDM = PrismTable.EFxDataModel;

namespace PrismTable.WebAPI.Repositories
{
    public class EntityMapper
    {
        #region Map EntityDataModel to Model

        public static IEnumerable<Entity> MapAllEDMtoDM(IEnumerable<EDM.Entity> edmEntities) {
            List<Entity> mEntities = new List<Entity>();

            foreach (EDM.Entity edmEntity in edmEntities) {
                mEntities.Add(EntityMapper.MapOneEDMtoDM(edmEntity));
            }

            return mEntities;
        }

        public static Entity MapOneEDMtoDM(EDM.Entity edmEntity) {
            Entity mEntity = new Entity();

			mEntity.Id = edmEntity.Id;
			mEntity.FirstName = edmEntity.FirstName;
			mEntity.LastName = edmEntity.LastName;

            return mEntity;
        }
        
        #endregion

        #region Map Model to EntityDataModel

        public static EDM.Entity MapOneDMtoEDM(Entity mEntity) {
            EDM.Entity edmEntity = new EDM.Entity();

			edmEntity.Id = mEntity.Id;
			edmEntity.FirstName = mEntity.FirstName;
			edmEntity.LastName = mEntity.LastName;

            return edmEntity;
        }

        #endregion
    }
}
