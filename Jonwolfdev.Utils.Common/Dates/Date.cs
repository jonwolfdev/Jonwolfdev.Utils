using System;
using System.Collections.Generic;
using System.Text;

namespace Jonwolfdev.Utils.Common.Dates
{
    public class Date
    {
        DateTimeOffset _date;
        public DateTimeOffset DateTimeOffset { get => _date; set { _date = value; } }
        public DateTimeOffset OnlyUtcDate
        {
            get => _date.UtcDateTime.Date;
        }
        public DateTimeOffset OnlyLocalDate
        {
            get => _date.LocalDateTime.Date;
        }
        public Date()
        {
            // For serialization
        }
        public Date(DateTimeOffset dt)
        {
            _date = dt.UtcDateTime.Date;
        }
    }
}
