using System;
using System.Runtime.InteropServices;

namespace YAN_Controls.Scipts
{
    internal class Method
    {
        #region Common
        /// <summary>
        /// Find min value.
        /// </summary>
        /// <param name="val">Current value.</param>
        /// <param name="lim">Check value.</param>
        internal static void Miner(ref int val, int lim)
        {
            if (val > lim)
            {
                val = lim;
            }
        }

        /// <summary>
        /// Find max value.
        /// </summary>
        /// <param name="val">Current value.</param>
        /// <param name="lim">Check value.</param>
        internal static void Maxer(ref int val, int lim)
        {
            if (val < lim)
            {
                val = lim;
            }
        }
        #endregion

        #region Animate
        internal enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }

        /// <summary>
        /// Low animation sync.
        /// </summary>
        /// <param name="hwand">Control.</param>
        /// <param name="dwTime">Timer.</param>
        /// <param name="dwFlags">Animation type.</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern void AnimateWindow(IntPtr hwand, int dwTime, AnimateWindowFlags dwFlags);
        #endregion

        #region Ellipse Form
        /// <summary>
        /// Mod form round ellipse.
        /// </summary>
        /// <param name="nLRect">Left path.</param>
        /// <param name="nTRect">Top path.</param>
        /// <param name="nRRect">Right path.</param>
        /// <param name="nBRect">Bot path.</param>
        /// <param name="nWEllipse">Width path.</param>
        /// <param name="nHElippse">Height path.</param>
        /// <returns>Platform specific.</returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        internal static extern IntPtr CreateRoundRectRgn(int nLRect, int nTRect, int nRRect, int nBRect, int nWEllipse, int nHElippse);
        #endregion
    }
}
