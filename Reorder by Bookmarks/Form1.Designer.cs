namespace Reorder_by_Bookmarks
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSort = new System.Windows.Forms.Button();
            this.cbPrioritize = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(13, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(313, 25);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Provide path to PDF for sorting.";
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(18, 41);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(542, 20);
            this.tbPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(566, 38);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSort
            // 
            this.btnSort.Location = new System.Drawing.Point(566, 67);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(75, 23);
            this.btnSort.TabIndex = 3;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.BtnSort_Click);
            // 
            // cbPrioritize
            // 
            this.cbPrioritize.AutoSize = true;
            this.cbPrioritize.Checked = true;
            this.cbPrioritize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPrioritize.Location = new System.Drawing.Point(433, 71);
            this.cbPrioritize.Name = "cbPrioritize";
            this.cbPrioritize.Size = new System.Drawing.Size(127, 17);
            this.cbPrioritize.TabIndex = 4;
            this.cbPrioritize.Text = "Prioritize Bookmarks?";
            this.cbPrioritize.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 98);
            this.Controls.Add(this.cbPrioritize);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.lblMessage);
            this.Name = "Form1";
            this.Text = "Reorder by Bookmarks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.CheckBox cbPrioritize;
    }
}

