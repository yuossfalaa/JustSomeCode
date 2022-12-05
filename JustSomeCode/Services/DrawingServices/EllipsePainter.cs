using JustSomeCode.Models;
using JustSomeCode.Models.Shapes;
using System.Collections.Generic;
using System.Drawing;

namespace JustSomeCode.Services.DrawingServices
{
    public class EllipsePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            Ellipse ellipse = new Ellipse();
            List<Point> points = new List<Point>();


            ellipse.Points = points;
            return ellipse;
        }
    }
}
