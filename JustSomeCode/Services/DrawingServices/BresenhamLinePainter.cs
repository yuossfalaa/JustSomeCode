using JustSomeCode.Models;
using JustSomeCode.Models.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JustSomeCode.Services.DrawingServices
{
    public class BresenhamLinePainter : CommonDrawingOperations, IDraw
    {
        public PointList Draw(Point Start, Point End)
        {
            Bresenham_Line bresenham_Line = new Bresenham_Line();
            List<Point> points = new List<Point>();

            //Modified algorithm for all Cases
            //http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/

            int x = Start.X;
            int y = Start.Y;
            int w = End.X - Start.X;
            int h = End.Y - Start.Y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            //Notice the line numerator = longest >> 1 :  Technically it means numerator is equal to half of longest,
            //and is important to avoid y from being rounded at every whole number instead of halfway point.
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                points.Add(new Point (x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }


            bresenham_Line.Points= points;
            return bresenham_Line;
        }
    }
}

//Old Algorithm
//Work Only in Certain Cases

//int m_new = 2 * (End.Y - Start.Y); // dy
//int slope_error_new = m_new - (End.X - Start.X); // 2dy-dx
//for (int x = Start.X, y = Start.Y; x <= End.X; x++)
//{
//    points.Add(new Point(x, y));
//    slope_error_new += m_new;
//    if (slope_error_new >= 0)
//    {
//        y++;
//        slope_error_new -= 2 * (End.X - Start.X);
//    }
//}
