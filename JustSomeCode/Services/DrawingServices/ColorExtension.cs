﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JustSomeCode.Services.DrawingServices
{
    public static class ColorExtension
    {
        /// <summary>
        /// Convert Media Color (WPF) to Drawing Color (WinForm)
        /// </summary>
        /// <param name="mediaColor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color mediaColor)
        {
            return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
        }

        /// <summary>
        /// Convert Drawing Color (WPF) to Media Color (WinForm)
        /// </summary>
        /// <param name="drawingColor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color drawingColor)
        {
            return System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }
    }
}
