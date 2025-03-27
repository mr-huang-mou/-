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
        private const int Padding = 20; // �߾�
        private Size originalSize;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            originalSize = dgvLogs.Size;

            // ע�ᴰ���С�ı��¼�
            this.Resize += MainForm_Resize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // �����µĳߴ�
            int newWidth = this.ClientSize.Width - 2 * Padding;
            int newHeight = this.ClientSize.Height - 2 * Padding;

            // ����DataGridViewλ�úʹ�С
            dgvLogs.Size = new Size(
                Math.Max(newWidth, 100),  // ��С���
                Math.Max(newHeight, 100)  // ��С�߶�
            );
            dgvLogs.Location = new Point(Padding, Padding + 30);

            // �п�̬����
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

                // ��ӵ��Դ�����֤���Ƿ����
                if (!dataTable.Columns.Contains("Id"))
                {
                    MessageBox.Show("���ݱ�ȱ��Id�У��������ݿ�ṹ��");
                    return;
                }
                // ����DataGridView
                dgvLogs.AutoGenerateColumns = false;
                dgvLogs.DataSource = dataTable;

                // ���ԭ���ж���
                dgvLogs.Columns.Clear();

                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Id",
                    HeaderText = "Id",
                    Visible = false  // �ؼ����ã�������
                });
                // ��������Ӧ��
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Title",
                    HeaderText = "����",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });

                var contentColumn = new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "Content",
                    HeaderText = "����",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        WrapMode = DataGridViewTriState.True
                    }
                };
                dgvLogs.Columns.Add(contentColumn);

                // ʱ���й̶����
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "UpdatedAt",
                    HeaderText = "����޸�ʱ��",
                    Width = 150,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = "yyyy-MM-dd HH:mm:ss"  // ��ȷָ��ʱ���ʽ
                    }
                });
                dgvLogs.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "CreatedAt",
                    HeaderText = "����ʱ��",
                    Width = 150,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = "yyyy-MM-dd HH:mm:ss"  // ��ȷָ��ʱ���ʽ
                    }
                });
                // �Զ������и�
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

            if (MessageBox.Show("ȷ��Ҫɾ��������־��", "ȷ��ɾ��",
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