using JustSomeCode.Models;
using JustSomeCode.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JustSomeCode.Services.DrawingServices
{
    public class CirclePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            List<Point> points = new List<Point>();
            Circle circle = new Circle();
            double radius = Math.Sqrt(Math.Pow(End.X - Start.X, 2) + Math.Pow(End.Y - Start.Y, 2));
            int x = (int)Math.Round(radius), y = 0;
            double p = 1- radius;

            while (x >= y)
            {
                y++;
                if (p <= 0)
                {
                    p = p + (2 * y) + 1;
                }
                else
                {
                    x--;
                    p = p + (2 * y) - (2 * x) + 1;
                }
                points.Add(new Point(x+Start.X,y+Start.Y));
                points.Add(new Point(-x + Start.X, y + Start.Y));
                points.Add(new Point(x + Start.X, -y +Start.Y));
                points.Add(new Point(-x + Start.X, -y + Start.Y));
                points.Add(new Point(y + Start.X, x + Start.Y));
                points.Add(new Point(-y + Start.X, x + Start.Y));
                points.Add(new Point(y + Start.X, -x + Start.Y));
                points.Add(new Point(-y + Start.X, -x + Start.Y));
            }

            circle.Points = points.OrderBy(x => Math.Atan2(x.X-Start.X, x.Y-Start.Y)).ToList(); 
            return circle;
        }
      
    }
    
}