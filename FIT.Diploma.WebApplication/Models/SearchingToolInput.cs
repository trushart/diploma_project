using System;
using System.Collections.Generic;

namespace FIT.Diploma.WebApplication.Models
{
    public class SearchingToolInput
    {
        public List<Condition> Conditions { get; set; }
        public RangeTypeId RangeTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ResultFormatId ResultFormatId { get; set; }
    }
}