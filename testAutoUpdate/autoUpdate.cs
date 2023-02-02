using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoUpdater;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace testAutoUpdate
{
    public class autoUpdate
    {
        public autoUpdate()
        { }

        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;

        public bool CheckUpdate(ref string updateContent)
        {
            bool isUpdate = false;
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            string serverXmlFile = string.Empty;

            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch
            {
                MessageBox.Show("配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdate = false;
            }
            //获取服务器地址
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");
            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/UpdateList.xml";



            //与服务器连接,下载更新配置文件
            try
            {
                tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            }
            catch
            {
                MessageBox.Show("与服务器连接失败,操作超时!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
              
                isUpdate = false;
            }

            //获取更新文件列表
            Hashtable htUpdateFile = new Hashtable();
            serverXmlFile = tempUpdatePath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                isUpdate = false;
            }
            else
            {
                availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
                if (availableUpdate > 0)
                {
                    updaterXmlFiles = new XmlFiles(serverXmlFile);
                    updateContent = updaterXmlFiles.GetNodeValue("//UpdateContent");

                    isUpdate = true;
                }
                else
                {
                    isUpdate = false;
                }
            }
            return isUpdate;
        }

    }
}
