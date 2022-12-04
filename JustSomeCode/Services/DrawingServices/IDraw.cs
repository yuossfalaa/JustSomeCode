using System.Collections.Generic;
using System.Drawing;

namespace JustSomeCode.Services
{
    public interface IDraw
    {
        public List <Point> Draw(Point Start);
        public List <Point> Draw(Point Start, Point End);
        public List <Point> Draw(Point Start, Size size);
  

    }
}
