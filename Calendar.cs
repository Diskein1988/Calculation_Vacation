using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation_Vacation
{
    internal class Calendar
    {
        public string Dayofyers { get; }
        public int Work_Months { get; }
        DateTime nowDate = DateTime.Now;

        public Calendar()
        {
            Dayofyers = calc_dayofyers();
            Work_Months = nowDate.Month;
        }

        private string calc_dayofyers()
        {
            return nowDate.DayOfYear.ToString();
        }

        public string Day_osn()
        {
            int day_osn_otpusk = Convert.ToInt32(Math.Round(0.077 * nowDate.DayOfYear));
            return day_osn_otpusk.ToString();
        }

        public string Day_dop(string day)
        {
            if (!string.IsNullOrEmpty(day))
            {
                if (IsAllDigi(day))
                {
                    int day_of_work = int.Parse(day.Trim());

                    if (day_of_work <= 364)
                    {
                        int day_dop_otpusk = Convert.ToInt32(Math.Round(0.043 * day_of_work));
                        return day_dop_otpusk.ToString();
                    }
                    else
                    {
                        return "Err_more_day";
                    }
                }
                else
                {
                    return "Err_NoDigi";
                }
            }
            else
            {
                return "Err_Null";
            }
        }

        private bool IsAllDigi(string raw)
        {
            string txt = raw.Trim(); // Убераем с начала и конца строки пробелы
            if (txt.Length == 0)
            {
                return false;
            }

            for (int index = 0; index < txt.Length; index++) // Проверяем введеные символы на наличие только цифр
            {
                if (char.IsDigit(txt[index]) == false)
                {
                    return false;
                }
            }

            return true;
        } //Проверка на цифры


    }
}
