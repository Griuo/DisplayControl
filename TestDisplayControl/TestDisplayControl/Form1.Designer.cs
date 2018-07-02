namespace TestDisplayControl
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.butDrawCircle = new System.Windows.Forms.Button();
            this.butDrawRect = new System.Windows.Forms.Button();
            this.butDrawPolygon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel.Location = new System.Drawing.Point(11, 11);
            this.panel.Margin = new System.Windows.Forms.Padding(2);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(527, 379);
            this.panel.TabIndex = 0;
            // 
            // butDrawCircle
            // 
            this.butDrawCircle.Location = new System.Drawing.Point(542, 43);
            this.butDrawCircle.Margin = new System.Windows.Forms.Padding(2);
            this.butDrawCircle.Name = "butDrawCircle";
            this.butDrawCircle.Size = new System.Drawing.Size(80, 39);
            this.butDrawCircle.TabIndex = 1;
            this.butDrawCircle.Text = "DrawCircle";
            this.butDrawCircle.UseVisualStyleBackColor = true;
            this.butDrawCircle.Click += new System.EventHandler(this.butDrawCircle_Click);
            // 
            // butDrawRect
            // 
            this.butDrawRect.Location = new System.Drawing.Point(542, 86);
            this.butDrawRect.Margin = new System.Windows.Forms.Padding(2);
            this.butDrawRect.Name = "butDrawRect";
            this.butDrawRect.Size = new System.Drawing.Size(80, 39);
            this.butDrawRect.TabIndex = 1;
            this.butDrawRect.Text = "DrawRect";
            this.butDrawRect.UseVisualStyleBackColor = true;
            this.butDrawRect.Click += new System.EventHandler(this.butDrawRect_Click);
            // 
            // butDrawPolygon
            // 
            this.butDrawPolygon.Location = new System.Drawing.Point(542, 129);
            this.butDrawPolygon.Margin = new System.Windows.Forms.Padding(2);
            this.butDrawPolygon.Name = "butDrawPolygon";
            this.butDrawPolygon.Size = new System.Drawing.Size(80, 39);
            this.butDrawPolygon.TabIndex = 1;
            this.butDrawPolygon.Text = "DrawPolygon";
            this.butDrawPolygon.UseVisualStyleBackColor = true;
            this.butDrawPolygon.Click += new System.EventHandler(this.butDrawPolygon_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 403);
            this.Controls.Add(this.butDrawPolygon);
            this.Controls.Add(this.butDrawRect);
            this.Controls.Add(this.butDrawCircle);
            this.Controls.Add(this.panel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button butDrawCircle;
        private System.Windows.Forms.Button butDrawRect;
        private System.Windows.Forms.Button butDrawPolygon;
    }
}

