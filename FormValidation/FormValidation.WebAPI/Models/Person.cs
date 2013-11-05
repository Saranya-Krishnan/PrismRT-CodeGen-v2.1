using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FormValidation.WebAPI.Strings.en_US;

namespace FormValidation.WebAPI.Models 
{
    public partial class Person 
    { 
        public Person() {
            MarkedToDelete = "false";
        }

		public int Id { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredErrorServerSide")]
		[RegularExpression(Constants.SSN_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RegexErrorServerSide")]
		public string SSN { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredErrorServerSide")]
		[RegularExpression(Constants.DATE_WITH_SLASHES_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RegexError")]
		public string BirthDate { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredErrorServerSide")]
		[RegularExpression(Constants.ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RegexErrorServerSide")]
		[CustomValidation(typeof(Person), "ValidateState")]
		public string State { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredErrorServerSide")]
		[RegularExpression(Constants.NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RegexErrorServerSide")]
		[CustomValidation(typeof(Person), "ValidateZipCode")]
		public string ZipCode { get; set; }

		public string MarkedToDelete { get; set; }

    }
}
