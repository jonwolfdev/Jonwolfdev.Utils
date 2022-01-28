using System;
using System.Collections.Generic;
using System.Text;

namespace Jonwolfdev.Utils6.Validation
{
    public interface IObjectValidator<T>
    {
        void Validate(object obj, string paramName);
    }
}
