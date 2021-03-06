﻿using P1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P1.GUI
{
    public partial class frmPhoto : Form
    {
        private MediaTagsClient c;
        private Media mediaLoaded { get; set; }
        public string result { get; set; }
        public Image imageBox { get; set; }
        public string path { get; set; }

        public frmPhoto()
        {
            InitializeComponent();
        }

        private void AddRow(string name)
        {
            listViewTags.Items.Add(new ListViewItem(new[] { name }));
        }

        private void frmAddMedia_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Dispose();
            }
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTagName.Text))
                return;

            AddRow(txtTagName.Text);
            c.AddTagToMedia(mediaLoaded, new Tags() { Name = txtTagName.Text });
            txtTagName.Clear();
            txtTagName.Focus();
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            if (listViewTags.Items.Count > 0)
            {
                foreach (ListViewItem itemSelected in listViewTags.SelectedItems)
                {
                    Tags t = mediaLoaded.Tags.ElementAt(itemSelected.Index);
                    c.RemoveTagFromMedia(mediaLoaded.Path, t.Id);
                    listViewTags.Items.Remove(itemSelected);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mediaLoaded.Creation_Date = dateTimePicker.Value.Date;
            mediaLoaded.Description = txtDescription.Text;

            if (result == "IN DB")
                c.UpdateMedia(mediaLoaded);
            else
                c.AddMedia(mediaLoaded);
            c.Close();
            result = "SAVED";
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (result == "IN DB")
            {
                for (int i = mediaLoaded.Tags.Count - 1; i >= 0; i--)
                {
                    c.DeleteTag(mediaLoaded.Tags.ElementAt(i).Id);
                }
                c.DeleteMedia(mediaLoaded.Id);
            }
            result = "DELETED";
            this.Close();
        }

        private void frmAddMedia_Load(object sender, EventArgs e)
        {
            this.c = new MediaTagsClient();
            this.pictureBox1.Image = this.imageBox;
            this.lblFilePath.Text = this.path;
            mediaLoaded = c.GetMediaByPath(this.path);
            if (mediaLoaded != null)
            {
                foreach (Tags t in mediaLoaded.Tags)
                {
                    AddRow(t.Name);
                }
                dateTimePicker.Value = mediaLoaded.Creation_Date.Date;
                txtDescription.Text = mediaLoaded.Description;
                result = "IN DB";
            }
            else
            {
                mediaLoaded = new Media();
                mediaLoaded.Path = this.path;
                mediaLoaded.Tags = new List<Tags>();
                result = "NOT IN DB";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_PrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bm, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }
    }
}
