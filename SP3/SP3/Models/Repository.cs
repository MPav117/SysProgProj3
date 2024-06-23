using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP3.Models
{
    public class Repository
    {
        public required string Name { get; set; }
        public required string URL { get; set; }
        public int Size { get; set; }
        public int NumForks { get; set; }
        public int NumStars { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}\nUrl: {URL}\nSize: {Size} kB\nNumber of Forks: {NumForks}\nNumber of Stars: {NumStars}\n";
        }
    }
}
