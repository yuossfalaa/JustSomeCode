using JustSomeCode.Models.Shapes;
using System.Collections.Generic;
using System.Drawing;

namespace JustSomeCode.Services
{
    public interface IDraw
    {
        public PointList Draw(Point Start, Point End);  

    }
}
