using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TiledGGD
{
    abstract class BrowseableData
    {
        #region Fields

        #region Field: Offset
        /// <summary>
        /// The current offset of the data
        /// </summary>
        private long offset = 0;
        /// <summary>
        /// The current offset of the data
        /// </summary>
        protected long Offset
        {
            get { return this.offset; }
            set
            {
                long newoffset = Math.Max(0, Math.Min(this.data.Length - 1, value));
                if (newoffset != this.offset)
                {
                    this.offset = newoffset;
                    MainWindow.DoRefresh();
                }

            }
        }
        #endregion

        #region Field: SkipSize
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        private long skipSize = 1;
        /// <summary>
        /// How far the data will be skipped ahead/back when pushing the appropriate button
        /// </summary>
        internal long SkipSize { get { return this.skipSize; } set { this.skipSize = Math.Abs(value); } }
        #endregion

        #region Field: SkipMethod
        /// <summary>
        /// The method used to skip data.
        /// </summary>
        private SkipMetric skipMeth;
        internal SkipMetric SkipMethod
        {
            get { return this.skipMeth; }
            set { this.skipMeth = value; }
        }
        #endregion

        #region Field: Data
        /// <summary>
        /// The actual data. Will only contiain bytes.
        /// </summary>
        private byte[] data;
        /// <summary>
        /// The actual data. Should only be set within the load() method.
        /// </summary>
        protected byte[] Data { get { return this.data; } set { this.data = value; } }
        /// <summary>
        /// Get a byte of data
        /// </summary>
        /// <param name="idx">The index of the byte</param>
        /// <returns>The byte at index idx, or 0 if it's out of range</returns>
        protected byte getData(long idx) { try { return this.data[idx]; } catch (IndexOutOfRangeException) { return 0; } }
        #endregion

        #endregion

        /// <summary>
        /// Load a file, and interpret it as BrowseableData. (either graphics or palette)
        /// </summary>
        /// <param name="filename">The name of the file to load</param>
        internal abstract void load(string filename);

        /// <summary>
        /// Skip SkipSize data
        /// </summary>
        /// <param name="positive">If the skip is to be in the positive direction.</param>
        internal void DoSkip(bool positive)
        {
            if (positive)
                this.Offset += this.SkipSize;
            else
                this.Offset -= this.SkipSize;
        }

        /// <summary>
        /// Loads the data of a file blindly; all bytes are copied.
        /// </summary>
        /// <param name="filename">The name of the file to load</param>
        protected void loadGenericData(String filename)
        {
            FileStream fstr = new FileStream(filename, FileMode.Open);
            this.data = new byte[fstr.Length];
            for (long l = 0; l < fstr.Length; l++)
                data[l] = (byte)fstr.ReadByte();
            fstr.Close();
        }

        /// <summary>
        /// Paint the data in some way
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal abstract void paint(object sender, PaintEventArgs e);

        /// <summary>
        /// Copy the currently shown data onto the clipboard
        /// </summary>
        internal abstract void copyToClipboard();
    }

    public enum SkipMetric
    {
        BYTES,
        ELEMENTS,
        SCRWIDTH,
        SCRHEIGHT
    }
}
