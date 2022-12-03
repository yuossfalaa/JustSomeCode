using System.Collections.Generic;
using System.Drawing;

namespace JustSomeCode.Services
{
    public interface IDraw
    {
        public List <Point> Draw(Point point, System.Windows.Media.Color color);
        public List <Point> Draw(Point point, double diameter, System.Windows.Media.Color color);
        public List <Point> Draw(Point point, Size size, System.Windows.Media.Color color);
  

    }
}
