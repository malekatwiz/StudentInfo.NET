﻿using StudentInfo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfo.Students
{
    public static class StudentHelper
    {
        public static bool IsPaymentDue()
        {
            var currentDate = DateTime.Now;

            switch(currentDate.Month)
            {
                case 2:
                case 3:
                case 4:
                    return true;
                case 6:
                    return true;
                case 8:
                    return true;
                case 10:
                case 11:
                case 12:
                    return true;
                default:
                    return false;
            }
        }

        public static string PaymentDueDate(Term term)
        {
            switch (term)
            {
                case Term.Fall:
                    return $"Sep 30, {DateTime.Now.Year}";
                case Term.Winter:
                    return $"Jan 31, {DateTime.Now.Year}";
                case Term.Summer1:
                    return $"May 31, {DateTime.Now.Year}";
                case Term.Summer2:
                    return $"Jul 31, {DateTime.Now.Year}";
                default:
                    return string.Empty;
            }
        }
    }
}