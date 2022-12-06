using JustSomeCode.Models;
using JustSomeCode.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JustSomeCode.Services.DrawingServices
{
    public class EllipsePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            Ellipse ellipse = new Ellipse();
            List<Point> points = new List<Point>();
          
            int xc = Start.X, yc = Start.Y, Rx = End.X, Ry = End.Y;

            int Rx2 = Rx * Rx;
            int Ry2 = Ry * Ry;

            int twoRx2 = 2 * Rx2;
            int twoRy2 = 2 * Ry2;

            double p;
            int x = 0, y = Ry, dx = twoRy2 * x, dy = twoRx2 * y;

            p = Math.Pow(Ry, 2) - (Math.Pow(Rx, 2) * Ry) + (Math.Pow(Rx, 2) * 0.25);
            do
            {
                x++;
                dx = twoRy2 * x;

                if (p < 0)
                    p += Ry2 + dx;

                else
                {
                    y--;
                    dy = twoRx2 * y;
                    p += Ry2 + dx - dy;
                }

                points.Add(new Point(xc + x+Start.X, yc + y+Start.Y));
                points.Add(new Point(xc - x+ Start.X, yc + y+Start.Y));
                points.Add(new Point(xc - x+Start.X, yc - y+Start.Y));
                points.Add(new Point(xc + x+Start.X, yc - y+Start.Y));

            } while (dx < dy);


            //region two
            p = (int)Math.Round((y - 1) * Rx2 * (y - 1) + Ry2 * (x + .5) * (x + .5) - Rx2 * Ry2);
            while (y > 0)
            {
                y--;
                dy = twoRx2 * y;
                if (p > 0)
                {
                    p += Rx2 - dy;
                }
                else
                {
                    x++;
                    dx = twoRy2 * x;
                    p += Rx2 - dy + dx;
                }

                points.Add(new Point(xc + x+Start.X, yc + y+Start.Y));
                points.Add(new Point(xc - x+Start.X, yc + y+Start.Y));
                points.Add(new Point(xc - x+Start.X, yc - y+Start.Y));
                points.Add(new Point(xc + x+Start.X, yc - y+Start.Y));




                
            }
            ellipse.Points = points.OrderBy(x => Math.Atan2(x.X - Start.X, x.Y - Start.Y)).ToList();
            return ellipse;
        }
    }
}
