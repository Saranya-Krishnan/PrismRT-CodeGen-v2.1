using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Models
{
    public partial class Person
    {
        [CustomValidation(typeof(Person), "SetAge")]
        public string Age { get; set; }

        public static ValidationResult SetAge(string value, ValidationContext validationContext)
        {
            bool isValid = false;

            //if (value == null)
            //{
            //    throw new ArgumentNullException("value");
            //}

            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            var person = (Person)validationContext.ObjectInstance;
            DateTime parsedDateTime;
            if (DateTime.TryParse(person.BirthDate, out parsedDateTime))
            {
                parsedDateTime = DateTime.Parse(person.BirthDate);
                person.Age = (DateTime.Now.Year - parsedDateTime.Year).ToString();
                isValid = true;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Age could not be calculated");
            }

        }
    }
}
