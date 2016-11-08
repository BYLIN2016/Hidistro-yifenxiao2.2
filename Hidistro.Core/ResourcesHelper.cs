namespace Hidistro.Core
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public static class ResourcesHelper
    {
        public static bool CheckPostedFile(HttpPostedFile postedFile)
        {
            if ((postedFile == null) || (postedFile.ContentLength == 0))
            {
                return false;
            }
            string str = Path.GetExtension(postedFile.FileName).ToLower();
            if ((((str != ".jpg") && (str != ".gif")) && ((str != ".jpeg") && (str != ".png"))) && (str != ".bmp"))
            {
                return false;
            }
            string str2 = postedFile.ContentType.ToLower();
            if (((((str2 != "image/pjpeg") && (str2 != "image/jpeg")) && ((str2 != "image/gif") && (str2 != "image/bmp"))) && (str2 != "image/png")) && (str2 != "image/x-png"))
            {
                return false;
            }
            return true;
        }

        public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
        {
            Image image = Image.FromFile(sourceFilename);
            if ((image.Width <= width) && (image.Height <= height))
            {
                File.Copy(sourceFilename, destFilename, true);
                image.Dispose();
            }
            else
            {
                int num = image.Width;
                int num2 = image.Height;
                float num3 = ((float) height) / ((float) num2);
                if ((((float) width) / ((float) num)) < num3)
                {
                    num3 = ((float) width) / ((float) num);
                }
                width = (int) (num * num3);
                height = (int) (num2 * num3);
                Image image2 = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(image2);
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                EncoderParameters encoderParams = new EncoderParameters();
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 100L);
                encoderParams.Param[0] = parameter;
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                for (int i = 0; i < imageEncoders.Length; i++)
                {
                    if (imageEncoders[i].FormatDescription.Equals("JPEG"))
                    {
                        encoder = imageEncoders[i];
                        break;
                    }
                }
                image2.Save(destFilename, encoder, encoderParams);
                encoderParams.Dispose();
                parameter.Dispose();
                image.Dispose();
                image2.Dispose();
                graphics.Dispose();
            }
        }

        public static void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                }
            }
        }

        public static string GenerateFilename(string extension)
        {
            return (Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension);
        }
    }
}

