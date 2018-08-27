using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Collections.Generic;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using Models.Class;
using Models.Class.FileUpload;

namespace Commons
{
    public static class ImageHelper
    {
        public static void CreateEncryptThumbnail(string path, int width)
        {
            try
            {
                Image image = Image.FromFile(path);
                float ratio = image.Width / width;
                int height = (int)((float)image.Height / ratio);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                thumb.Save(string.Format("{0}\\{1}_thumb.png", Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)), ImageFormat.Png);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        public static void CreateEncryptThumbnail(Stream file, string path, int width)
        {
            try
            {
                Image image = Image.FromStream(file);
                float ratio = image.Width / width;
                int height = (int)((float)image.Height / ratio);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                thumb.Save(string.Format("{0}\\{1}_thumb.png", Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)), ImageFormat.Png);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        public static byte[] GetThumbnailBytes(Stream file, int width)
        {
            byte[] thumbBytes = null;
            try
            {
                Image image = Image.FromStream(file);
                int height = (int)((image.Height * width) / image.Width);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);


                using (var thumbStream = new MemoryStream())
                {
                    thumb.Save(thumbStream, ImageFormat.Png);
                    thumbBytes = thumbStream.ToArray();
                }

                thumb.Dispose();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return thumbBytes;
        }

        /// <summary>
        /// Convert a bdf to an array bytes in order to save a picture
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        public static byte[] ConvertPdfToPngBytes(FileUpload upload)
        {
            byte[] fileBytes = null;
            try
            {
                int desired_x_dpi = 96;
                int desired_y_dpi = 96;

                var _lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion(GhostscriptLicense.GPL | GhostscriptLicense.AFPL, GhostscriptLicense.GPL);
                var _rasterizer = new GhostscriptRasterizer();
                _rasterizer.Open(upload.File.InputStream, _lastInstalledVersion, true);

                List<Image> images = new List<Image>();
                int maxWidth = 0;
                int totalHeight = 0;
                int pageCount = _rasterizer.PageCount > 1 ? 2 : 1;
                for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                {
                    Image img = _rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
                    maxWidth = maxWidth > img.Width ? maxWidth : img.Width;
                    totalHeight = totalHeight + img.Height + 1;
                    images.Add(img);
                }
                _rasterizer.Close();

                Bitmap finalImage = new Bitmap(maxWidth, totalHeight, PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(finalImage))
                {
                    int startPoint = 0;
                    foreach (Image img in images)
                    {
                        graphics.DrawImage(img, new Rectangle(new Point(0, startPoint), img.Size), new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                        startPoint = startPoint + img.Height + 1;
                        img.Dispose();
                    }
                }

                var imgStream = new MemoryStream();
                finalImage.Save(imgStream, ImageFormat.Png);
                upload.ThumbBytes = GetThumbnailBytes(imgStream, 190);


                using (var ms = new MemoryStream())
                {
                    imgStream.Position = 0;
                    imgStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                imgStream.Dispose();
                finalImage.Dispose();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UrlPath = " + upload.UploadName);
            }

            return fileBytes;
        }
    }
}
