using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace TestCore.Common.Helper
{
    public class QrCodeHelper
    {
       
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="contents">要生成二维码包含的信息</param>
        /// <param name="width">生成的二维码宽度（默认300像素）</param>
        /// <param name="height">生成的二维码高度（默认300像素）</param>
        /// <returns>二维码图片</returns>
        public static byte[] GeneratorQrImage(string contents, int width = 300, int height = 300)
        {
            if (string.IsNullOrEmpty(contents))
            {
                return null;
            }
            var writerSvg = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    ErrorCorrection = ErrorCorrectionLevel.H,
                     CharacterSet= "UTF-8",
                    DisableECI = true,
                    Width = width,
                    Height = height,
                }
            };
            var pixelData = writerSvg.Write(contents);
            return ConvertPixelDataToByteArray(pixelData);
        }

        /// <summary>
        /// 生成中间带有图片的二维码图片
        /// </summary>
        /// <param name="contents">要生成二维码包含的信息</param>
        /// <param name="middleImg">要生成到二维码中间的图片</param>
        /// <param name="width">生成的二维码宽度（默认300像素）</param>
        /// <param name="height">生成的二维码高度（默认300像素）</param>
        /// <returns>中间带有图片的二维码</returns>
        public static Bitmap GeneratorQrImage(string contents, Image middleImg, int width = 300, int height = 300)
        {
            if (string.IsNullOrEmpty(contents))
            {
                return null;
            }
            if (middleImg == null)
            {
                return null;
                //return GeneratorQrImage(contents);
            }
            ////本文地址：http://www.cnblogs.com/Interkey/p/qrcode.html
            //构造二维码写码器
            MultiFormatWriter mutiWriter = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            //生成二维码
            BitMatrix bm = mutiWriter.encode(contents, BarcodeFormat.QR_CODE, width, height, hint);

            var writerSvg = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    ErrorCorrection = ErrorCorrectionLevel.H,
                    CharacterSet = "UTF-8",
                    DisableECI = true,
                    Width = width,
                    Height = height,
                }
            };
            var pixelData = writerSvg.Write(bm);

            Bitmap bitmap = ConvertToBitmap(pixelData);

            //BarcodeWriter barcodeWriter = new BarcodeWriter();
            //Bitmap bitmap = barcodeWriter.Write(bm);

            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleImgW = Math.Min((int)(rectangle[2] / 3.5), middleImg.Width);
            int middleImgH = Math.Min((int)(rectangle[3] / 3.5), middleImg.Height);
            int middleImgL = (pixelData.Width - middleImgW) / 2;
            int middleImgT = (pixelData.Height - middleImgH) / 2;

            //将img转换成bmp格式，否则后面无法创建 Graphics对象
            Bitmap bmpimg = new Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.DrawingCore.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.DrawingCore.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.DrawingCore.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bitmap, 0, 0);
            }

            //在二维码中插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleImgL, middleImgT, middleImgW, middleImgH);
            myGraphic.DrawImage(middleImg, middleImgL, middleImgT, middleImgW, middleImgH);
            return bmpimg;
        }

        static Bitmap ConvertToBitmap(PixelData pixelData)
        {
            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.DrawingCore.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.DrawingCore.Imaging.ImageLockMode.WriteOnly, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);

                var p = Convert.ToBase64String(ms.ToArray());
                return bitmap;
            }
        }

        static byte[] ConvertPixelDataToByteArray(PixelData pixelData)
        {
            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.DrawingCore.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.DrawingCore.Imaging.ImageLockMode.WriteOnly, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                bitmap.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                // save to stream as PNG
                return ms.ToArray();
            }
        }
    }
}
