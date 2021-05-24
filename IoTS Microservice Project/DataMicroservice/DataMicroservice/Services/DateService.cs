using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMicroservice.Contracts;

namespace DataMicroservice.Services
{
    public class DateService : IDateService
    {
        public String ConvertDateToString(DateTime date)
        {
            String result = date.Year + "-" + TwoCharacterString(date.Month) + "-" + TwoCharacterString(date.Day) + "T"
                          + TwoCharacterString(date.Hour) + ":" + TwoCharacterString(date.Minute) + ":" + TwoCharacterString(date.Second);
            return result;
        }

        private String TwoCharacterString(int number)
        {
            if (number < 10)
                return "0" + number;
            else return number.ToString();
        }
    }
}
