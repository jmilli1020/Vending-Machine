using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class Change
    {
        public string GetChange(decimal amountPaid, decimal amountDue)
        {
            
            decimal change = amountPaid - amountDue;
            int numberOfQs = 0;
            int changeAfterQs = 0;
            int numberOfDs = 0;
            int changeAfterDs = 0;
            int numberOfNs = 0;
            int numberOfPs = 0;
            int totalCents = decimal.ToInt32(change * 100);

            if (totalCents >= 25)
            {
                numberOfQs = totalCents / 25;
                changeAfterQs = totalCents % 25;
                numberOfDs = changeAfterQs / 10;
                changeAfterDs = changeAfterQs % 10;
                numberOfNs = changeAfterDs / 5;
                numberOfPs = changeAfterDs % 5;
            }
            else if (totalCents >= 10)
            {
                numberOfDs = totalCents / 10;
                changeAfterDs = totalCents % 10;
                numberOfNs = changeAfterDs / 5;
                numberOfPs = changeAfterDs % 5;
            }
            else if (totalCents >= 5)
            {
                numberOfNs = totalCents / 5;
                numberOfPs = totalCents % 5;
            }
            return $"Your change is { numberOfQs } quarters, {numberOfDs} dimes, {numberOfNs} nickels";

        }
    }
}
