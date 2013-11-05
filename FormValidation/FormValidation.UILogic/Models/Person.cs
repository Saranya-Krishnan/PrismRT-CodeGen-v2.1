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
    public partial class Person : ValidatableBindableBase
    {
		private int _Id;
		private string _SSN;
		private string _BirthDate;
		private string _State;
		private string _ZipCode;
		private string _MarkedToDelete;

        public Person() {
            _MarkedToDelete = "false";
        }

		public int Id {
			get { return _Id; }
			set { SetProperty(ref _Id, value); }
		}

		[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredError")]
		[RegularExpression(Constants.SSN_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexError")]
		public string SSN {
			get { return _SSN; }
			set { SetProperty(ref _SSN, value); }
		}

		[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredError")]
		[RegularExpression(Constants.DATE_WITH_SLASHES_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexError")]
		public string BirthDate {
			get { return _BirthDate; }
			set { SetProperty(ref _BirthDate, value); }
		}

		[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredError")]
		[RegularExpression(Constants.ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexError")]
		[StringLength(2, MinimumLength = 2,  ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "InvalidLengthError")]
		public string State {
			get { return _State; }
			set { SetProperty(ref _State, value); }
		}

		[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredError")]
		[RegularExpression(Constants.NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexError")]
		[StringLength(5, MinimumLength = 5,  ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "InvalidLengthError")]
		public string ZipCode {
			get { return _ZipCode; }
			set { SetProperty(ref _ZipCode, value); }
		}

		public string MarkedToDelete {
			get { return _MarkedToDelete; }
			set { SetProperty(ref _MarkedToDelete, value); }
		}

    }
}
