﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Drawing.Color;
using static System.Drawing.Drawing2D.SmoothingMode;
using static System.Windows.Forms.Cursors;

namespace YAN_Controls
{
    public class YANToggleButton : CheckBox
    {
        #region Fields
        private Color _onBackColor = MediumSlateBlue;
        private Color _onToggleColor = WhiteSmoke;
        private Color _offBackColor = Gray;
        private Color _offToggleColor = Gainsboro;
        private bool _solidStyle = true;
        #endregion

        #region Constructors
        public YANToggleButton() => MinimumSize = new Size(45, 22);
        #endregion

        #region Properties
        [Category("YAN Appearance"), Description("The background color of the control when the control set to on.")]
        public Color OnBackColor
        {
            get => _onBackColor;
            set
            {
                _onBackColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The color of the control when the control set to on.")]
        public Color OnToggleColor
        {
            get => _onToggleColor;
            set
            {
                _onToggleColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The background color of the control when the control set to off.")]
        public Color OffBackColor
        {
            get => _offBackColor;
            set
            {
                _offBackColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The color of the control when the control set to off.")]
        public Color OffToggleColor
        {
            get => _offToggleColor;
            set
            {
                _offToggleColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("When this property is true, the background color of the control is set to solid.")]
        [DefaultValue(true)]
        public bool SolidStyle
        {
            get => _solidStyle;
            set
            {
                _solidStyle = value;
                Invalidate();
            }
        }
        #endregion

        #region Overridden
        //prop text
        [Category("YAN Appearance")]
        public override string Text
        {
            get => base.Text;
            set { }
        }

        //on paint
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = AntiAlias;
            graphics.Clear(Parent.BackColor);
            var toggleSize = Height - 5;
            if (Checked)
            {
                //draw the control surface
                if (_solidStyle)
                {
                    graphics.FillPath(new SolidBrush(_onBackColor), GetFigurePath());
                }
                else
                {
                    graphics.DrawPath(new Pen(_onBackColor, 2), GetFigurePath());
                }
                //draw the toggle
                graphics.FillEllipse(new SolidBrush(_onToggleColor), new Rectangle(Width - Height + 1, 2, toggleSize, toggleSize));
            }
            else
            {
                //draw the control surface
                if (_solidStyle)
                {
                    graphics.FillPath(new SolidBrush(_offBackColor), GetFigurePath());
                }
                else
                {
                    graphics.DrawPath(new Pen(_offBackColor, 2), GetFigurePath());
                }
                //draw the toggle
                graphics.FillEllipse(new SolidBrush(_offToggleColor), new Rectangle(2, 2, toggleSize, toggleSize));
            }
        }

        //on mouse move
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = Hand;
        }
        #endregion

        #region Methods
        //get path of figure
        private GraphicsPath GetFigurePath()
        {
            var path = new GraphicsPath();
            path.StartFigure();
            var arcSize = Height - 1;
            path.AddArc(new Rectangle(0, 0, arcSize, arcSize), 90, 180);
            path.AddArc(new Rectangle(Width - arcSize - 2, 0, arcSize, arcSize), 270, 180);
            path.CloseFigure();
            return path;
        }
        #endregion
    }
}
