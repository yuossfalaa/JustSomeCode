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

            double rx,ry,xc,yc;
            xc = (Start.X + End.X) / 2;
            yc = (Start.Y + End.Y) / 2;
            rx = Math.Sqrt(Math.Pow(xc - Start.X, 2) + Math.Pow(Start.Y - Start.Y, 2));
            ry = Math.Sqrt(Math.Pow(Start.X - Start.X, 2) + Math.Pow(Start.Y - yc, 2));
            double dx, dy, d1, d2, x, y;
            x = 0;
            y = ry;

            d1 = (ry * ry) - (rx * rx * ry) + (0.25f * rx * rx);
            dx = 2 * ry * ry * x;
            dy = 2 * rx * rx * y;

            while (dx < dy)
            {
                if (d1 < 0)
                {
                    x++;
                    dx = dx + (2 * ry * ry);
                    d1 = d1 + dx + (ry * ry);
                }
                else
                {
                    x++;
                    y--;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d1 = d1 + dx - dy + (ry * ry);
                }

                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc - y )));
                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc - y )));

               

            }

            d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f)))
                + ((rx * rx) * ((y - 1) * (y - 1)))
                - (rx * rx * ry * ry);

            while (y >= 0)
            {

                if (d2 > 0)
                {
                    y--;
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + (rx * rx) - dy;
                }
                else
                {
                    y--;
                    x++;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + dx - dy + (rx * rx);
                }

                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round( yc - y )));
                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc - y )));


                
            }
            

            ellipse.Points = points.OrderBy(x => Math.Atan2(x.X - xc, x.Y - yc)).ToList();
            return ellipse;
        }
    }
}

