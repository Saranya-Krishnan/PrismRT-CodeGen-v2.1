using FormValidation.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Models
{
    public partial class State : ValidatableBindableBase
    {
		private int _Id;
		private string _Name;
		private string _Code;
		private string _ZipCodeRange;
		private string _MarkedToDelete;

        public State() {
            _MarkedToDelete = "false";
        }

		public int Id {
			get { return _Id; }
			set { SetProperty(ref _Id, value); }
		}

		public string Name {
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		public string Code {
			get { return _Code; }
			set { SetProperty(ref _Code, value); }
		}

		public string ZipCodeRange {
			get { return _ZipCodeRange; }
			set { SetProperty(ref _ZipCodeRange, value); }
		}

		public string MarkedToDelete {
			get { return _MarkedToDelete; }
			set { SetProperty(ref _MarkedToDelete, value); }
		}

    }
}
