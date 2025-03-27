using System.Data;
using System.Data.SQLite;

namespace log_management_system
{
    public partial class Form1 : Form
    {
        private DataTable dataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
            LoadLogs();
        }
        private const int Padding = 20; // 边距
        private Size originalSize;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            originalSize = dgvLogs.Size;

            // 注册窗体大小改变事件
            this.Resize += MainForm_Resize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // 计算新的尺寸
            int newWidth = this.ClientSize.Width - 2 * Padding;
            int newHeight = this.ClientSize.Height - 2 * Padding;

            // 设置DataGridView位置和大小
            dgvLogs.Size = new Size(
                Math.Max(newWidth, 100),  // 最小宽度
                Math.Max(newHeight, 100)  // 最小高度
            );
            dgvLogs.Location = new Point(Padding, Padding + 30);

            // 列宽动态调整
            foreach (DataGridViewColumn col in dgvLogs.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void LoadLogs()
        {
            dgvLogs.SuspendLayout();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var adapter = new SQLiteDataAdapter("SELECT Id, Title, Content, CreatedAt, UpdatedAt FROM Logs ORDER BY UpdatedAt DESC", conn);
                dataTable.Clear();
                adapter.Fill(dataTable);

                // 添加调试代码验证列是否存在
                if (!dataTable.Columns.Contains("Id"))
                {
                    MessageBox.Show("数据表缺少Id列，请检查数据库结构！");
                    return;
                }
                // 配置DataGridView
                dgvLogs.AutoGenerateColumns = false;
                dgvLogs.DataSource = dataTable;

                // 清空原有列定义
                dgvLogs.Columns.Clear();

                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Id",
                    HeaderText = "Id",
                    Visible = false  // 关键设置：隐藏列
                });
                // 创建自适应列
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Title",
                    HeaderText = "标题",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });

                var contentColumn = new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Content",
                    HeaderText = "内容",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        WrapMode = DataGridViewTriState.True
                    }
                };
                dgvLogs.Columns.Add(contentColumn);

                // 时间列固定宽度
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "UpdatedAt",
                    HeaderText = "最后修改时间",
                    Width = 150,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = "yyyy-MM-dd HH:mm:ss"  // 明确指定时间格式
                    }
                });
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "CreatedAt",
                    HeaderText = "创建时间",
                    Width = 150,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = "yyyy-MM-dd HH:mm:ss"  // 明确指定时间格式
                    }
                });
                // 自动调整行高
                dgvLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvLogs.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dgvLogs.ResumeLayout();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var editor = new LogEditor();
            if (editor.ShowDialog() == DialogResult.OK)
            {
                LoadLogs();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLogs.CurrentRow == null) return;

            var id = Convert.ToInt32(dgvLogs.CurrentRow.Cells[0].Value);
            var editor = new LogEditor(id);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                LoadLogs();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLogs.CurrentRow == null) return;

            if (MessageBox.Show("确定要删除这条日志吗？", "确认删除",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var id = Convert.ToInt32(dgvLogs.CurrentRow.Cells[0].Value);

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("DELETE FROM Logs WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                LoadLogs();
            }
        }
    }
}