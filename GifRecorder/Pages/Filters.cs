﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenToGif.Encoding;
using ScreenToGif.Properties;

namespace ScreenToGif.Pages
{
    public partial class Filters : Form
    {
        public List<Bitmap> ListBitmap { get; private set; }
        private List<Bitmap> listBitmapReset;

        private Thread thread;

        /// <summary>
        /// Constructor, Gets the List of Bitmaps and shows in the picture box the first one.
        /// </summary>
        /// <param name="bitmap">A list of Bitmap to apply filters on it.</param>
        public Filters(List<Bitmap> bitmap)
        {
            InitializeComponent();

            listBitmapReset = new List<Bitmap>(bitmap);
            ListBitmap = new List<Bitmap>(bitmap);

            //Calculates the size of the form, to keep everything on place.
            this.Size = new Size(bitmap[0].Size.Width + 24, bitmap[0].Size.Height + (110));

            //Shows the first image of the list.
            pictureBoxFilter.Image = bitmap[0];

            //Sets the range of the trackBar
            trackBar.Maximum = ListBitmap.Count - 1;
            this.Text = Resources.Title_FiltersFrame + trackBar.Value + " - " + (ListBitmap.Count - 1);
        }

        /// <summary>
        /// Shows the respective image selected in the trackBar
        /// </summary>
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            pictureBoxFilter.Image = (Bitmap)ListBitmap[trackBar.Value];
            this.Text = Resources.Title_FiltersFrame + trackBar.Value + " - " + (ListBitmap.Count - 1);
        }

        /// <summary>
        /// Resets all changes done here in this page.
        /// </summary>
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBitmap.Clear();
            ListBitmap = new List<Bitmap>(listBitmapReset);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
        }

        /// <summary>
        /// Apply selected image to a grayscale filter
        /// </summary>
        private void GrayscaleOne_Click(object sender, EventArgs e)
        {
            ListBitmap[trackBar.Value] = ImageUtil.MakeGrayscale((Bitmap)pictureBoxFilter.Image);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
        }

        /// <summary>
        /// Apply selected image to a pixelated filter
        /// </summary>
        private void PixelateOne_Click(object sender, EventArgs e)
        {
            //User first need to choose the intensity of the pixelate
            ValuePicker valuePicker = new ValuePicker(100, 2, Resources.Msg_PixelSize);
            valuePicker.ShowDialog();

            ListBitmap[trackBar.Value] = ImageUtil.Pixelate((Bitmap)pictureBoxFilter.Image, new Rectangle(0, 0, pictureBoxFilter.Image.Width, pictureBoxFilter.Image.Height), valuePicker.Value);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];

            valuePicker.Dispose();
        }

        /// <summary>
        /// Apply selected image to a blur filter
        /// </summary>
        private void BlurOne_Click(object sender, EventArgs e)
        {
            ValuePicker valuePicker = new ValuePicker(5, 1, Resources.Msg_BlurIntense);
            valuePicker.ShowDialog();

            ListBitmap[trackBar.Value] = ImageUtil.Blur((Bitmap)pictureBoxFilter.Image, new Rectangle(0, 0, pictureBoxFilter.Image.Width, pictureBoxFilter.Image.Height), valuePicker.Value);

            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            valuePicker.Dispose();
        }

        private void ColorizeOne_Click(object sender, EventArgs e)
        {
            //here colorize code, not done yet
           pictureBoxFilter.Image = ListBitmap[trackBar.Value];
        }

        /// <summary>
        /// Convert selected image to negative filter
        /// </summary>
        private void NegativeOne_Click(object sender, EventArgs e)
        {            
            pictureBoxFilter.Image = ListBitmap[trackBar.Value] = ImageUtil.Negative(pictureBoxFilter.Image);
        }

        /// <summary>
        /// Convert selected image to transparency filter
        /// </summary>
        private void TransparencyOne_Click(object sender, EventArgs e)
        {
            pictureBoxFilter.Image = ListBitmap[trackBar.Value] = ImageUtil.Transparency(pictureBoxFilter.Image);
        }

        /// <summary>
        /// Convert selected image to SepiaTone filter
        /// </summary>
        private void sepiaToneOne_Click(object sender, EventArgs e)
        {
            pictureBoxFilter.Image = ListBitmap[trackBar.Value] = ImageUtil.SepiaTone(pictureBoxFilter.Image);

        }

        /// <summary>
        /// Accept all changes.
        /// </summary>
        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel the filters, so nothing changed here will be saved
        /// </summary>
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Convert all images to grayscale filter
        /// </summary>
        private void GrayscaleAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ListBitmap = ImageUtil.GrayScale(ListBitmap);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Apply all images to a Pixelated filter
        /// </summary>
        private void PixelateAll_Click(object sender, EventArgs e)
        {
            ValuePicker valuePicker = new ValuePicker(100, 2, Resources.Msg_PixelSize);
            valuePicker.ShowDialog();

            this.Cursor = Cursors.WaitCursor;
            ListBitmap = ImageUtil.Pixelate(ListBitmap, new Rectangle(0, 0, pictureBoxFilter.Image.Width, pictureBoxFilter.Image.Height), valuePicker.Value);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;
            valuePicker.Dispose();
        }

        /// <summary>
        /// Apply all images to a blur filter
        /// </summary>
        private void BlurAll_Click(object sender, EventArgs e)
        {
            ValuePicker valuePicker = new ValuePicker(5, 1, Resources.Msg_BlurIntense);
            valuePicker.ShowDialog();

            this.Cursor = Cursors.WaitCursor;
            ListBitmap = ImageUtil.Blur(ListBitmap, new Rectangle(0, 0, pictureBoxFilter.Image.Width, pictureBoxFilter.Image.Height), valuePicker.Value);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;

            valuePicker.Dispose();
        }

        /// <summary>
        /// Convert all images to negative filter
        /// </summary>
        private void NegativeAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ListBitmap = ImageUtil.Negative(ListBitmap);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Convert all images to transparency filter
        /// </summary>
        private void TransparencyAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;            
            ListBitmap = ImageUtil.Transparency(ListBitmap);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;
        }        

        /// <summary>
        /// Convert all images to SepiaTone filter
        /// </summary>
        private void sepiaToneAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ListBitmap = ImageUtil.SepiaTone(ListBitmap);
            pictureBoxFilter.Image = ListBitmap[trackBar.Value];
            this.Cursor = Cursors.Default;
        }

        

        
    }
}
