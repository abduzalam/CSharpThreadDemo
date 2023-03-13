using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskvsThread.component
{
    public class Suspension
    {
        public int SupportedKg { get; init; }

        public Suspension(int supportedKg)
        {
            SupportedKg = supportedKg;
        }
    }
}
