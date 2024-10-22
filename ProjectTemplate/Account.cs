﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    /*
     * ACCOUNT CLASS
     * 2/9/2021 - Varun S
     * Serves as a model template for an account's json data in our application.
     * The professor's example code uses public fields, but I instead placed those in properties. 
     */
    public class Account
    {
        private DateTime dayLastUsed;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DayLastUsed 
        { 
            get { return dayLastUsed;  }
            
            set
            {
                dayLastUsed = value;
                DaysInactive = CalculateTimeDifference(DateTime.Now, DayLastUsed);
            }
        }
        public double DaysInactive { get; set; }

        private static int CalculateTimeDifference(DateTime timeOne, DateTime timeTwo)
        {
            TimeSpan difference = timeOne - timeTwo;
            return difference.Days;
        }

    }
}
