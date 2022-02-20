using System;
using System.Windows.Forms;
using static System.Windows.Forms.DialogResult;
using static System.Windows.Forms.Keys;
using static YAN_Controls.Scipts.Method;
using static YAN_Controls.Scipts.Method.AnimateWindowFlags;

namespace YAN_Controls
{
    public partial class FormUpdater : Form
    {
        #region Constructors
        public FormUpdater()
        {
            InitializeComponent();
            //setting
            labelCapacity.Text = null;
            labelPercent.Text = null;
            panelProgressBar.Width = 1;
        }
        #endregion

        #region Overridden
        //hide sub windows
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        //disable close
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) => keyData == (Alt | F4) || base.ProcessCmdKey(ref msg, keyData);
        #endregion

        #region Events
        //load
        private void FormUpdater_Load(object sender, EventArgs e) => AnimateWindow(Handle, 500, AW_BLEND);
        #endregion

        #region Event Tokens
        //close
        public void FrmCloseToken()
        {
            DialogResult = OK;
            AnimateWindow(Handle, 500, AW_BLEND | AW_HIDE);
            Dispose();
        }
        #endregion
    }
}
