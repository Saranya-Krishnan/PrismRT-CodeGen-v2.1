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
    public partial class State 
    { 
        public State() {
            MarkedToDelete = "false";
        }

		public int Id { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public string ZipCodeRange { get; set; }

		public string MarkedToDelete { get; set; }

    }
}
