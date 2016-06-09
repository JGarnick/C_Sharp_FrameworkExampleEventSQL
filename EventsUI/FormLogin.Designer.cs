namespace EventsUI
{
    partial class FormLogin
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
            this.logoPB = new System.Windows.Forms.PictureBox();
            this.pwTB = new System.Windows.Forms.TextBox();
            this.userTB = new System.Windows.Forms.TextBox();
            this.pwLbl = new System.Windows.Forms.Label();
            this.UserLbl = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.submitBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logoPB)).BeginInit();
            this.SuspendLayout();
            // 
            // logoPB
            // 
            this.logoPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoPB.Location = new System.Drawing.Point(28, 24);
            this.logoPB.Name = "logoPB";
            this.logoPB.Size = new System.Drawing.Size(179, 130);
            this.logoPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPB.TabIndex = 24;
            this.logoPB.TabStop = false;
            // 
            // pwTB
            // 
            this.pwTB.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwTB.Location = new System.Drawing.Point(303, 98);
            this.pwTB.Name = "pwTB";
            this.pwTB.PasswordChar = '*';
            this.pwTB.Size = new System.Drawing.Size(114, 23);
            this.pwTB.TabIndex = 21;
            // 
            // userTB
            // 
            this.userTB.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userTB.Location = new System.Drawing.Point(303, 58);
            this.userTB.Name = "userTB";
            this.userTB.Size = new System.Drawing.Size(112, 23);
            this.userTB.TabIndex = 18;
            // 
            // pwLbl
            // 
            this.pwLbl.BackColor = System.Drawing.SystemColors.Control;
            this.pwLbl.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwLbl.Location = new System.Drawing.Point(207, 98);
            this.pwLbl.Name = "pwLbl";
            this.pwLbl.Size = new System.Drawing.Size(88, 24);
            this.pwLbl.TabIndex = 19;
            this.pwLbl.Text = "Password";
            this.pwLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UserLbl
            // 
            this.UserLbl.BackColor = System.Drawing.SystemColors.Control;
            this.UserLbl.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLbl.Location = new System.Drawing.Point(207, 58);
            this.UserLbl.Name = "UserLbl";
            this.UserLbl.Size = new System.Drawing.Size(88, 23);
            this.UserLbl.TabIndex = 20;
            this.UserLbl.Text = "User Name";
            this.UserLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.Location = new System.Drawing.Point(231, 170);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(88, 23);
            this.cancelBtn.TabIndex = 23;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // submitBtn
            // 
            this.submitBtn.BackColor = System.Drawing.SystemColors.Control;
            this.submitBtn.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitBtn.Location = new System.Drawing.Point(127, 170);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(88, 23);
            this.submitBtn.TabIndex = 22;
            this.submitBtn.Text = "OK";
            this.submitBtn.UseVisualStyleBackColor = false;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 218);
            this.Controls.Add(this.logoPB);
            this.Controls.Add(this.pwTB);
            this.Controls.Add(this.userTB);
            this.Controls.Add(this.pwLbl);
            this.Controls.Add(this.UserLbl);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.submitBtn);
            this.Name = "FormLogin";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.logoPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPB;
        private System.Windows.Forms.TextBox pwTB;
        private System.Windows.Forms.TextBox userTB;
        private System.Windows.Forms.Label pwLbl;
        private System.Windows.Forms.Label UserLbl;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button submitBtn;
    }
}