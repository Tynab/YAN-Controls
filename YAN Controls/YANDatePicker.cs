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
        private Image _calendarIcon = pCalendarWhite;
        private RectangleF _iconButtonArea;
        private const int _calendarIconWidth = 34;
        private const int _arrowIconWidth = 17;
        private bool _droppedDown = false;
        #endregion

        #region Constructors
        public YANDatePicker()
        {
            SetStyle(UserPaint, true);
            MinimumSize = new Size(0, 35);
            Font = new Font(Font.Name, 9.5f);
            Resize += Control_Resize;
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
                _calendarIcon = _skinColor.GetBrightness() >= 0.8f ? pCalendarBlack : pCalendarWhite;
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
        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            _droppedDown = true;
        }

        //on close up
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
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
                        using (var openIconBrush = new SolidBrush(FromArgb(50, 64, 64, 64)))
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
                                        graphics.FillRectangle(openIconBrush, new RectangleF(clientArea.Width - _calendarIconWidth, 0, _calendarIconWidth, clientArea.Height));
                                    }
                                    //draw border
                                    if (_borderSize >= 1)
                                    {
                                        graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                                    }
                                    //draw icon
                                    graphics.DrawImage(_calendarIcon, Width - _calendarIcon.Width - 9, (Height - _calendarIcon.Height) / 2);
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
            var iconWidth = GetIconButtonWidth();
            _iconButtonArea = new RectangleF(Width - iconWidth, 0, iconWidth, Height);
        }

        //on mouse move
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = _iconButtonArea.Contains(e.Location) ? Hand : DefaultCursor;
        }
        #endregion

        #region Event Tokens
        //check border size and radius when resize the control
        private void Control_Resize(object sender, EventArgs e)
        {
            var minSize = Width > Height ? Height : Width;
            Miner(ref _borderSize, minSize / 2);
        }
        #endregion

        #region Methods
        //get width of icon of button
        private int GetIconButtonWidth() => MeasureText(Text, Font).Width <= Width - _calendarIconWidth - 20 ? _calendarIconWidth : _arrowIconWidth;
        #endregion
    }
}
