using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Brush = System.Drawing.Brush;
using Color = System.Windows.Media.Color;
using Colors = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;
using JustSomeCode.Services.DrawingServices;
using System.Windows.Media;
using System.Runtime.ConstrainedExecution;

namespace JustSomeCode.Models
{
    // Scene manage layer logic. 
    public class Scene:IDisposable
    {
        #region constructor
        public Scene()
        {
            _points = new List<Point>();
            _selectedLayerIndex = -1;
            Layers = new List<Layer>();
            Color = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#E30057");
            Thickness = 5;
        }
        #endregion

        #region private fields

        private bool _pressed;
        private bool _moved;
        private Point _lastPoint;
        private Point _startPoint;
        private Point _endPoint;
        private int _selectedLayerIndex;
        private List<Point> _points;
        private Pen _pen;
        private Pen _eraserPin;
        private Brush _brush;
        private Brush _eraserBrush;
        private int _thickness;
        private Color _color;
        private Layer SelectedLayer
        {
            get
            {
                if (SelectedLayerIndex == -1)
                    return null;

                return Layers[SelectedLayerIndex];
            }
        }

        #endregion

        #region public properties
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                InvalidateBrushAndPen();
            }
        }
        public int Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
                InvalidateBrushAndPen();
            }
        }
        /// Gets or sets current selected layer index,
        /// if scene has layers, selected index should be positive, 
        /// if has no layers SelectedLayerIndex=-1
        public int SelectedLayerIndex
        {
            get { return _selectedLayerIndex; }
            set
            {
                if (HasNoLayers)
                    return;
                if (value < 0)
                    throw new ArgumentException("SelectedLayerIndex must be greater then 0");
                if (value >= Layers.Count)
                    throw new ArgumentOutOfRangeException("SelectedLayerIndex must be < layers array count");

                _selectedLayerIndex = value;
            }
        }
        public List<Layer> Layers { get; private set; }
        public bool HasNoLayers
        {
            get { return Layers.Count == 0; }
        }
        public int Mode { get;set; }

        #endregion
        
        #region public events
        //Fires if something changed in scene
        public event EventHandler SceneChanged;
        // Fires when order of layers is changed (moving layer up\down, deleting) (Disabled For Now)
        public event EventHandler LayersOrderChanged;
        #endregion

        #region public methods


        /// Export scene to PNG
        /// <param name="filename">Filename</param>
        /// <param name="size">Canvas size</param>
        public void Export(string filename, Size size)
        {
            var bmp = DrawToBitmap(size.Width, size.Height);           
            bmp.Save(filename,ImageFormat.Png);
        }

        /// Draw scene to bitmap
        /// <param name="sceneWidth">Width of bitmap</param>
        /// <param name="sceneHeight">Height of bitmap</param>
        /// <returns>Bitmap with drawed scene</returns>
        public Bitmap DrawToBitmap(int sceneWidth,int sceneHeight)
        {
            var bitmap = new Bitmap(sceneWidth, sceneHeight, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(bitmap))
            {
                gr.Clear(Colors.White);
                foreach (var layer in Layers)
                {
                    if (layer.IsVisible)
                    {
                        gr.DrawImage(layer.Bitmap, layer.Position);
                        if (layer.BufferBitmap != null)
                            gr.DrawImage(layer.BufferBitmap, layer.Position);
                    }
                }
            }
            return bitmap;
        }
        public void AddNewLayer()
        {
            var layer = new Layer();
            layer.LayerChanged += layer_LayerChanged;
            Layers.Add(layer);
            if (Layers.Count == 1)
                SelectedLayerIndex = 0;
            Invalidate();
            InvalidateLayersOrder();
        }

        /// <summary>
        /// Start user interaction logic, like MouseDown on control
        /// </summary>
        /// <param name="p">Start point</param>
        public void PressDown(Point p)
        {
            if ((HasNoLayers)||(!SelectedLayer.IsVisible))
                return;

            _pressed = true;
            _lastPoint = p;
            if (Mode == 0 || Mode == 1)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                _startPoint = p;
            }
            else if (Mode == 2)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                _startPoint = p;
            }
            else if (Mode == 5)
            {
                CheckLayerPostionAndSize(new[] {new Point(p.X, p.Y)});
                var normalized = p.Normalize(SelectedLayer.Position);
                SelectedLayer.DrawPoint(_brush, Thickness, normalized);
                _points = new List<Point>();
                _points.Add(p);
            }
            else if (Mode == 6)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                SelectedLayer.DrawPoint(_eraserBrush, Thickness, normalized);
                _points = new List<Point>();
                _points.Add(p);
            }
            
            
        }

        /// <summary>
        /// Draw line or move layer (depends on pan mode)
        /// </summary>
        /// <param name="p">Point to move</param>
        public void Move(Point p)
        {
            if ((((HasNoLayers) | (!_pressed))) || (!SelectedLayer.IsVisible))
                return;
            if (Mode == 0 || Mode == 1|| Mode == 2)
            {
                _moved = true;
            }
            else if (Mode == 5)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y), new Point(_lastPoint.X, _lastPoint.Y) });
                _points.Add(p);
                SelectedLayer.DrawLines(_pen, _points.Select(c => c.Normalize(SelectedLayer.Position)).ToArray());
            }
            else if (Mode == 6)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y), new Point(_lastPoint.X, _lastPoint.Y) });
                _points.Add(p);
                SelectedLayer.DrawLines(_eraserPin, _points.Select(c => c.Normalize(SelectedLayer.Position)).ToArray());
            }
            else if (Mode == 7)
            {
                var dx = p.X - _lastPoint.X ;
                var dy = p.Y - _lastPoint.Y;
                SelectedLayer.Offset(dx, dy);
            }
            
            
            _lastPoint = p;
        }

        /// <summary>
        /// Ends user intercation logic, like mouseup
        /// </summary>
        /// <param name="p">Last point</param>
        public void PressUp(Point p)
        {
            if ((((HasNoLayers) | (!_pressed))) || (!SelectedLayer.IsVisible))
                return;

            _pressed = false;
            if (Mode == 0)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                _endPoint = p;
                DDALinePainter painter = new DDALinePainter();
                if (_moved)
                {
                    SelectedLayer.DrawLines(_pen, painter.Draw(_startPoint, _endPoint).Points.Select(c => c.Normalize(SelectedLayer.Position)).ToArray());
                }
            }
            else if (Mode == 1)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                _endPoint = p;
                BresenhamLinePainter painter = new BresenhamLinePainter();
                if (_moved)
                {
                    SelectedLayer.DrawLines(_pen, painter.Draw(_startPoint, _endPoint).Points.Select(c => c.Normalize(SelectedLayer.Position)).ToArray());
                }
            }
            else if (Mode == 2)
            {
                CheckLayerPostionAndSize(new[] { new Point(p.X, p.Y) });
                var normalized = p.Normalize(SelectedLayer.Position);
                _endPoint = p;
                CirclePainter painter = new CirclePainter();
                List<Point> points = painter.Draw(_startPoint, _endPoint).Points;
                if (_moved)
                {
                    SelectedLayer.DrawLines(_pen, points.Select(c => c.Normalize(SelectedLayer.Position)).ToArray());
                }
            }
            else if (Mode == 7)
            {
                return;

            }


            if (SelectedLayer != null)
                SelectedLayer.Apply();
            _points.Clear();
            _moved = false;
        }

        // Dispose scene
        public void Dispose()
        {
            _pen.Dispose();
            _brush.Dispose();
            foreach (var layer in Layers)
            {
                layer.Dispose();
            }
        }

        #endregion

        #region private methods
        /// <summary>
        /// Add layers from bundles (Disabled For Now)
        /// </summary>
        /// <param name="layerBundles">Collection of layer bundles</param>
        private void LayersFromBundle(IEnumerable<LayerBundle> layerBundles)
        {
            foreach (var layerBundle in layerBundles)
            {
                var layer = new Layer(layerBundle);
                layer.LayerChanged += layer_LayerChanged;
                Layers.Add(layer);
            }
            if (Layers.Count > 0)
                SelectedLayerIndex = 0;
        }

        /// <summary>
        /// Checks that drawed point or line in layer bounds, 
        /// if not increase bounds to contains drawing
        /// </summary>
        /// <param name="points">Points of drawing</param>
        private void CheckLayerPostionAndSize(Point[] points)
        {
            if (HasNoLayers)
                return;

            var needToChangeSize = false;
            var halfThickness = Thickness/2;
            var minX = points.Min(c => c.X) - halfThickness;
            var minY = points.Min(c => c.Y) - halfThickness;
            var maxX = points.Max(c => c.X) + halfThickness;
            var maxY = points.Max(c => c.Y) + halfThickness;

            var newPosition = SelectedLayer.Position;
            var newSize = SelectedLayer.Size;

            if (minX < SelectedLayer.Position.X)
            {
                newPosition.X = minX;
                needToChangeSize = true;
            }
            if (minY < SelectedLayer.Position.Y)
            {
                newPosition.Y = minY;
                needToChangeSize = true;
            }

            if (maxX >= SelectedLayer.Position.X + SelectedLayer.Size.Width)
            {
                newSize.Width = maxX - newPosition.X;
                needToChangeSize = true;
            }

            if (maxY >= SelectedLayer.Position.Y + SelectedLayer.Size.Height)
            {
                newSize.Height = maxY - newPosition.Y;
                needToChangeSize = true;
            }

            if (needToChangeSize)
            {
                var dx = SelectedLayer.Position.X - newPosition.X;
                var dy = SelectedLayer.Position.Y - newPosition.Y;
                newSize.Width += dx;
                newSize.Height += dy;
                SelectedLayer.ChangeSizeAndPosition(newPosition, newSize);
            }
        }

        private void layer_LayerChanged(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void Invalidate()
        {
            if (SceneChanged != null)
                SceneChanged(this, new EventArgs());
        }

        private void InvalidateLayersOrder()
        {
            if (LayersOrderChanged!=null)
                LayersOrderChanged(this,new EventArgs());
        }

        private void InvalidateBrushAndPen()
        {
            if(_pen!=null)
                _pen.Dispose();
            if (_brush!=null)
                _brush.Dispose();
            _brush = new SolidBrush(ColorExtension.ToDrawingColor(_color));
            
            _eraserBrush = new SolidBrush(ColorExtension.ToDrawingColor(Color.FromRgb(255,255,255)));
            _pen = new Pen(_brush,Thickness)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
            _eraserPin = new Pen(_eraserBrush, Thickness)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
        }

        #endregion
       
    }
}
