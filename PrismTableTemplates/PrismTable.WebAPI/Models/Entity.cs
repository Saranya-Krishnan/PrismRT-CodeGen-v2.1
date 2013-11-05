using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PrismTable.WebAPI.Strings.en_US;

namespace PrismTable.WebAPI.Models 
{
    public partial class Entity 
    { 
        public Entity() {
            MarkedToDelete = "false";
        }

		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

    }
}