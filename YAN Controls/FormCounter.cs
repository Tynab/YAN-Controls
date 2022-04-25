using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Drawing.Region;
using static System.Windows.Forms.DialogResult;
using static System.Windows.Forms.FormStartPosition;
using static System.Windows.Forms.Keys;
using static YAN_Controls.Scipts.Method;
using static YAN_Controls.Scipts.Method.AnimateWindowFlags;

namespace YAN_Controls
{
    public partial class FormCounter : Form
    {
        #region Constructors
        public FormCounter(Form frm, int cor, bool is_Top)
        {
            InitializeComponent();
            //this
            Load += FormCounter_Load;
            //op
            StartPosition = Manual;
            Location = new Point(frm.Location.X, frm.Location.Y);
            Width = frm.Width;
            Height = frm.Height;
            TopMost = is_Top;
            pictureBoxWait.Top = (Height - pictureBoxWait.Height) / 2;
            labelPercent.Top = pictureBoxWait.Top + 300;
            labelPercent.Text = null;
            Region = FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, cor, cor));
        }
        #endregion

        #region Overridden
        //ẩn sub window
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        //tắt alt+f4
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) => keyData == (Alt | F4) || base.ProcessCmdKey(ref msg, keyData);
        #endregion

        #region Events
        //load
        private void FormCounter_Load(object sender, EventArgs e) => AnimateWindow(Handle, 300, AW_BLEND);
        #endregion

        #region Event Tokens
        //close
        public void FrmCloseToken()
        {
            DialogResult = OK;
            AnimateWindow(Handle, 300, AW_BLEND | AW_HIDE);
            Dispose();
        }
        #endregion
    }
}
