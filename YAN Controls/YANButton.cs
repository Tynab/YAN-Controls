using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Drawing.Color;
using static System.Drawing.Drawing2D.PenAlignment;
using static System.Drawing.Drawing2D.SmoothingMode;
using static System.Drawing.Rectangle;
using static System.Windows.Forms.ControlStyles;
using static System.Windows.Forms.Cursors;
using static System.Windows.Forms.FlatStyle;
using static YAN_Controls.Scipts.Method;

namespace YAN_Controls
{
    public class YANButton : Button
    {
        #region Fields
        private Color _borderColor = PaleVioletRed;
        private int _borderSize = 0;
        private int _borderRadius = 20;
        #endregion

        #region Constructors
        public YANButton()
        {
            SetStyle(Selectable, false);
            TabStop = false;
            TabIndex = 0;
            FlatStyle = Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            BackColor = MediumSlateBlue;
            ForeColor = White;
            Resize += Ctrl_Resize;
        }
        #endregion

        #region Properties
        [Category("YAN Appearance"), Description("This property specifies the color of the border around the control.")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("This property specifies the size, in pixels, of the border around the control.")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                if (value >= 0 && value <= Width / 2 && value <= Height / 2)
                {
                    _borderSize = value;
                    Invalidate();
                }
            }
        }

        [Category("YAN Appearance"), Description("This property allows you to add rounded corners to the control.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                if (value >= 0 && value <= Width / 2 && value <= Height / 2)
                {
                    _borderRadius = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region Overridden
        //on paint
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            var graphics = pevent.Graphics;
            var rectSurface = ClientRectangle;
            if (_borderRadius > 2)
            {
                using (var pathSurface = GetFigurePath(rectSurface, _borderRadius))
                {
                    using (var pathBorder = GetFigurePath(Inflate(rectSurface, -_borderSize, -_borderSize), _borderRadius - _borderSize))
                    {
                        using (var penSurface = new Pen(Parent.BackColor, _borderSize > 0 ? _borderSize : 2))
                        {
                            using (var penBorder = new Pen(_borderColor, _borderSize))
                            {
                                graphics.SmoothingMode = AntiAlias;
                                Region = new Region(pathSurface);
                                //draw surface
                                graphics.DrawPath(penSurface, pathSurface);
                                //draw border
                                if (_borderSize >= 1)
                                {
                                    graphics.DrawPath(penBorder, pathBorder);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                graphics.SmoothingMode = None;
                Region = new Region(rectSurface);
                //draw border
                if (_borderSize >= 1)
                {
                    using (var penBorder = new Pen(_borderColor, _borderSize))
                    {
                        penBorder.Alignment = Inset;
                        graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }

        //on handle created
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += Container_BackClChanged;
        }

        //on mouse move
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = Hand;
        }
        #endregion

        #region Event Tokens
        //background color changed
        private void Container_BackClChanged(object sender, EventArgs e) => Invalidate();

        //check border size and radius when resize the control
        private void Ctrl_Resize(object sender, EventArgs e)
        {
            var minSize = Width > Height ? Height : Width;
            Miner(ref _borderRadius, minSize / 2);
            Miner(ref _borderSize, minSize / 2);
        }
        #endregion

        #region Methods
        //get path of figure
        private GraphicsPath GetFigurePath(RectangleF rectF, float rad)
        {
            var path = new GraphicsPath();
            var curveSize = rad * 2f;
            path.StartFigure();
            path.AddArc(rectF.X, rectF.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rectF.Right - curveSize, rectF.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rectF.Right - curveSize, rectF.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rectF.X, rectF.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion
    }
}
