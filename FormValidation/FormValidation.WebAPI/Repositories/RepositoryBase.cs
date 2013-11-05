using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormValidation.EFxDataModel;

namespace FormValidation.WebAPI.Repositories
{
    public class RepositoryBase : IDisposable
    {
        /// <summary>
        /// Reference to data access instance (Entity Framework) and loads at instantiation.
        /// </summary>
        protected FormValidationEntities _dbContext = new FormValidationEntities();


        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                // free managed resources
                if (_dbContext != null) {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        #endregion
    }
}
