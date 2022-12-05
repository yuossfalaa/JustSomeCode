using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace JustSomeCode.Models
{
    //Made By Sergey Voyteshonok
    //https://github.com/SVoyt
    //Edited By Youssef Alaa 
    //https://github.com/yuossfalaa

    // Layer class. Contains drawing logic

    public class Layer : IDisposable
    {
        #region private fields

        private bool _isVisible = true;
        private Point _position;
        private Size _size;
        private Bitmap _bitmap;
        private Bitmap _bufferBitmap;

        #endregion
        #region public properties
        // Gets or sets visibility of the layer
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                Invalidate();
            }
        }
        //Gets or sets layer position
        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Invalidate();
            }
        }
        // Gets layer size
        public Size Size
        {
            get { return _size; }
            private set
            {
                _size = value;
                Invalidate();
            }
        }
        // Gets bitmap with already drawed figures, 
        // does not contain now drawing figure
        public Bitmap Bitmap
        {
            get { return _bitmap; }
            private set
            {
                if (_bitmap!=null)
                    _bitmap.Dispose();
                _bitmap = value;
            }
        }
        // Gets bitmap of now drawing figure
        public Bitmap BufferBitmap
        {
            get { return _bufferBitmap; }
            private set
            {
                if (_bufferBitmap != null)
                    _bufferBitmap.Dispose();
                _bufferBitmap = value;
            }
        }

        #endregion
        // Fires when something changed in layer 
        public event EventHandler LayerChanged;

        #region constructors
        // Creates layer with 1 on 1 canvas
        public Layer()
        {
            Size = new Size(1,1);
            Bitmap = new Bitmap(Size.Width, Size.Height, PixelFormat.Format32bppArgb);            
        }
        // Creates layer from layer bundle
        /// <param name="layerBundle">layer bundle</param>
        public Layer(LayerBundle layerBundle)
        {
            if (layerBundle==null)
                throw new ArgumentNullException("layerBundle");
            Bitmap = layerBundle.Bitmap;
            Position = layerBundle.Position;
            Size = layerBundle.Size;
            IsVisible = layerBundle.IsVisible;
        }

        #endregion

        #region public methods
        // Changes poisition and size of layer
        /// <param name="newPosition">New position</param>
        /// <param name="newSize">New size</param>
        public void ChangeSizeAndPosition(Point newPosition, Size newSize)
        {
            var newBitmap = new Bitmap(newSize.Width,newSize.Height,PixelFormat.Format32bppArgb);

            using (var gr = Graphics.FromImage(newBitmap))
            {
                var dx = Position.X - newPosition.X;
                var dy = Position.Y - newPosition.Y;
                if (dx < 0)
                    dx = 0;
                if (dy < 0)
                    dy = 0;
                gr.DrawImage(Bitmap,dx,dy);
            }
            Bitmap = newBitmap;
            Position = newPosition;
            Size = newSize;
            Invalidate();
        }

        /// Draw lines by points
        /// To BE EDITED
        /// <param name="pen">Pen to draw</param>
        /// <param name="points">Lines points</param>
        public void DrawLines(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            BufferBitmap = new Bitmap(Bitmap.Width, Bitmap.Height, PixelFormat.Format32bppArgb);

            using (var gr = Graphics.FromImage(BufferBitmap))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.DrawLines(pen, points);
            }

            //Invalidate();
        }

        /// <summary>
        /// Draw point 
        /// </summary>
        /// <param name="brush">Brush to draw</param>
        /// <param name="thickness">Diameter of point</param>
        /// <param name="p">Point</param>
        public void DrawPoint(Brush brush, float thickness, Point p)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            BufferBitmap = new Bitmap(Bitmap.Width,Bitmap.Height,PixelFormat.Format32bppArgb);

            using (var gr = Graphics.FromImage(BufferBitmap))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                var rect = new RectangleF(p.X - thickness/2, p.Y - thickness/2, thickness, thickness);
                gr.FillEllipse(brush, rect);
            }

            Invalidate();
        }

        // Applies now drawing figure to Bitmap
        public void Apply()
        {
            using (var gr = Graphics.FromImage(Bitmap))
            {
                if (BufferBitmap != null)
                    gr.DrawImage(BufferBitmap,0,0);
            }
            BufferBitmap = null;
            Invalidate();
        }

        /// Offset layer
        /// <param name="dx">X offset (adds to x)</param>
        /// <param name="dy">Y offset (adds to y)</param>
        public void Offset(int dx, int dy)
        {
            _position.X += dx;
            _position.Y += dy;
            Invalidate();
        }

        /// Invalidating layer
        public void Invalidate()
        {
            if (LayerChanged!=null)
                LayerChanged(this,new EventArgs());
        }

        /// Disposing layer ( dispose bitmaps )
        public void Dispose()
        {
            Bitmap = null;
            BufferBitmap = null;
        }

        #endregion
    }
}
