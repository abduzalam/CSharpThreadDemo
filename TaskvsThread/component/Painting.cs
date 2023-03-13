using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskvsThread.component
{
    public class Painting
    {
        public string Color { get; init; }
        public int PaintArea { get; init; }

        public Painting(string color, int paintArea)
        {
            Color = color;
            PaintArea = paintArea;
        }
    }
}
