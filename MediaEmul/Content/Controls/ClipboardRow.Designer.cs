namespace MediaEmul.Content.Controls
{
    partial class ClipboardRow
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoLabel = new System.Windows.Forms.Label();
            this.valueInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.infoLabel.Location = new System.Drawing.Point(0, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(65, 45);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Ctrl+X";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // valueInfo
            // 
            this.valueInfo.AutoEllipsis = true;
            this.valueInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.valueInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueInfo.Location = new System.Drawing.Point(65, 0);
            this.valueInfo.Name = "valueInfo";
            this.valueInfo.Padding = new System.Windows.Forms.Padding(7);
            this.valueInfo.Size = new System.Drawing.Size(235, 45);
            this.valueInfo.TabIndex = 1;
            this.valueInfo.Text = "Clipboard text";
            this.valueInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ClipboardRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueInfo);
            this.Controls.Add(this.infoLabel);
            this.MaximumSize = new System.Drawing.Size(99999, 45);
            this.MinimumSize = new System.Drawing.Size(300, 45);
            this.Name = "ClipboardRow";
            this.Size = new System.Drawing.Size(300, 45);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Label valueInfo;
    }
}
