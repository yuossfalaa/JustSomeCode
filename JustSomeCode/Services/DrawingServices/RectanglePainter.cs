using JustSomeCode.Models.Shapes;
using JustSomeCode.Models;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace JustSomeCode.Services.DrawingServices
{
    public class RectanglePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            BresenhamLinePainter Line1= new BresenhamLinePainter();
            BresenhamLinePainter Line2= new BresenhamLinePainter();
            BresenhamLinePainter Line3= new BresenhamLinePainter();
            BresenhamLinePainter Line4= new BresenhamLinePainter();


            List<Point> points = 
                Line1.Draw(Start,new Point(End.X,Start.Y)).Points
                .Concat(Line2.Draw(Start, new Point(Start.X,End.Y)).Points)
                .Concat(Line3.Draw(new Point(Start.X, End.Y), End).Points)
                .Concat(Line4.Draw(End, new Point(End.X, Start.Y)).Points)
                .ToList();

            Models.Rectangle rectangle= new Models.Rectangle();
            rectangle.Points = points;
            return rectangle;

        }
    }
}
