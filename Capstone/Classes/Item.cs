using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class Item
    {
        public Item(decimal cost, string productName, string type)
        {
            this.cost = cost;
            this.productName = productName;
            this.type = type;
        }

        private decimal cost;
        private string productName;
        private string type;


        public string ReturnType()
        {
            return type;
        }

        public string GetProductName()
        {
            return productName;
        }

        public decimal GetCost()
        {
            return cost;
        }
    }
}
