using P1.Data;
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

namespace P1.GUI
{
    public partial class Form1 : Form
    {
        List<string> filePaths = new List<string>();
        ImageList imageList = new ImageList();
        public Form1()
        {
            InitializeComponent();
            listViewFile.LargeImageList = imageList;
            imageList.ImageSize = new Size(64, 64);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "Media (jpg, jpeg, png, mp4, wmv, mov, avi)|*jpg;*.jpeg;*.png;*.mp4;*.wmv;*.mov;*.avi" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filePaths.Clear();
                    imageList = new ImageList();
                    imageList.ImageSize = new Size(64, 64);
                    listViewFile.Items.Clear();
                    foreach (string filePath in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(filePath);
                        filePaths.Add(fi.FullName);
                    }
                    AddFilesToListView(filePaths);
                }
            }
        }

        private void listViewFile_ItemActivate(object sender, EventArgs e)
        {
            if(listViewFile.FocusedItem != null)
            {
                if (IsImage(Path.GetExtension(filePaths[listViewFile.FocusedItem.Index])))
                {
                    using (frmPhoto frm = new frmPhoto())
                    {
                        if (File.Exists(filePaths[listViewFile.FocusedItem.Index]))
                        {
                            Image img = Image.FromFile(filePaths[listViewFile.FocusedItem.Index]);
                            frm.imageBox = img;
                        }
                        frm.path = filePaths[listViewFile.FocusedItem.Index];
                        frm.ShowDialog();
                        if (frm.result == "SAVED")
                        {
                            listViewFile.FocusedItem.BackColor = Color.Green;
                        }
                        else if (frm.result == "DELETED")
                        {
                            listViewFile.FocusedItem.BackColor = Color.AliceBlue;
                        }
                    }
                }
                else
                {
                    using (frmVideo frm = new frmVideo())
                    {
                        frm.path = filePaths[listViewFile.FocusedItem.Index];
                        frm.ShowDialog();
                        if (frm.result == "SAVED")
                        {
                            listViewFile.FocusedItem.BackColor = Color.Green;
                        }
                        else if (frm.result == "DELETED")
                        {
                            listViewFile.FocusedItem.BackColor = Color.AliceBlue;
                        }
                    }
                }
            }
        }

        private bool IsImage(string extension)
        {
            return extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png");
        }

        private void AddFilesToListView(List<string> fp)
        {
            MediaTagsClient c = new MediaTagsClient();
            int cnt = 0;

            foreach (string filePath in fp)
            {
                Media result = c.GetMediaByPath(filePath);
                Color clr;
                if (result != null)
                    clr = Color.Green;
                else clr = Color.AliceBlue;
                if (IsImage(Path.GetExtension(filePath)))
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        imageList.Images.Add(Image.FromStream(stream));
                    }
                else
                    using (FileStream stream = new FileStream("./videoicon.png", FileMode.Open, FileAccess.Read))
                    {
                        imageList.Images.Add(Image.FromStream(stream));
                    }
                listViewFile.LargeImageList = imageList;
                listViewFile.Items.Add(new ListViewItem
                {
                    ImageIndex = cnt,
                    Text = filePath.Substring(filePath.LastIndexOf("\\") + 1),
                    Tag = filePath,
                    BackColor = clr
                });
                cnt++;
            }
            c.Close();
        }

        private void AddFilesToListView(List<Media> m)
        {
            int cnt = 0;

            foreach (Media media in m)
            {
                Color clr;

                if (File.Exists(media.Path))
                    if (IsImage(Path.GetExtension(media.Path)))
                        using (FileStream stream = new FileStream(media.Path, FileMode.Open, FileAccess.Read))
                        {
                            imageList.Images.Add(Image.FromStream(stream));
                            media.IsDeleted = false;
                        }
                    else
                        using (FileStream stream = new FileStream("./videoicon.png", FileMode.Open, FileAccess.Read))
                        {
                            imageList.Images.Add(Image.FromStream(stream));
                            media.IsDeleted = false;
                        }
                else
                    using (FileStream stream = new FileStream("./notfound.png", FileMode.Open, FileAccess.Read))
                    {
                        imageList.Images.Add(Image.FromStream(stream));
                        media.IsDeleted = true;
                    }

                if (media.IsDeleted)
                    clr = Color.Red;
                else clr = Color.Green;

                listViewFile.LargeImageList = imageList;
                listViewFile.Items.Add(new ListViewItem
                {
                    ImageIndex = cnt,
                    Text = media.Path.Substring(media.Path.LastIndexOf("\\") + 1),
                    Tag = media.Path,
                    BackColor = clr
                });
                cnt++;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            filePaths.Clear();
            imageList = new ImageList();
            imageList.ImageSize = new Size(64, 64);
            listViewFile.Items.Clear();
            string toSearch = textBox1.Text.ToLower();
            MediaTagsClient c = new MediaTagsClient();
            if (checkBox1.Checked)
            {
                var mediaResult = c.QueryMediaDate(toSearch, dateTimePickerFrom.Value.Date, dateTimePickerTo.Value.Date)
                .ToList();

                var tagsResult = c.QueryTags(toSearch);

                HashSet<int> mediaIds = new HashSet<int>(tagsResult);
                foreach(int mediaId in mediaIds)
                {
                    Media m = c.GetMedia(mediaId);
                    if(m.Creation_Date >= dateTimePickerFrom.Value.Date && m.Creation_Date <= dateTimePickerTo.Value.Date)
                        mediaResult.Add(m);
                }
                foreach (Media m in mediaResult)
                {
                    filePaths.Add(m.Path);
                }
                AddFilesToListView(mediaResult);
            }
            else
            {
                var mediaResult = c.QueryMedia(toSearch)
                    .ToList();

                var tagsResult = c.QueryTags(toSearch);

                HashSet<int> mediaIds = new HashSet<int>(tagsResult);
                foreach (int mediaId in mediaIds)
                {
                    Media m = c.GetMedia(mediaId);
                    mediaResult.Add(m);
                }
                foreach (Media m in mediaResult)
                {
                    filePaths.Add(m.Path);
                }
                AddFilesToListView(mediaResult);
            }
            c.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    foreach(ListViewItem item in listViewFile.SelectedItems)
                    {
                        string destFile = fbd.SelectedPath;
                        string fileName = filePaths[item.Index].Substring(destFile.LastIndexOf("\\") + 1);
                        destFile = Path.Combine(destFile, fileName);
                        if(File.Exists(filePaths[item.Index]))
                            File.Copy(filePaths[item.Index], destFile, true);
                    }
                }
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    filePaths.Clear();
                    imageList = new ImageList();
                    imageList.ImageSize = new Size(64, 64);
                    listViewFile.Items.Clear();
                    var ext = new List<string> { ".jpg", ".jpeg", ".png", ".mp4", ".wmv", ".mov", ".avi" };
                    var myFiles = Directory
                        .EnumerateFiles(fbd.SelectedPath, "*.*")
                        .Where(s => ext.Contains(Path.GetExtension(s).ToLowerInvariant())).ToList();

                    foreach(string path in myFiles)
                    {
                        filePaths.Add(path);
                    }

                    AddFilesToListView(filePaths);
                }
            }
        }
    }
}
