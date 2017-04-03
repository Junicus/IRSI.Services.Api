using IRSI.Services.Api.Models.Data.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Extensions
{
    public static class SummaryGroupExtensions
    {
        public static string Name(this SummaryGroup group)
        {
            switch (group)
            {
                case SummaryGroup.Group0To7Mins:
                    return "0 to 7";
                case SummaryGroup.Group7To10Mins:
                    return "7 to 10";
                case SummaryGroup.Group10To13Mins:
                    return "10 to 13";
                case SummaryGroup.Group13To14Mins:
                    return "13 to 14";
                case SummaryGroup.Group14To15Mins:
                    return "14 to 15";
                case SummaryGroup.Group15To16Mins:
                    return "15 to 16";
                case SummaryGroup.Group16To20Mins:
                    return "16 to 20";
                case SummaryGroup.Group20To30Mins:
                    return "20 to 30";
                case SummaryGroup.GroupOver30Mins:
                    return "Over 30";
            }
            return string.Empty;
        }
    }
}
