using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jonwolfdev.Utils.Common.Validation
{
    public static class ObjectValidator
    {
        public static void Validate(object objx, string paramName)
        {
            if (objx == null)
                throw new ArgumentNullException(nameof(objx));

            if (objx is System.Collections.IEnumerable objList)
            {
                foreach (var objItem in objList)
                {
                    if (objItem == null)
                    {
                        throw new Exception($"{nameof(ObjectValidator)} Item in list is null. List type: {objx.GetType().Name}");
                    }
                    Validate(objItem, objItem.GetType().Name);
                }
            }

            var list = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(objx, new ValidationContext(objx), list, true);

            if (!isValid)
            {
                string message = string.Join("\r\n", list);
                throw new ArgumentException(message, paramName);
            }

            //`objx` must be object for this to work
            var propertyList = objx.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var objects = propertyList.Where(x => x.IsDefined(typeof(ValidateComplexObjectAttribute))).ToList();
            var lists = propertyList.Where(x => x.IsDefined(typeof(ValidateComplexListAttribute))).ToList();

            foreach (var prop in objects)
            {
                var propVal = prop.GetValue(objx);
                if (propVal != null)
                {
                    Validate(propVal, prop.Name);
                }
            }

            foreach (var prop in lists)
            {
                if (prop.GetValue(objx) is System.Collections.IEnumerable propVal)
                {
                    foreach (var listVal in propVal)
                    {
                        Validate(listVal, prop.Name);
                    }
                }
            }
        }
    }
}
