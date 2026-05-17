using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CAB.UI
{
    public partial class FolderSelectDialog : Form
    {
        private static string driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private DirectoryInfo folder;
        public FolderSelectDialog()
        {
            InitializeComponent();
            fillTree();
        }
        private void fillTree()
        {
            DirectoryInfo directory;
            string sCurPath = "";
            treeView1.Nodes.Clear();
            foreach (char c in driveLetters)
            {
                sCurPath = c + ":\\";
                try
                {
                    directory = new DirectoryInfo(sCurPath);
                    if (directory.Exists == true)
                    {
                        TreeNode newNode = new TreeNode(directory.FullName);
                        treeView1.Nodes.Add(newNode);
                        getSubDirs(newNode);
                    }
                }
                catch (Exception doh)
                {
                    Console.WriteLine(doh.Message);
                }
            }
        }

        private void getSubDirs(TreeNode parent)
        {
            DirectoryInfo directory;
            try
            {
                if (parent.Nodes.Count == 0)
                {
                    directory = new DirectoryInfo(parent.FullPath);
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        TreeNode newNode = new TreeNode(dir.Name);
                        parent.Nodes.Add(newNode);
                    }
                }
                foreach (TreeNode node in parent.Nodes)
                {
                    if (node.Nodes.Count == 0)
                    {
                        directory = new DirectoryInfo(node.FullPath);
                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            TreeNode newNode = new TreeNode(dir.Name);
                            node.Nodes.Add(newNode);
                        }
                    }
                }
            }
            catch (Exception doh)
            {
                Console.WriteLine(doh.Message);
            }
        }
        private string fixPath(TreeNode node)
        {
            string sRet = "";
            try
            {
                sRet = node.FullPath;
                int index = sRet.IndexOf("\\\\");
                if (index > 1)
                {
                    sRet = node.FullPath.Remove(index, 1);
                }
            }
            catch (Exception doh)
            {
                Console.WriteLine(doh.Message);
            }
            return sRet;
        }
        private void treeView1_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            getSubDirs(e.Node);					 
            textBox1.Text = fixPath(e.Node);	 
            folder = new DirectoryInfo(e.Node.FullPath);	 
        }

        private void treeView1_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            getSubDirs(e.Node);					 
            textBox1.Text = fixPath(e.Node);	 
            folder = new DirectoryInfo(e.Node.FullPath);	 
        }

        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            folder = null;
            this.Close();
        }
        private void selectBtn_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string name
        {
            get { return ((folder != null && folder.Exists)) ? folder.Name : null; }
        }
        public string fullPath
        {
            get { return ((folder != null && folder.Exists && treeView1.SelectedNode != null)) ? fixPath(treeView1.SelectedNode) : null; }
        }

        public DirectoryInfo info
        {
            get { return ((folder != null && folder.Exists)) ? folder : null; }
        }
    }
}
