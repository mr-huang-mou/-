using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace log_management_system
{
    public partial class LogEditor : Form
    {
        private int? _logId;
        public LogEditor(int? id = null)
        {
            InitializeComponent();
            _logId = id;

            if (_logId.HasValue)
            {
                LoadExistingLog();
            }
        }
        private void LoadExistingLog()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Logs WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", _logId.Value);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtTitle.Text = reader["Title"].ToString();
                        txtContent.Text = reader["Content"].ToString();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("请输入标题");
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SQLiteCommand cmd;

                if (_logId.HasValue)
                {
                    cmd = new SQLiteCommand(
                        @"UPDATE Logs 
                     SET Title = @Title, 
                         Content = @Content, 
                         UpdatedAt = datetime('now', 'localtime')
                     WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", _logId.Value);
                }
                else
                {
                    cmd = new SQLiteCommand(
                        @"INSERT INTO Logs (Title, Content)
                     VALUES (@Title, @Content)", conn);
                }

                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Content", txtContent.Text);
                cmd.ExecuteNonQuery();
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 初始窗体大小根据内容自适应
            int preferredHeight = txtTitle.Height + txtContent.PreferredHeight + 100;
            this.Height = Math.Min(preferredHeight, Screen.PrimaryScreen.WorkingArea.Height - 100);

            // 允许用户调整窗体大小
            this.MinimumSize = new Size(350, 400);
        }
        private void LogEditor_Resize(object sender, EventArgs e)
        {
            txtTitle.Width = this.ClientSize.Width - 30;
            txtContent.Height = this.ClientSize.Height - txtTitle.Height - 80;
        }
    }
}
