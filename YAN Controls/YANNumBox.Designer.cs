
namespace YAN_Controls
{
    partial class YANNumBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDownMain = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMain)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownMain
            // 
            this.numericUpDownMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownMain.Location = new System.Drawing.Point(10, 7);
            this.numericUpDownMain.Name = "numericUpDownMain";
            this.numericUpDownMain.Size = new System.Drawing.Size(230, 18);
            this.numericUpDownMain.TabIndex = 0;
            this.numericUpDownMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // YANNumBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.numericUpDownMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DimGray;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "YANNumBox";
            this.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.Size = new System.Drawing.Size(250, 30);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownMain;
    }
}
