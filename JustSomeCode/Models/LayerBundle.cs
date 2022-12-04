using System;
using System.Drawing;

namespace JustSomeCode.Models
{
    // Layer bundle. Short data from Layer object to serializing.
    [Serializable]
    public class LayerBundle
    {
        public Bitmap Bitmap { get; set; }
        public bool IsVisible { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
    }
}
