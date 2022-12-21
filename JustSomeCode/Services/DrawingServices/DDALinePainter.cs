using JustSomeCode.Models;
using JustSomeCode.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace JustSomeCode.Services.DrawingServices
{
    public class DDALinePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            List<Point> points = new List<Point>();
            DDA_Line dda_Line= new DDA_Line();

            int dx = End.X - Start.X , dy = End.Y - Start.Y ;
            int Distance = Math.Max(Math.Abs(dx), Math.Abs(dy));
            if (Distance > 0 )
            {
                float dxdt =(float)dx / Distance, dydt = (float)dy / Distance;
                float x = Start.X, y = Start.Y;
                while (Distance != 0)
                {
                    points.Add(new Point((int)Math.Round(x), (int)Math.Round(y)));
                    x += dxdt;
                    y += dydt;
                    Distance--;
                }
                dda_Line.Points = points;

            }
            else
            {
                points.Add(Start);
                dda_Line.Points = points;
            }
           
            return dda_Line;
        }
    }
}
