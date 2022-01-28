using System;
using System.Collections.Generic;
using System.Text;

namespace Jonwolfdev.Utils.Common.Validation
{
    public interface IObjectValidator
    {
        void Validate(object obj, string paramName);
    }

    public interface IObjectValidator<T> : IObjectValidator
    {
    }
}
