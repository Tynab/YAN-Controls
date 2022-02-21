using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Drawing.Color;
using static System.Drawing.Drawing2D.PenAlignment;
using static System.Drawing.Drawing2D.SmoothingMode;
using static System.Drawing.Rectangle;
using static System.Windows.Forms.TextRenderer;
using static YAN_Controls.Scipts.Method;

namespace YAN_Controls
{
    [DefaultEvent("ValChanged")]
    public partial class YANNumBox : UserControl
    {
        #region Fields
        private Color _borderColor = MediumSlateBlue;
        private Color _borderFocusColor = HotPink;
        private int _borderSize = 2;
        private int _borderRadius = 0;
        private bool _underlinedStyle = false;
        private bool _isFocus = false;
        #endregion

        #region Constructors
        public YANNumBox()
        {
            InitializeComponent();
            Txt = Val.ToString();
            Resize += Ctrl_Resize;
        }
        #endregion

        #region Properties
        public string Txt;

        [Category("YAN Appearance"), Description("Indicates how the text should be aligned for edit controls.")]
        public HorizontalAlignment TextAlign
        {
            get => numericUpDownMain.TextAlign;
            set
            {
                numericUpDownMain.TextAlign = value;
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

        [Category("YAN Appearance"), Description("This property specifies the color of the border around the control when the control have the focus.")]
        public Color BorderFocusColor { get => _borderFocusColor; set => _borderFocusColor = value; }

        [Category("YAN Appearance"), Description("Indicates the minimum value for the numeric up-down control.")]
        public decimal Minimum
        {
            get => numericUpDownMain.Minimum;
            set
            {
                if (value > Val)
                {
                    Val = value;
                    Txt = Val.ToString();
                    numericUpDownMain.Value = value;
                }
                numericUpDownMain.Minimum = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("Indicates the maximum value for the numeric up-down control.")]
        public decimal Maximum
        {
            get => numericUpDownMain.Maximum;
            set
            {
                if (value < Val)
                {
                    Val = value;
                    Txt = Val.ToString();
                    numericUpDownMain.Value = value;
                }
                numericUpDownMain.Maximum = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The current value of the numeric up-down control.")]
        public decimal Val
        {
            get => numericUpDownMain.Value;
            set
            {
                numericUpDownMain.Value = value < Minimum ? Minimum : value > Maximum ? Maximum : value;
                Txt = value.ToString();
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("indicates the amount to increment or decrement on each button click.")]
        public decimal Increment { get => numericUpDownMain.Increment; set => numericUpDownMain.Increment = value; }

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

        [Category("YAN Appearance"), Description("Indicates the number of decimal places to display.")]
        public int DecimalPlaces
        {
            get => numericUpDownMain.DecimalPlaces;
            set
            {
                numericUpDownMain.DecimalPlaces = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("When this property is true, the underline added to text.")]
        public bool UnderlinedStyle
        {
            get => _underlinedStyle;
            set
            {
                _underlinedStyle = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")]
        public bool ThousandsSeparator
        {
            get => numericUpDownMain.ThousandsSeparator;
            set
            {
                numericUpDownMain.ThousandsSeparator = value;
                Invalidate();
            }
        }

        //event
        [Category("YAN Event"), Description("Event raised when the value of the Val property is changed on Control.")]
        public event EventHandler ValChanged;
        #endregion

        #region Overridden
        //background color
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                numericUpDownMain.BackColor = value;
            }
        }

        //foreground color
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                numericUpDownMain.ForeColor = value;
            }
        }

        //font display
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                numericUpDownMain.Font = value;
                if (DesignMode)
                {
                    UpdateHCtrl();
                }
            }
        }

        //on paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            if (_borderRadius > 1)
            {
                var rectBorderSmooth = ClientRectangle;
                var smoothSize = _borderSize > 0 ? _borderSize : 1;
                using (var pathBorderSmooth = GetFigurePath(rectBorderSmooth, _borderRadius))
                {
                    using (var pathBorder = GetFigurePath(Inflate(rectBorderSmooth, -_borderSize, -_borderSize), _borderRadius - _borderSize))
                    {
                        using (var penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
                        {
                            using (var penBorder = new Pen(_borderColor, _borderSize))
                            {
                                Region = new Region(pathBorderSmooth);
                                if (_borderRadius > 15)
                                {
                                    SetTxtRoundedRegion();
                                }
                                graphics.SmoothingMode = AntiAlias;
                                penBorder.Alignment = Center;
                                if (_isFocus)
                                {
                                    penBorder.Color = _borderFocusColor;
                                }
                                if (_underlinedStyle)
                                {
                                    //draw border smoothing
                                    graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                                    //draw border
                                    graphics.SmoothingMode = None;
                                    graphics.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                                }
                                else
                                {
                                    //draw border smoothing
                                    graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                                    //draw border
                                    graphics.DrawPath(penBorder, pathBorder);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (var penBorder = new Pen(_borderColor, _borderSize))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = Inset;
                    if (_isFocus)
                    {
                        penBorder.Color = _borderFocusColor;
                    }
                    if (_underlinedStyle)
                    {
                        graphics.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    }
                    else
                    {
                        graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5f, Height - 0.5f);
                    }
                }
            }
        }

        //on resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (DesignMode)
            {
                UpdateHCtrl();
            }
        }

        //on load
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateHCtrl();
        }
        #endregion

        #region Events
        //raises the value changed event
        private void NumericUpDownMain_ValueChanged(object sender, EventArgs e)
        {
            if (ValChanged != null)
            {
                ValChanged.Invoke(sender, e);
            }
        }

        //raises the key press event
        private void NumericUpDownMain_KeyPress(object sender, KeyPressEventArgs e) => OnKeyPress(e);

        //raises the key down event
        private void NumericUpDownMain_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        //raises the enter event
        private void NumericUpDownMain_Enter(object sender, EventArgs e)
        {
            _isFocus = true;
            Invalidate();
        }

        //raises the leave event
        private void NumericUpDownMain_Leave(object sender, EventArgs e)
        {
            _isFocus = false;
            if (string.IsNullOrWhiteSpace(numericUpDownMain.Text))
            {
                numericUpDownMain.Text = numericUpDownMain.Value.ToString();
            }
            Invalidate();
        }
        #endregion

        #region Event Handles
        //check border size and radius when resize the control
        private void Ctrl_Resize(object sender, EventArgs e)
        {
            var minSize = Width > Height ? Height : Width;
            Miner(ref _borderRadius, minSize / 2);
            Miner(ref _borderSize, minSize / 2);
        }
        #endregion

        #region Methods
        //set rounded region to the control
        private void SetTxtRoundedRegion() => numericUpDownMain.Region = new Region(GetFigurePath(numericUpDownMain.ClientRectangle, _borderSize * 2));

        //update the height of control when changed font display
        private void UpdateHCtrl()
        {
            numericUpDownMain.MinimumSize = new Size(0, MeasureText("0", Font).Height + 1);
            Height = numericUpDownMain.Height + Padding.Top + Padding.Bottom;
        }

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
