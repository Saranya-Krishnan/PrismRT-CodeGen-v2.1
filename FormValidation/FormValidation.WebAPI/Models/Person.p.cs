using FormValidation.WebAPI.Repositories;
using FormValidation.WebAPI.Strings.en_US;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.WebAPI.Models
{
    public partial class Person
    {
        public static ValidationResult ValidateZipCode(object value, ValidationContext validationContext)
        {
            // http://stackoverflow.com/questions/10165143/customvalidationattribute-specifed-method-not-being-called
            // Above example seems to imply that:
            //      1) object value is the value of ZipCode of the current instance of Person
            //      2) validationContext is the current instance of Person which gives access to other property values of the current instance
            //      3) validationContext can be used with dependent properties (http://msdn.microsoft.com/en-us/library/windows/apps/xx130659.aspx)
            bool isValid = false;
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext");
                }

                var person = (Person)validationContext.ObjectInstance;

                if (person.ZipCode.Length < 3)
                {
                    return new ValidationResult(Resources.InvalidLengthErrorServerSide);
                }

                string code = person.State;
                int zipCode = Convert.ToInt32(person.ZipCode.Substring(0, 3), CultureInfo.InvariantCulture);
                State state = new StateRepository().GetAll().Where(s => s.Code == code).FirstOrDefault();
                if (state != null)
                {
                    List<string> ValidZipCodeRanges = new List<string>(state.ZipCodeRange.Split(new char[] { ',' }));

                    foreach (string range in ValidZipCodeRanges)
                    {
                        // If the first 3 digits of the Zip Code falls within the given range, it is valid.
                        int minValue = Convert.ToInt32(range.Split('-')[0], CultureInfo.InvariantCulture);
                        int maxValue = Convert.ToInt32(range.Split('-')[1], CultureInfo.InvariantCulture);

                        isValid = zipCode >= minValue && zipCode <= maxValue;

                        if (isValid) break;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(Resources.InvalidValueErrorServerSide);
            }
        }

        public static ValidationResult ValidateState(object value, ValidationContext validationContext)
        {
            bool isValid = false;
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext");
                }

                var person = (Person)validationContext.ObjectInstance;

                string code = person.State;
                List<State> states = new StateRepository().GetAll().ToList<State>();
                foreach (State state in states)
                {
                    if (state.Code == code)
                    {
                        isValid = true;
                        break;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(Resources.InvalidValueErrorServerSide);
            }
        }
    }
}
