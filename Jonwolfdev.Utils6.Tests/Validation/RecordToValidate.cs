using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Tests.Validation
{
    internal record RecordToValidate([property: MinLength(1), MaxLength(2), Required] string AnotherValue)
    {
        [MinLength(1), MaxLength(2), Required]
        public string Value { get; set; } = "";
    }
}
