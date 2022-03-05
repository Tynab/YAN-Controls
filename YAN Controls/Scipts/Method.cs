using System;
using System.Runtime.InteropServices;

namespace YAN_Controls.Scipts
{
    internal class Method
    {
        #region Common
        /// <summary>
        /// Tìm giá trị nhỏ nhất.
        /// </summary>
        /// <param name="list">Chuỗi dữ liệu so sánh.</param>
        /// <returns>Giá trị nhỏ nhất.</returns>
        internal static T Miner<T>(params T[] list)
        {
            dynamic res = list[0];
            foreach (var item in list)
            {
                if (item < res)
                {
                    res = item;
                }
            }
            return res;
        }

        /// <summary>
        /// Tìm giá trị lớn nhất.
        /// </summary>
        /// <param name="list">Chuỗi dữ liệu so sánh.</param>
        /// <returns>Giá trị lớn nhất.</returns>
        internal static T Maxer<T>(params T[] list)
        {
            dynamic res = list[0];
            foreach (var item in list)
            {
                if (item > res)
                {
                    res = item;
                }
            }
            return res;
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
        /// Điều khiển object với animation đồng bộ.
        /// </summary>
        /// <param name="hwand">Object.</param>
        /// <param name="dwTime">Thời gian tính bằng milisecond.</param>
        /// <param name="dwFlags">Flag animate.</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern void AnimateWindow(IntPtr hwand, int dwTime, AnimateWindowFlags dwFlags);
        #endregion

        #region Ellipse Form
        /// <summary>
        /// Tạo khung ellipse cho form.
        /// </summary>
        /// <param name="nLRect">Tọa độ trái.</param>
        /// <param name="nTRect">Tọa độ trên.</param>
        /// <param name="nRRect">Tọa độ phải.</param>
        /// <param name="nBRect">Tọa độ dưới.</param>
        /// <param name="nWEllipse">Độ rộng.</param>
        /// <param name="nHElippse">Độ cao.</param>
        /// <returns>Platform specific.</returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        internal static extern IntPtr CreateRoundRectRgn(int nLRect, int nTRect, int nRRect, int nBRect, int nWEllipse, int nHElippse);
        #endregion
    }
}
