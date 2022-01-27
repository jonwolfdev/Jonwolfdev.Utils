using Jonwolfdev.Utils6.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jonwolfdev.Utils.Common.UnitTests.Validation
{
    public class ObjectToValidate
    {
        [Range(0, 1)]
        public int Integer { get; set; }
        [Required, MaxLength(3)]
        public string String { get; set; }

        [ValidateComplexObject]
        public ObjectToValidate ValidateThis { get; set; }
        [Required, ValidateComplexList]
        public List<ObjectToValidate> ObjectsToValidate { get; set; }

        [ValidateComplexList, MinLength(1)]
        public List<ObjectToValidate> ObjectsToValidateRange { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string ImageUrl { get; set; }

        public ObjectToValidate()
        {
            ImageUrl = string.Empty;
        }
    }
}
