using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoUpdater;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace testAutoUpdate
{
    public partial class frmtestUpdate : Form
    {
        public frmtestUpdate()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string content = "";
            autoUpdate update = new autoUpdate();
            if (update.CheckUpdate(ref content))
            {
                if(MessageBox.Show(string.Format("系统检测有可用更新,更新内容为:{0}是否现在立即更新？", content), "确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)                
                {
                    this.Close();
                    Process.Start(Path.Combine(Application.StartupPath, "AutoUpdater.exe"));
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
