using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChainChomp
{
    public partial class LibraryWindow : Form
    {
        public LibraryWindow()
        {
            InitializeComponent();

            ImageEmuLibrary.emus.ForEach(i =>
            {
                ListViewItem item = new ListViewItem(i);
                if (!File.Exists(i))
                {
                    item.ForeColor = Color.DarkRed;
                }
                emuListView.Items.Add(item);
            });


            ImageEmuLibrary.images.ForEach(i =>
            {
                ListViewItem item = new ListViewItem(i[0]);
                item.Tag = i[1];
                if (!File.Exists(i[0]))
                {
                    item.ForeColor = Color.DarkRed;
                }
                romImageListView.Items.Add(item);
            });


          
        }

        private void addROMImageButton_Click(object sender, EventArgs e)
        {
            if (addRomImageDialog.ShowDialog() == DialogResult.OK)
            {
                ListViewItem[] itemArr = new ListViewItem[romImageListView.Items.Count];
                romImageListView.Items.CopyTo(itemArr, 0);
                List<ListViewItem> itemList = itemArr.ToList();

                addRomImageDialog.FileNames.ToList().ForEach(i => 
                {
                    if (itemList.FirstOrDefault(j => j.Text == i) == null)
                    {
                        romImageListView.Items.Add(new ListViewItem(i));
                    }
                });
            }
        }


        private void addEmulatorButton_Click(object sender, EventArgs e)
        {
            ListViewItem[] itemArr = new ListViewItem[emuListView.Items.Count];
            emuListView.Items.CopyTo(itemArr, 0);
            List<ListViewItem> itemList = itemArr.ToList();

            if (addEmuDialog.ShowDialog() == DialogResult.OK)
            {
                if (itemArr.ToList().FirstOrDefault(i => i.Text == addEmuDialog.FileName) == null)
                {
                    emuListView.Items.Add(new ListViewItem(addEmuDialog.FileName));    
                }
            }
        }

        private void emuRemoveButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in emuListView.SelectedItems)
            {
                emuListView.Items.Remove(item);
            }
        }

        private void romImageRemoveButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in romImageListView.SelectedItems)
            {
                romImageListView.Items.Remove(item);
            }
        }

        private void LibraryWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //store lists in static class
            ImageEmuLibrary.emus.Clear();
            ImageEmuLibrary.images.Clear();

            foreach (ListViewItem item in emuListView.Items)
            {
                ImageEmuLibrary.emus.Add(item.Text);
            }

            foreach (ListViewItem item in romImageListView.Items)
            {
                ImageEmuLibrary.images.Add(new string[]{item.Text, item.Tag != null ? item.Tag.ToString() : null});
            }

            //save to disk
            ImageEmuLibrary.SaveSettings();
        }

    }
}
