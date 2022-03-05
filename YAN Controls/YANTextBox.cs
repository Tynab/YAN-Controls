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
    [DefaultEvent("TxtChanged")]
    public partial class YANTextBox : UserControl
    {
        #region Fields
        private Color _borderColor = MediumSlateBlue;
        private Color _borderFocusColor = HotPink;
        private Color _placeholderColor = DarkGray;
        private string _placeholderText = null;
        private int _borderSize = 2;
        private int _borderRadius = 0;
        private bool _underlinedStyle = false;
        private bool _isFocus = false;
        private bool _isPlaceholder = false;
        private bool _isPasswordChar = false;
        #endregion

        #region Constructors
        public YANTextBox()
        {
            InitializeComponent();
            Resize += Ctrl_Resize;
        }
        #endregion

        #region Properties
        [Category("YAN Appearance"), Description("Indicates how the text should be aligned for edit controls.")]
        public HorizontalAlignment TextAlign
        {
            get => textBoxMain.TextAlign;
            set
            {
                textBoxMain.TextAlign = value;
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

        [Category("YAN Appearance"), Description("The color of the placeholder text.")]
        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set
            {
                _placeholderColor = value;
                if (_isPlaceholder)
                {
                    textBoxMain.ForeColor = value;
                }
            }
        }

        [Category("YAN Appearance"), Description("The text associated with the control.")]
        public string Txt
        {
            get => _isPlaceholder ? null : textBoxMain.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    textBoxMain.Text = value;
                    SetPlaceholder();
                }
                else
                {
                    RemovePlaceholder();
                    textBoxMain.Text = value;
                }
            }
        }

        [Category("YAN Appearance"), Description("The text that is displayed when the control has no text and does not have the focus.")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                textBoxMain.Text = null;
                SetPlaceholder();
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

        [Category("YAN Appearance"), Description("Specifies the maximum number of characters that can be entered into the edit control.")]
        public int MaxLength { get => textBoxMain.MaxLength; set => textBoxMain.MaxLength = value; }

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

        [Category("YAN Appearance"), Description("Indicates the character to display for password input for single-line edit controls.")]
        public bool PasswordChar
        {
            get => _isPasswordChar;
            set
            {
                _isPasswordChar = value;
                if (!_isPlaceholder)
                {
                    textBoxMain.UseSystemPasswordChar = value;
                }
            }
        }

        [Category("YAN Appearance"), Description("Control whether the text of the edit control can span more than one line.")]
        public bool Multiline { get => textBoxMain.Multiline; set => textBoxMain.Multiline = value; }

        //event
        [Category("YAN Event"), Description("Event raised when the value of the Txt property is changed on Control.")]
        public event EventHandler TxtChanged;
        #endregion

        #region Overridden
        //background color
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                textBoxMain.BackColor = value;
            }
        }

        //foreground color
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                textBoxMain.ForeColor = value;
            }
        }

        //font display
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                textBoxMain.Font = value;
                if (DesignMode)
                {
                    UpdateHCtrl();
                }
            }
        }

        //one paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            if (_borderRadius > 1)
            {
                var rectBorderSmooth = ClientRectangle;
                using (var pathBorderSmooth = GetFigurePath(rectBorderSmooth, _borderRadius))
                {
                    using (var pathBorder = GetFigurePath(Inflate(rectBorderSmooth, -_borderSize, -_borderSize), _borderRadius - _borderSize))
                    {
                        using (var penBorderSmooth = new Pen(Parent.BackColor, _borderSize > 0 ? _borderSize : 1))
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
        //raises the text changed event
        private void TextBoxMain_TextChanged(object sender, EventArgs e)
        {
            if (TxtChanged != null)
            {
                TxtChanged.Invoke(sender, e);
            }
        }

        //raises the mouse enter event
        private void TextBoxMain_MouseEnter(object sender, EventArgs e) => OnMouseEnter(e);

        //raises the mouse leave event
        private void TextBoxMain_MouseLeave(object sender, EventArgs e) => OnMouseLeave(e);

        //raises the key press event
        private void TextBoxMain_KeyPress(object sender, KeyPressEventArgs e) => OnKeyPress(e);

        //raises the key down event
        private void TextBoxMain_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        //raises the key up event
        private void TextBoxMain_KeyUp(object sender, KeyEventArgs e) => OnKeyUp(e);

        //raises the enter event
        private void TextBoxMain_Enter(object sender, EventArgs e)
        {
            _isFocus = true;
            Invalidate();
            RemovePlaceholder();
        }

        //raises the leave event
        private void TextBoxMain_Leave(object sender, EventArgs e)
        {
            _isFocus = false;
            Invalidate();
            SetPlaceholder();
        }
        #endregion

        #region Event Handles
        //check border size and radius when resize the control
        private void Ctrl_Resize(object sender, EventArgs e)
        {
            var minSize = Width > Height ? Height : Width;
            _borderRadius = Miner(_borderRadius, minSize / 2);
            _borderSize = Miner(_borderSize, minSize / 2);
        }
        #endregion

        #region Methods
        //add placeholder text to the control
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBoxMain.Text) && !string.IsNullOrWhiteSpace(_placeholderText))
            {
                _isPlaceholder = true;
                textBoxMain.Text = _placeholderText;
                textBoxMain.ForeColor = _placeholderColor;
                if (_isPasswordChar)
                {
                    if (Created)
                    {
                        BeginInvoke(new Action(() => textBoxMain.UseSystemPasswordChar = false));
                    }
                    else
                    {
                        textBoxMain.UseSystemPasswordChar = false;
                    }
                }
            }
        }

        //remove placeholder text to the control
        private void RemovePlaceholder()
        {
            if (_isPlaceholder && !string.IsNullOrWhiteSpace(_placeholderText))
            {
                _isPlaceholder = false;
                textBoxMain.Text = null;
                textBoxMain.ForeColor = ForeColor;
                if (_isPasswordChar)
                {
                    textBoxMain.UseSystemPasswordChar = true;
                }
            }
        }

        //set rounded region to the control
        private void SetTxtRoundedRegion()
        {
            var client = textBoxMain.ClientRectangle;
            textBoxMain.Region = Multiline ? new Region(GetFigurePath(client, _borderRadius - _borderSize)) : new Region(GetFigurePath(client, _borderSize * 2));
        }

        //update the height of control when changed font display
        private void UpdateHCtrl()
        {
            if (!textBoxMain.Multiline)
            {
                textBoxMain.Multiline = true;
                textBoxMain.MinimumSize = new Size(0, MeasureText("Text", Font).Height + 1);
                textBoxMain.Multiline = false;
                Height = textBoxMain.Height + Padding.Top + Padding.Bottom;
            }
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
