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
        ///<summary>
        ///Algorithm From :  
        ///<seealso cref="https://www.geeksforgeeks.org/midpoint-ellipse-drawing-algorithm/"/>
        ///</summary>
        public PointList Draw(Point Start, Point End)
        {
            Ellipse ellipse = new Ellipse();
            List<Point> points = new List<Point>();

            double rx = Start.X,ry = Start.Y,xc = End.X,yc = End.Y;
            double dx, dy, d1, d2, x, y;
            x = 0;
            y = ry;

            // Initial decision parameter of region 1
            d1 = (ry * ry) - (rx * rx * ry) + (0.25f * rx * rx);
            dx = 2 * ry * ry * x;
            dy = 2 * rx * rx * y;

            // For region 1
            while (dx < dy)
            {

                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc - y )));
                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc - y )));

                // Checking and updating value of
                // decision parameter based on algorithm
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
            }

            // Decision parameter of region 2
            d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f)))
                + ((rx * rx) * ((y - 1) * (y - 1)))
                - (rx * rx * ry * ry);

            // Plotting points of region 2
            while (y >= 0)
            {



                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round(yc + y )));
                points.Add(new Point((int)Math.Round(xc - x ), (int)Math.Round( yc - y )));
                points.Add(new Point((int)Math.Round(xc + x ), (int)Math.Round(yc - y )));

                // Checking and updating parameter
                // value based on algorithm
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
            }


            ellipse.Points = points.OrderBy(x => Math.Atan2(x.X - Start.X, x.Y - Start.Y)).ToList();
            return ellipse;
        }
    }
}

