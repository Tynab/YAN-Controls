using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static System.ComponentModel.EditorBrowsableState;
using static System.Drawing.Color;
using static System.Drawing.StringAlignment;
using static System.Windows.Forms.ControlStyles;
using static System.Windows.Forms.TextRenderer;
using static YAN_Controls.TextPosition;

namespace YAN_Controls
{
    public class YANProgressBar : ProgressBar
    {
        #region Fields
        private Color _channelColor = LightSteelBlue;
        private Color _sliderColor = RoyalBlue;
        private Color _valueBackColor = RoyalBlue;
        private TextPosition _textAlign = TextPosition.Right;
        private string _symbolBefore = null;
        private string _symbolAfter = null;
        private int _channelHeight = 6;
        private int _sliderHeight = 6;
        private bool _showMaximum = false;
        private bool _paintedBlack = false;
        private bool _stopPainting = false;
        #endregion

        #region Constructors
        public YANProgressBar()
        {
            SetStyle(UserPaint, true);
            ForeColor = White;
        }
        #endregion

        #region Properties
        [Category("YAN Appearance"), Description("The color of the channel.")]
        public Color ChannelColor
        {
            get => _channelColor;
            set
            {
                _channelColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The color of the slider.")]
        public Color SliderColor
        {
            get => _sliderColor;
            set
            {
                _sliderColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("The background color of the value.")]
        public Color ValueBackColor
        {
            get => _valueBackColor;
            set
            {
                _valueBackColor = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("Indicates how the value should be aligned for edit controls.")]
        internal TextPosition TextAlign
        {
            get => _textAlign;
            set
            {
                _textAlign = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("That symbol that will be displayed before value on the control.")]
        public string SymbolBefore
        {
            get => _symbolBefore;
            set
            {
                _symbolBefore = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("That symbol that will be displayed after value on the control.")]
        public string SymbolAfter
        {
            get => _symbolAfter;
            set
            {
                _symbolAfter = value;
                Invalidate();
            }
        }

        [Category("YAN Appearance"), Description("That symbol that will be displayed before value on the control.")]
        public int ChannelHeight
        {
            get => _channelHeight;
            set
            {
                if (value >= 0)
                {
                    _channelHeight = value;
                    Invalidate();
                }
            }
        }

        [Category("YAN Appearance"), Description("The height of the slider in pixels.")]
        public int SliderHeight
        {
            get => _sliderHeight;
            set
            {
                if (value >= 0)
                {
                    _sliderHeight = value;
                    Invalidate();
                }
            }
        }

        [Category("YAN Appearance"), Description("When this property is true, the maximum value added to after value.")]
        public bool ShowMaximum
        {
            get => _showMaximum;
            set
            {
                _showMaximum = value;
                Invalidate();
            }
        }
        #endregion

        #region Overridden
        //font display
        [Browsable(true)]
        [EditorBrowsable(Always)]
        public override Font Font { get => base.Font; set => base.Font = value; }

        //foreground color
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        //paint background & channel
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!_stopPainting)
            {
                if (!_paintedBlack)
                {
                    var graphics = e.Graphics;
                    var rectChannel = new Rectangle(0, 0, Width, ChannelHeight);
                    using (var brushChannel = new SolidBrush(_channelColor))
                    {
                        rectChannel.Y = _channelHeight >= _sliderHeight ? Height - _channelHeight : Height - (_channelHeight + _sliderHeight) / 2;
                        //painting surface
                        graphics.Clear(Parent.BackColor);
                        //channel
                        graphics.FillRectangle(brushChannel, rectChannel);
                        //painting stop painting
                        if (!DesignMode)
                        {
                            _paintedBlack = true;
                        }
                    }
                }
                //reset painting
                if (Value == Maximum || Value == Minimum)
                {
                    _paintedBlack = false;
                }
            }
        }

        //paint slider
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_stopPainting)
            {
                var graphics = e.Graphics;
                var wSlider = (int)(Width * ((double)Value - Minimum) / ((double)Maximum - Minimum));
                var rectSlider = new Rectangle(0, 0, wSlider, SliderHeight);
                using (var brushSlider = new SolidBrush(_sliderColor))
                {
                    rectSlider.Y = _sliderHeight >= _channelHeight ? Height - _sliderHeight : Height - (_sliderHeight + _channelHeight) / 2;
                    //painting slider
                    if (wSlider > 1)
                    {
                        graphics.FillRectangle(brushSlider, rectSlider);
                    }
                    //painting text
                    if (_textAlign != None)
                    {
                        DrawValueText(graphics, wSlider, rectSlider);
                    }
                }
            }
            _stopPainting = Value == Maximum;
        }

        //paint value text
        private void DrawValueText(Graphics graphics, int wSlider, Rectangle rectSlider)
        {
            var text = $"{_symbolBefore}{Value}{_symbolAfter}";
            if (_showMaximum)
            {
                text += $"/{_symbolBefore}{Maximum}{_symbolAfter}";
            }
            var textSize = MeasureText(text, Font);
            var rectText = new Rectangle(0, 0, textSize.Width, textSize.Height + 2);
            using (var brushText = new SolidBrush(ForeColor))
            {
                using (var brushTextBack = new SolidBrush(_valueBackColor))
                {
                    using (var textFormat = new StringFormat())
                    {
                        switch (_textAlign)
                        {
                            case TextPosition.Left:
                            {
                                rectText.X = 0;
                                textFormat.Alignment = Near;
                                break;
                            }
                            case TextPosition.Right:
                            {
                                rectText.X = Width - textSize.Width;
                                textFormat.Alignment = Far;
                                break;
                            }
                            case TextPosition.Center:
                            {
                                rectText.X = (Width - textSize.Width) / 2;
                                textFormat.Alignment = StringAlignment.Center;
                                break;
                            }
                            case Sliding:
                            {
                                rectText.X = wSlider - textSize.Width;
                                textFormat.Alignment = StringAlignment.Center;
                                //clean previous surface
                                using (var brushClear = new SolidBrush(Parent.BackColor))
                                {
                                    var rect = rectSlider;
                                    rect.Y = rectText.Y;
                                    rect.Height = rectText.Height;
                                    graphics.FillRectangle(brushClear, rect);
                                }
                                break;
                            }
                        }
                        //painting
                        graphics.FillRectangle(brushTextBack, rectText);
                        graphics.DrawString(text, Font, brushText, rectText, textFormat);
                    }
                }
            }
        }
        #endregion
    }

    internal enum TextPosition
    {
        Left,
        Right,
        Center,
        Sliding,
        None
    }
}
