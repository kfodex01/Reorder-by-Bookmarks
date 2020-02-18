using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reorder_by_Bookmarks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult browseResult;
            browseResult = openFileDialog1.ShowDialog();
            if (browseResult == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = openFileDialog1.FileName;
                tbPath.Update();
            }

        }

        private void BtnSort_Click(object sender, EventArgs e)
        {
            string path = tbPath.Text;
            bool prioritize = cbPrioritize.Checked;
            string[] priorityList = System.Configuration.ConfigurationManager.AppSettings["sortList"].Split(',');
            if (ValidateTextBox(path))
            {
                lblMessage.Text = "Working";
                lblMessage.Update();
                BookmarkSorter sorter = new BookmarkSorter(path, prioritize, priorityList);
                string result = sorter.ReorderBookmarks();
                if (result.Equals(""))
                {
                    lblMessage.Text = "Finished";
                }
                else
                {
                    lblMessage.Text = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(result, "Error", buttons);
                }
            }

        }

        private bool ValidateTextBox(string text)
        {
            bool result = false;
            if (!text.Equals(""))
            {
                try
                {
                    if (System.IO.File.Exists(text) && System.IO.Path.GetExtension(text).ToLower().Equals(".pdf"))
                    {
                        result = true;
                    }
                }
                catch
                {

                }

            }

            return result;
        }
    }

}
