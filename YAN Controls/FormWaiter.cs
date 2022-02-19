﻿using System;
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
    public partial class FormWaiter : Form
    {
        #region Constructors
        public FormWaiter(Form form, int corner, bool onTop)
        {
            InitializeComponent();
            //form
            StartPosition = Manual;
            Location = new Point(form.Location.X, form.Location.Y);
            Width = form.Width;
            Height = form.Height;
            TopMost = onTop;
            //option
            pictureBoxWait.Top = (Height - pictureBoxWait.Height) / 2;
            //setting
            Region = FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, corner, corner));
        }
        #endregion

        #region Locks
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
        private void FormWaiter_Load(object sender, EventArgs e) => AnimateWindow(Handle, 500, AW_BLEND);
        #endregion

        #region Event Tokens
        //close
        public void CloseToken()
        {
            DialogResult = OK;
            AnimateWindow(Handle, 500, AW_BLEND | AW_HIDE);
            Dispose();
        }
        #endregion
    }
}