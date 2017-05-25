using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class Items
    {
        public Items(decimal cost, string type)
        {
            this.cost = cost;
            this.type = type;
        }

        private decimal cost;
        private string type;


        public string ReturnType()
        {
            return type;
        }

        public decimal GetCost()
        {
            return cost;
        }
    }
}
