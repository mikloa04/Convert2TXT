using System;
using System.Windows.Forms;

namespace Convert2TXT
{
    public partial class Main : Form
    {
        bool bIsBusy = false;
        string current_result = "";
        int current_percent = 0;
        int current_row = 0;
        public Main()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int count = dgvFileList.Rows.Count;
            if (selectFiles.ShowDialog() == DialogResult.OK)
            {
                foreach (string st in selectFiles.FileNames)
                {
                    count = dgvFileList.Rows.Add();
                    dgvFileList.Rows[count].Cells["colOrder"].Value = count + 1;
                    dgvFileList.Rows[count].Cells["colFile"].Value = st;
                    dgvFileList.Rows[count].Cells["colResult"].Value = string.Empty;

                }
            }
            
            
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            dgvFileList.Rows.Clear();
        }

        private void radOther_CheckedChanged(object sender, EventArgs e)
        {
            btnSelectFolder.Enabled = radOther.Checked;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (saveFolder.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = saveFolder.SelectedPath;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            if (bIsBusy)
            {
                string stFile = "";
                current_result = "";
                for (int i = 0; i < dgvFileList.Rows.Count; i++)
                {
                    stFile = dgvFileList.Rows[i].Cells["colFile"].Value.ToString();
                    current_result = ConvertTool.Convert(stFile, txtOutputFolder.Text,radOther.Checked);
                    current_row = i;                    
                    current_percent = ((i + 1) / dgvFileList.Rows.Count)*100;
                    backgroundWorker1.ReportProgress(current_percent);
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            dgvFileList.Rows[current_row].Cells["colResult"].Value = current_result;
           

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            
            bIsBusy = false;
            Set_Control(!bIsBusy);
            MessageBox.Show("Đã xong!", "Thông báo");
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            //Check output
            if (radOther.Checked && txtOutputFolder.Text.Length <= 0)
            {
                MessageBox.Show("Please select output folder!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bIsBusy = true;
            Set_Control(!bIsBusy);
            //Do work
            backgroundWorker1.RunWorkerAsync();
        }
        private void Set_Control(bool Active)
        {
            btnAdd.Enabled = Active;
            btnConvert.Enabled = Active;
            btnExit.Enabled = Active;
            btnClear.Enabled = Active;
            btnSelectFolder.Enabled = (Active && radOther.Checked);            
            if (Active)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
            }
        }
    }
}
