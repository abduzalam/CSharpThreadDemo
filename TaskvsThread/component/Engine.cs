using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskvsThread.component
{
    public class Engine
    {
        public int Horsepower { get; init; }

        public Engine(int horsepower)
        {
            Horsepower = horsepower;
        }
    }
}
