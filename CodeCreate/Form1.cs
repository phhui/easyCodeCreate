using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CodeCreate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<FileInfo> fileListData = new List<FileInfo>(); //保存所有的文件信息 
        private void btn_createCode_Click(object sender, EventArgs e)
        {
            if (txt_modelName.Text.Length < 3)
            {
                MessageBox.Show("请输入项目名字");
                return;
            }
            if (txt_path.Text.Length < 5 || txt_path.Text == "点击选择路径,项目中类路径默认为main.submodules")
            {
                MessageBox.Show("请选择项目位置");
                return;
            }
            GetAllFilesInDirectory("template/", true);
            createFile();
        }
        private void txt_path_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                txt_path.Text = fb.SelectedPath;
            }
        }
        public void GetAllFilesInDirectory(string strDirectory, Boolean contains)
        {
            if (!Directory.Exists(strDirectory))
            {
                MessageBox.Show("模板路径无效~！");
                return;
            }
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) fileListData.AddRange(fileInfoArray);
            if (contains)
            {
                foreach (DirectoryInfo _directoryInfo in directoryArray)
                {//遍历子目录
                    GetAllFilesInDirectory(_directoryInfo.FullName, contains);
                }
            }
            txt_output.Text = "获取模板数据~~\r\n";            
        }
        private void createFile()
        {
            foreach (FileInfo fi in fileListData)
            {
                string p = fi.FullName.Replace(System.Windows.Forms.Application.StartupPath + "\\template\\", "").Replace(fi.Name, "");//模板目录结构
                string path = txt_path.Text + "/" + txt_modelName.Text + "/" + p;//目标目录
                Directory.CreateDirectory(path);//创建目标目录
                string str = File.ReadAllText(fi.FullName);//读取模板数据
                str = str.Replace("56lea.com", txt_modelName.Text);//替换模板数据
                txt_output.Text += fi.Name.Replace("56lea.com",txt_modelName.Text) + "生成完毕，位置：" + path + fi.Name.Replace("56lea.com", txt_modelName.Text).Replace("/","\\") + "\r\n";
                string fileType = fi.Name.Substring(fi.Name.Length - 4);
                if (fileType.IndexOf(".jpg") != -1 || fileType.IndexOf(".gif") != -1 || fileType.IndexOf(".png") != -1 || fileType.IndexOf(".jpeg") != -1 || fileType.IndexOf(".swf") != -1)
                {
                    File.Copy(fi.FullName, path + fi.Name.Replace("56lea.com", txt_modelName.Text));
                }
                else
                {
                    File.WriteAllText(path + fi.Name.Replace("56lea.com", txt_modelName.Text), str);//生成代码
                }
            }
            txt_output.Text += "生成完毕~~";
            MessageBox.Show("代码生成完毕");
        }
    }
}
