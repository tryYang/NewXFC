namespace XFC.View.Dialog.ProductPump
{
    partial class Form_SavePump
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_updata = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_CarName = new System.Windows.Forms.TextBox();
            this.btn_select = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 141);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1331, 821);
            this.dataGridView1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "产品信息浏览";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btn_delete);
            this.panel2.Controls.Add(this.btn_updata);
            this.panel2.Controls.Add(this.btn_add);
            this.panel2.Location = new System.Drawing.Point(553, 21);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(512, 79);
            this.panel2.TabIndex = 10;
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_delete.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_delete.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_delete.Location = new System.Drawing.Point(349, 18);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 37);
            this.btn_delete.TabIndex = 2;
            this.btn_delete.Text = "删除";
            this.btn_delete.UseVisualStyleBackColor = false;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_updata
            // 
            this.btn_updata.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_updata.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_updata.Location = new System.Drawing.Point(201, 18);
            this.btn_updata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_updata.Name = "btn_updata";
            this.btn_updata.Size = new System.Drawing.Size(75, 37);
            this.btn_updata.TabIndex = 1;
            this.btn_updata.Text = "修改";
            this.btn_updata.UseVisualStyleBackColor = false;
            this.btn_updata.Click += new System.EventHandler(this.btn_updata_Click);
            // 
            // btn_add
            // 
            this.btn_add.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_add.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_add.Location = new System.Drawing.Point(43, 18);
            this.btn_add.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 37);
            this.btn_add.TabIndex = 0;
            this.btn_add.Text = "添加";
            this.btn_add.UseVisualStyleBackColor = false;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.tb_CarName);
            this.panel3.Controls.Add(this.btn_select);
            this.panel3.Location = new System.Drawing.Point(21, 21);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 79);
            this.panel3.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "水泵名称";
            // 
            // tb_CarName
            // 
            this.tb_CarName.Location = new System.Drawing.Point(16, 28);
            this.tb_CarName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_CarName.Name = "tb_CarName";
            this.tb_CarName.Size = new System.Drawing.Size(173, 25);
            this.tb_CarName.TabIndex = 4;
            // 
            // btn_select
            // 
            this.btn_select.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_select.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_select.Location = new System.Drawing.Point(229, 17);
            this.btn_select.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(75, 37);
            this.btn_select.TabIndex = 3;
            this.btn_select.Text = "查询";
            this.btn_select.UseVisualStyleBackColor = false;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // Form_SavePump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 962);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "Form_SavePump";
            this.Text = "消防泵产品信息";
            this.Load += new System.EventHandler(this.Form_SavePump_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_updata;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_CarName;
        private System.Windows.Forms.Button btn_select;
    }
}