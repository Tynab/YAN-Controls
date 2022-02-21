using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static System.Drawing.Color;
using static System.Drawing.Drawing2D.PenAlignment;
using static System.Windows.Forms.ControlStyles;
using static System.Windows.Forms.Cursors;
using static System.Windows.Forms.TextRenderer;
using static YAN_Controls.Properties.Resources;
using static YAN_Controls.Scipts.Method;

namespace YAN_Controls
{
    public class YANDatePicker : DateTimePicker
    {
        #region Fields
        private Color _skinColor = MediumSlateBlue;
        private Color _textColor = White;
        private Color _borderColor = PaleVioletRed;
        private int _borderSize = 0;
        private Image _calIc = pCalendarWhite;
        private RectangleF _icBtnArea;
        private const int _wCalIc = 34;
        private const int _wArrowIc = 17;
        private bool _droppedDown = false;
        #endregion

        #region Constructors
        public YANDatePicker()
        {
            SetStyle(UserPaint, true);
            MinimumSize = new Size(0, 35);
            Font = new Font(Font.Name, 9.5f);
            Resize += Ctrl_Resize;
        }
        #endregion

        #region Properties
        [Category("YAN Appearance"), Description("The background color of the component.")]
        public Color SkinColor
        {
            get => _skinColor;
            set
            {
                _skinColor = value;
                _calIc = _skinColor.GetBrightness() >= 0.8f ? pCalendarBlack : pCalendarWhite;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The foreground color of this component, which is used to display text.")]
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                Invalidate();
            }
        }

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
        #endregion

        #region Overridden
        //on drop down
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            _droppedDown = true;
        }

        //on close up
        protected override void OnCloseUp(EventArgs e)
        {
            base.OnCloseUp(e);
            _droppedDown = false;
        }

        //on key press
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }

        //on paint
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var graphics = CreateGraphics())
            {
                using (var penBorder = new Pen(_borderColor, _borderSize))
                {
                    using (var skinBrush = new SolidBrush(_skinColor))
                    {
                        using (var openIcBrush = new SolidBrush(FromArgb(50, 64, 64, 64)))
                        {
                            using (var textBrush = new SolidBrush(_textColor))
                            {
                                using (var textFormat = new StringFormat())
                                {
                                    penBorder.Alignment = Inset;
                                    textFormat.LineAlignment = StringAlignment.Center;
                                    var clientArea = new RectangleF(0, 0, Width - 0.5f, Height - 0.5f);
                                    //draw surface
                                    graphics.FillRectangle(skinBrush, clientArea);
                                    //draw text
                                    graphics.DrawString("   " + Text, Font, textBrush, clientArea, textFormat);
                                    //draw open calendar icon highlight
                                    if (_droppedDown)
                                    {
                                        graphics.FillRectangle(openIcBrush, new RectangleF(clientArea.Width - _wCalIc, 0, _wCalIc, clientArea.Height));
                                    }
                                    //draw border
                                    if (_borderSize >= 1)
                                    {
                                        graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                                    }
                                    //draw icon
                                    graphics.DrawImage(_calIc, Width - _calIc.Width - 9, (Height - _calIc.Height) / 2);
                                }
                            }
                        }
                    }
                }
            }
        }

        //on handle created
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            var wIc = GetWIcBtn();
            _icBtnArea = new RectangleF(Width - wIc, 0, wIc, Height);
        }

        //on mouse move
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = _icBtnArea.Contains(e.Location) ? Hand : DefaultCursor;
        }
        #endregion

        #region Event Handles
        //check border size and radius when resize the control
        private void Ctrl_Resize(object sender, EventArgs e)
        {
            var minSize = Width > Height ? Height : Width;
            Miner(ref _borderSize, minSize / 2);
        }
        #endregion

        #region Methods
        //get width of icon of button
        private int GetWIcBtn() => MeasureText(Text, Font).Width <= Width - _wCalIc - 20 ? _wCalIc : _wArrowIc;
        #endregion
    }
}
