﻿namespace XFC.View.Dialog.User
{
    partial class Form_UserTianJia
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
            this.label10 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_affirm = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_UserName = new System.Windows.Forms.TextBox();
            this.tb_UserPassWord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(158, 206);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 20);
            this.label10.TabIndex = 35;
            this.label10.Text = "登录密码：";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancel.Location = new System.Drawing.Point(364, 368);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 39);
            this.btn_cancel.TabIndex = 34;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_affirm
            // 
            this.btn_affirm.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_affirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_affirm.Location = new System.Drawing.Point(160, 368);
            this.btn_affirm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_affirm.Name = "btn_affirm";
            this.btn_affirm.Size = new System.Drawing.Size(75, 39);
            this.btn_affirm.TabIndex = 33;
            this.btn_affirm.Text = "确认";
            this.btn_affirm.UseVisualStyleBackColor = false;
            this.btn_affirm.Click += new System.EventHandler(this.btn_affirm_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(158, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 20);
            this.label9.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(158, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 20);
            this.label8.TabIndex = 31;
            this.label8.Text = "登录名：";
            // 
            // tb_UserName
            // 
            this.tb_UserName.Location = new System.Drawing.Point(290, 146);
            this.tb_UserName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_UserName.Name = "tb_UserName";
            this.tb_UserName.Size = new System.Drawing.Size(149, 25);
            this.tb_UserName.TabIndex = 29;
            // 
            // tb_UserPassWord
            // 
            this.tb_UserPassWord.Location = new System.Drawing.Point(290, 207);
            this.tb_UserPassWord.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_UserPassWord.Name = "tb_UserPassWord";
            this.tb_UserPassWord.Size = new System.Drawing.Size(149, 25);
            this.tb_UserPassWord.TabIndex = 36;
            // 
            // Form_UserTianJia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(566, 450);
            this.Controls.Add(this.tb_UserPassWord);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_affirm);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_UserName);
            this.Name = "Form_UserTianJia";
            this.Text = "用户添加";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_affirm;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_UserName;
        private System.Windows.Forms.TextBox tb_UserPassWord;
    }
}