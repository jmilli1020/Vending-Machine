using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.Classes
{
    class VendingMachineFileWriter
    {
        public VendingMachineFileWriter()
        {
            string directory = Environment.CurrentDirectory;
            string file = "Log.txt";
            this.filepath = Path.Combine(directory, file);
        }

        private string filepath;

        public void WriteToLog(string actionPerformed, string startingBalance, string endingBalance)
        {
            using (StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.WriteLine(DateTime.UtcNow + " " + actionPerformed + "  " + startingBalance.PadRight(10) + endingBalance);
            }
        }
    }
}
