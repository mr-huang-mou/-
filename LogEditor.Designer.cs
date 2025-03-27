namespace log_management_system
{
    partial class LogEditor
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
            txtTitle = new TextBox();
            txtContent = new RichTextBox();
            btnSave = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(103, 12);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(138, 23);
            txtTitle.TabIndex = 0;
            txtTitle.TextAlign = HorizontalAlignment.Center;
            // 
            // txtContent
            // 
            txtContent.Location = new Point(22, 41);
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(292, 261);
            txtContent.TabIndex = 1;
            txtContent.Text = "";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(230, 308);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(84, 30);
            btnSave.TabIndex = 2;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 15);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 3;
            label1.Text = "标题";
            // 
            // LogEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(333, 350);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(txtContent);
            Controls.Add(txtTitle);
            Name = "LogEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "新建";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTitle;
        private RichTextBox txtContent;
        private Button btnSave;
        private Label label1;
    }
}