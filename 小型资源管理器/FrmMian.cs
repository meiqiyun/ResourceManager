using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 小型资源管理器
{
    public partial class FrmMian : Form
    {
        public FrmMian()
        {
            InitializeComponent();
        }
        //显示选中节点下的所有目录
        public void BindInfo(TreeNode node)
        {
            
            //创建一个目录对象
            DirectoryInfo di = new DirectoryInfo(node.Tag.ToString());         
            try
            {
                //返回目录下所有的子目录
               DirectoryInfo[] dirs = di.GetDirectories();
               //遍历读取出所有的子目录
               foreach (DirectoryInfo item in dirs)
               {
                   TreeNode temp = new TreeNode();
                   temp.Text = item.Name;//目录名
                   temp.Tag = item.FullName;//全路径
                   node.Nodes.Add(temp);
               }

               //获取该目录下的文件列表
               FileInfo[] fileInfos = di.GetFiles();
               //遍历读取文件列表到listView控件上
               this.listView1.Items.Clear();
               foreach (FileInfo item in fileInfos)
               {

                   ListViewItem li = new ListViewItem();
                   li.Text = item.Name;//文件名
                   float length = float.Parse(Math.Round(Convert.ToDouble(item.Length)/ 1024, 2).ToString());
                   li.SubItems.Add(length.ToString());//文件大小(KB)
                   li.SubItems.Add(item.Extension);//文件类型
                   li.SubItems.Add(item.FullName);//文件全路径
                   this.listView1.Items.Add(li);
               }
               //让listView视图的列宽随内容自适应
               foreach (ColumnHeader item in this.listView1.Columns)
               {
                   item.Width = -2;
               }
            }
            catch (Exception)
            {
                MessageBox.Show("无权限访问");
            }
            
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;
            //显示目录和文件信息
            BindInfo(node);

        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count<=0)
            {
                MessageBox.Show("请先选择要复制的文件！");
                return;
            }
            //提示用户选择目录文件夹
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            //源文件路径
            string sourcePath = this.listView1.SelectedItems[0].SubItems[3].Text;
            //目标文件路径
            string desPath = null;
            //如果正确选择目标位置，开始执行复制操作
            bool jg = false;//用于标识是否复制成功
            try
            {
                if (result == DialogResult.OK)
                {
                    desPath = fbd.SelectedPath;
                    desPath += "\\" + this.listView1.SelectedItems[0].SubItems[0].Text;//目标文件路径追加上文件名
                    //执行复制
                    File.Copy(sourcePath, desPath);
                    jg = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                jg = false;
            }
            if (jg==true)
            {
                MessageBox.Show("复制成功！");
            }
            else
            {
                MessageBox.Show("复制失败！");
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择要删除的文件！");
                return;
            }
            //要删除的文件路径
            string Path = this.listView1.SelectedItems[0].SubItems[3].Text;
            if (!File.Exists(Path))
            {
                MessageBox.Show("文件不存在！");
                return;
            }
            bool jg = false;//用于标识是否删除成功

            try
            {
                File.Delete(Path);
                jg = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                jg = false;
            }

            if (jg==true)
            {
                MessageBox.Show("删除成功！");
                TreeNode node = this.treeView1.SelectedNode;
                BindInfo(node);
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }
    }
}
