using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using JustSay.Common.DotNetCode;
namespace JustSay.Common.DotNetImage
{
    public class ThumbImg
    {
        public ThumbImg(string imgPath,int width,string targetPath)
        {
            ImageHelper.MakeThumbnailAndMark(imgPath, targetPath, width,"JustSay.cn|��˵");
        }
        
    }

    public class ImageHelper
    {




        #region ͼƬ�Ƚ�

        /// <summary>
        /// ����ͼƬ�ĶԱȽ��
        /// </summary>
        public enum CompareResult
        {
            /// <summary>
            /// ͼƬһ��
            /// </summary>
            CompareOk,
            /// <summary>
            /// ���ز�һ��
            /// </summary>
            PixelMismatch,
            /// <summary>
            /// ͼƬ��С�ߴ粻һ��
            /// </summary>
            SizeMismatch
        };
        // ժҪ:
        //     ָ��������ʾ System.Windows.Forms.ImageList �ؼ��е�ͼ�����ɫ����
        public enum ColorDepth
        {
            // ժҪ:
            //     4 λͼ��
            Depth4Bit = 4,
            //
            // ժҪ:
            //     8 λͼ��
            Depth8Bit = 8,
            //
            // ժҪ:
            //     16 λͼ��
            Depth16Bit = 16,
            //
            // ժҪ:
            //     24 λͼ��
            Depth24Bit = 24,
            //
            // ժҪ:
            //     32 λͼ��
            Depth32Bit = 32,
        }


        /// <summary>
        /// �Ա�����ͼƬ
        /// </summary>
        /// <param name="bmp1">The first bitmap image</param>
        /// <param name="bmp2">The second bitmap image</param>
        /// <returns>CompareResult</returns>
        public static CompareResult CompareTwoImages(Bitmap bmp1, Bitmap bmp2)
        {
            CompareResult cr = CompareResult.CompareOk;

            //Test to see if we have the same size of image
            if (bmp1.Size != bmp2.Size)
            {
                cr = CompareResult.SizeMismatch;
            }
            else
            {
                //Convert each image to a byte array
                System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                byte[] btImage1 = new byte[1];
                btImage1 = (byte[])ic.ConvertTo(bmp1, btImage1.GetType());
                byte[] btImage2 = new byte[1];
                btImage2 = (byte[])ic.ConvertTo(bmp2, btImage2.GetType());

                //Compute a hash for each image
                SHA256Managed shaM = new SHA256Managed();
                byte[] hash1 = shaM.ComputeHash(btImage1);
                byte[] hash2 = shaM.ComputeHash(btImage2);

                //Compare the hash values
                for (int i = 0; i < hash1.Length && i < hash2.Length
                                  && cr == CompareResult.CompareOk; i++)
                {
                    if (hash1[i] != hash2[i])
                        cr = CompareResult.PixelMismatch;
                }
            }
            return cr;
        }

        #endregion

        #region ͼƬ����

        public enum ScaleMode
        {
            /// <summary>
            /// ָ���߿����ţ����ܱ��Σ�
            /// </summary>
            HW,
            /// <summary>
            /// ָ�����߰�����
            /// </summary>
            W,
            /// <summary>
            /// ָ���ߣ�������
            /// </summary>
            H,
            /// <summary>
            /// ָ���߿�ü��������Σ�
            /// </summary>
            Cut
        };

        /// <summary>
        /// Resizes the image by a percentage
        /// </summary>
        /// <param name="imgPhoto">The img photo.</param>
        /// <param name="Percent">The percentage</param>
        /// <returns></returns>
        public static Image ResizeImageByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
            }

            return bmPhoto;
        }

        /// <summary>    
        /// ��������ͼ   2014��9��3�� 00:35:07  ��������
        /// </summary> 
        /// <param name="originalImagePath">Դͼ·��������·����</param> 
        /// <param name="thumbnailPath">����ͼ·��������·����</param> 
        /// <param name="width">����ͼ���</param> 
        /// <param name="height">����ͼ�߶�</param>   
        public static Image MakeAThumbnail(Image originalImage, int width, int height)
        {

            int towidth = 0;
            int toheight = 0;
            if(height==0)
             height = originalImage.Height * width / originalImage.Width;
            if (originalImage.Width > width && originalImage.Height < height)
            {
                towidth = width;
                toheight = originalImage.Height;
            }

            if (originalImage.Width < width && originalImage.Height > height)
            {
                towidth = originalImage.Width;
                toheight = height;
            }
            if (originalImage.Width > width && originalImage.Height > height)
            {
                towidth = width;
                toheight = height;
            }
            if (originalImage.Width < width && originalImage.Height < height)
            {
                towidth = originalImage.Width;
                toheight = originalImage.Height;
            }
            int x = 0;//���Ͻǵ�x���� 
            int y = 0;//���Ͻǵ�y���� 


            //�½�һ��bmpͼƬ 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������ 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //���ø�������ֵ�� 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶� 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ��� 
            g.Clear(Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������ 
            g.DrawImage(originalImage, x, y, towidth, toheight);


                originalImage.Dispose();
          
                g.Dispose();

            return bitmap;
        }


        /// <summary>
        /// ���š�����ͼƬ
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Image ResizeImageToAFixedSize(Image originalImage, int width, int height, ScaleMode mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ScaleMode.HW:
                    break;
                case ScaleMode.W:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ScaleMode.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ScaleMode.Cut:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //�½�һ��bmpͼƬ
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������
            using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {
                //���û������������
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //��ջ�������͸������ɫ���
                g.Clear(Color.Transparent);

                //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                    new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);
            }

            return bitmap;
        }


        #endregion

        #region ��������ͼ
        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ScaleMode mode)
        {
            //��jpg��ʽ��������ͼ
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, ImageFormat.Jpeg);
        }

        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ScaleMode mode, System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            System.Drawing.Image bitmap = ResizeImageToAFixedSize(originalImage, width, height, mode);

            try
            {
                bitmap.Save(thumbnailPath, format);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
            }
        }


        /// <summary>
        /// ��������ͼ����ˮӡ,���
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>
        public static void MakeThumbnailAndMark(Stream inputStream, string thumbnailPath, int width,string waterMark)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(inputStream);
            System.Drawing.Image thumbnail;
            if (originalImage.Width > width)//ֻ�д���ָ��ֵʱ������
            {

               //thumbnail = ResizeImageToAFixedSize(originalImage, width, 0, ScaleMode.W);

                thumbnail = MakeAThumbnail(originalImage, width, 0);
            }
            else
            {
                thumbnail = originalImage;
            }
            System.Drawing.Image bitmap = WatermarkText(thumbnail, waterMark);

            try
            {
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// ��������ͼ����ˮӡ,���
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>
        public static void MakeThumbnailAndMark(string originalImagePath, string thumbnailPath, int width,string waterMark)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            if (originalImage.Width < width)
                width = originalImage.Width;

            System.Drawing.Image thumbnail = ResizeImageToAFixedSize(originalImage, width, 0, ScaleMode.W);
            System.Drawing.Image bitmap = WatermarkText(thumbnail, waterMark);

            try
            {
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
            }
        }

        #endregion

        #region ͼƬˮӡ

        /// <summary>
        /// ˮӡλ��
        /// </summary>
        public enum WatermarkPosition
        {
            /// <summary>
            /// ���Ͻ�
            /// </summary>
            TopLeft,
            /// <summary>
            /// ���Ͻ�
            /// </summary>
            TopRight,
            /// <summary>
            /// ���½�
            /// </summary>
            BottomLeft,
            /// <summary>
            /// ���½�
            /// </summary>
            BottomRight,
            /// <summary>
            /// ����
            /// </summary>
            Center
        }

        /// <summary>
        /// ����
        /// </summary>
        private static int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

        /// <summary>
        /// �������ˮӡ   Ĭ�����壺Verdana  12�Ŵ�С  б��  ��ɫ    λ��:���½�
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="text">ˮӡ��</param>
        /// <returns></returns>
        public static Image WatermarkText(Image originalImage, string text)
        {
            return WatermarkText(originalImage, text, WatermarkPosition.BottomRight);
        }

        /// <summary>
        /// �������ˮӡ   Ĭ�����壺Verdana  12�Ŵ�С  б��  ��ɫ
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="text">ˮӡ��</param>
        /// <param name="position">ˮӡλ��</param>
        /// <returns></returns>
        public static Image WatermarkText(Image originalImage, string text, WatermarkPosition position)
        {
            return WatermarkText(originalImage, text, position, new Font("΢���ź�", 14, FontStyle.Italic), new SolidBrush(Color.White));
        }

        /// <summary>
        /// �������ˮӡ
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="text">ˮӡ��</param>
        /// <param name="position">ˮӡλ��</param>
        /// <param name="font">����</param>
        /// <param name="brush">��ɫ</param>
        /// <returns></returns>
        public static Image WatermarkText(Image originalImage, string text, WatermarkPosition position, Font font, Brush brush)
        {
            using (Graphics g = Graphics.FromImage(originalImage))
            {
                Font tempfont = null;
                SizeF sizeF = new SizeF();
                sizeF = g.MeasureString(text, font);

                if (sizeF.Width >= originalImage.Width)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        tempfont = new Font(font.FontFamily, sizes[i], font.Style);

                        sizeF = g.MeasureString(text, tempfont);

                        if (sizeF.Width < originalImage.Width)
                            break;
                    }
                }
                else
                {
                    tempfont = font;
                }

                float x = (float)originalImage.Width * (float)0.01;
                float y = (float)originalImage.Height * (float)0.01;

                switch (position)
                {
                    case WatermarkPosition.TopLeft:
                        break;
                    case WatermarkPosition.TopRight:
                        x = (float)((float)originalImage.Width * (float)0.99 - sizeF.Width);
                        break;
                    case WatermarkPosition.BottomLeft:
                        y = (float)((float)originalImage.Height * (float)0.99 - sizeF.Height);
                        break;
                    case WatermarkPosition.BottomRight:
                        x = (float)((float)originalImage.Width * (float)0.99 - sizeF.Width);
                        y = (float)((float)originalImage.Height * (float)0.99 - sizeF.Height);
                        break;
                    case WatermarkPosition.Center:
                        x = (float)((float)originalImage.Width / 2 - sizeF.Width / 2);
                        y = (float)((float)originalImage.Height / 2 - sizeF.Height / 2);
                        break;
                    default:
                        break;
                }

                g.DrawString(text, tempfont, brush, x, y);

                return originalImage;
            }
        }

        /// <summary>
        /// ���ͼƬˮӡ     λ��Ĭ�����½�
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="watermarkImage">ˮӡImage</param>
        /// <returns></returns>
        public static Image WatermarkImage(Image originalImage, Image watermarkImage)
        {
            return WatermarkImage(originalImage, watermarkImage, WatermarkPosition.BottomRight);
        }

        /// <summary>
        /// ���ͼƬˮӡ
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="watermarkImage">ˮӡImage</param>
        /// <param name="position">λ��</param>
        /// <returns></returns>
        public static Image WatermarkImage(Image originalImage, Image watermarkImage, WatermarkPosition position)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {    new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int x = 0;
            int y = 0;
            int watermarkWidth = 0;
            int watermarkHeight = 0;
            double bl = 1d;

            if ((originalImage.Width > watermarkImage.Width * 4) && (originalImage.Height > watermarkImage.Height * 4))
            {
                bl = 1;
            }
            else if ((originalImage.Width > watermarkImage.Width * 4) && (originalImage.Height < watermarkImage.Height * 4))
            {
                bl = Convert.ToDouble(originalImage.Height / 4) / Convert.ToDouble(watermarkImage.Height);

            }
            else if ((originalImage.Width < watermarkImage.Width * 4) && (originalImage.Height > watermarkImage.Height * 4))
            {
                bl = Convert.ToDouble(originalImage.Width / 4) / Convert.ToDouble(watermarkImage.Width);
            }
            else
            {
                if ((originalImage.Width * watermarkImage.Height) > (originalImage.Height * watermarkImage.Width))
                {
                    bl = Convert.ToDouble(originalImage.Height / 4) / Convert.ToDouble(watermarkImage.Height);

                }
                else
                {
                    bl = Convert.ToDouble(originalImage.Width / 4) / Convert.ToDouble(watermarkImage.Width);
                }
            }

            watermarkWidth = Convert.ToInt32(watermarkImage.Width * bl);
            watermarkHeight = Convert.ToInt32(watermarkImage.Height * bl);

            switch (position)
            {
                case WatermarkPosition.TopLeft:
                    x = 10;
                    y = 10;
                    break;
                case WatermarkPosition.TopRight:
                    y = 10;
                    x = originalImage.Width - watermarkWidth - 10;
                    break;
                case WatermarkPosition.BottomLeft:
                    x = 10;
                    y = originalImage.Height - watermarkHeight - 10;
                    break;
                case WatermarkPosition.BottomRight:
                    x = originalImage.Width - watermarkWidth - 10;
                    y = originalImage.Height - watermarkHeight - 10;
                    break;
                case WatermarkPosition.Center:
                    x = originalImage.Width / 2 - watermarkWidth / 2;
                    y = originalImage.Height / 2 - watermarkHeight / 2;
                    break;
                default:
                    break;
            }
            using (Graphics g = Graphics.FromImage(originalImage))
            {
                g.DrawImage(watermarkImage, new Rectangle(x, y, watermarkWidth, watermarkHeight), 0, 0, watermarkImage.Width, watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return originalImage;
        }

        #endregion


        #region ��������

        /// <summary>
        /// ����JPGʱ��
        /// </summary>
        /// <param name="mimeType"> </param>
        /// <returns>�õ�ָ��mimeType��ImageCodecInfo </returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// ����ͼƬ͸����
        /// </summary>
        /// <param name="imgPic"></param>
        /// <param name="imgOpac"></param>
        /// <returns></returns>
        public static Image SetImgOpacity(Image imgPic, float imgOpac)
        {
            Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);
            Graphics gfxPic = Graphics.FromImage(bmpPic);
            ColorMatrix cmxPic = new ColorMatrix();
            cmxPic.Matrix33 = imgOpac;
            ImageAttributes iaPic = new ImageAttributes();
            iaPic.SetColorMatrix(cmxPic);//, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);
            gfxPic.Dispose();
            return bmpPic;
        }

        ///// <summary>
        ///// �޸�ͼƬ����
        ///// </summary>
        ///// <param name="bmp"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static Bitmap ChangeBrightness(Bitmap bmp, int value)
        //{
        //    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        //    int nOffset = bmpData.Stride - bmp.Width * 3;
        //    int bmpWidth = bmp.Width * 3;
        //    int nVal;

        //    unsafe
        //    {
        //        byte* p = (byte*)(void*)bmpData.Scan0;

        //        for (int y = 0; y < bmp.Height; ++y)
        //        {
        //            for (int x = 0; x < bmpWidth; ++x)
        //            {
        //                nVal = p[0] + value;
        //                if (nVal < 0) nVal = 0;
        //                else if (nVal > 255) nVal = 255;

        //                p[0] = (byte)nVal;
        //                ++p;
        //            }
        //            p += nOffset;
        //        }
        //    }

        //    bmp.UnlockBits(bmpData);

        //    return bmp;
        //}

        #region �ı�ͼƬ��С
        /// <summary>
        /// �ı�ͼƬ��С
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ChangeImageSize(Image img, int width, int height)
        {
            Image bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmp;
        }

        public static Image ChangeImageSize(Image img, int width, int height, bool preserveSize)
        {
            if (preserveSize)
            {
                width = Math.Min(img.Width, width);
                height = Math.Min(img.Height, height);
            }

            Image bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmp;
        }

        public static Image ChangeImageSize(Image img, float percentage)
        {
            int width = (int)(percentage / 100 * img.Width);
            int height = (int)(percentage / 100 * img.Height);
            return ChangeImageSize(img, width, height, false);
        }
        #endregion

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Image CropImage(Image img, Rectangle rect)
        {
            Image bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
            }

            return bmp;
        }

        /// <summary>
        /// ���� -- ��GDI+
        /// </summary>
        /// <param name="b">ԭʼBitmap</param>
        /// <param name="StartX">��ʼ����X</param>
        /// <param name="StartY">��ʼ����Y</param>
        /// <param name="iWidth">���</param>
        /// <param name="iHeight">�߶�</param>
        /// <returns>���ú��Bitmap</returns>
        public static Bitmap CropImage(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }

            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }

            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }

            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        public static Image AddCanvas(Image img, int size)
        {
            Image bmp = new Bitmap(img.Width + size * 2, img.Height + size * 2);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(size, size, img.Width, img.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            }

            return bmp;
        }

        /// <summary>
        /// ��תͼƬ
        /// </summary>
        /// <param name="img"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static Bitmap RotateImage(Image img, float theta)
        {
            Matrix matrix = new Matrix();
            matrix.Translate(img.Width / -2, img.Height / -2, MatrixOrder.Append);
            matrix.RotateAt(theta, new Point(0, 0), MatrixOrder.Append);
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddPolygon(new Point[] { new Point(0, 0), new Point(img.Width, 0), new Point(0, img.Height) });
                gp.Transform(matrix);
                PointF[] pts = gp.PathPoints;

                Rectangle bbox = BoundingBox(img, matrix);
                Bitmap bmpDest = new Bitmap(bbox.Width, bbox.Height);

                using (Graphics gDest = Graphics.FromImage(bmpDest))
                {
                    Matrix mDest = new Matrix();
                    mDest.Translate(bmpDest.Width / 2, bmpDest.Height / 2, MatrixOrder.Append);
                    gDest.Transform = mDest;
                    gDest.CompositingQuality = CompositingQuality.HighQuality;
                    gDest.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gDest.DrawImage(img, pts);
                    return bmpDest;
                }
            }
        }

        private static Rectangle BoundingBox(Image img, Matrix matrix)
        {
            GraphicsUnit gu = new GraphicsUnit();
            Rectangle rImg = Rectangle.Round(img.GetBounds(ref gu));

            Point topLeft = new Point(rImg.Left, rImg.Top);
            Point topRight = new Point(rImg.Right, rImg.Top);
            Point bottomRight = new Point(rImg.Right, rImg.Bottom);
            Point bottomLeft = new Point(rImg.Left, rImg.Bottom);
            Point[] points = new Point[] { topLeft, topRight, bottomRight, bottomLeft };
            GraphicsPath gp = new GraphicsPath(points,
                new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });
            gp.Transform(matrix);
            return Rectangle.Round(gp.GetBounds());
        }

        ///// <summary>
        ///// ��ȡ������Ļ�ľ��ο��
        ///// </summary>
        ///// <returns></returns>
        //public static Rectangle GetScreenBounds()
        //{
        //    Point topLeft = new Point(0, 0);
        //    Point bottomRight = new Point(0, 0);
        //    foreach (Screen screen in Screen.AllScreens)
        //    {
        //        if (screen.Bounds.X < topLeft.X) topLeft.X = screen.Bounds.X;
        //        if (screen.Bounds.Y < topLeft.Y) topLeft.Y = screen.Bounds.Y;
        //        if ((screen.Bounds.X + screen.Bounds.Width) > bottomRight.X) bottomRight.X = screen.Bounds.X + screen.Bounds.Width;
        //        if ((screen.Bounds.Y + screen.Bounds.Height) > bottomRight.Y) bottomRight.Y = screen.Bounds.Y + screen.Bounds.Height;
        //    }
        //    return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X + Math.Abs(topLeft.X), bottomRight.Y + Math.Abs(topLeft.Y));
        //}

        //public static Bitmap MakeBackgroundTransparent(IntPtr hWnd, Image image)
        //{
        //    Region region;
        //    if (NativeMethods.GetWindowRegion(hWnd, out region))
        //    {
        //        Bitmap result = new Bitmap(image.Width, image.Height);

        //        using (Graphics g = Graphics.FromImage(result))
        //        {
        //            if (!region.IsEmpty(g))
        //            {
        //                RectangleF bounds = region.GetBounds(g);
        //                g.Clip = region;
        //                g.DrawImage(image, new RectangleF(new PointF(0, 0), bounds.Size), bounds, GraphicsUnit.Pixel);

        //                return result;
        //            }
        //        }
        //    }

        //    return (Bitmap)image;
        //}

        public static void SaveOneInchPic(Image image, Color transColor, float dpi, string path)
        {
            try
            {
                image = image.Clone() as Image;

                ((Bitmap)image).SetResolution(dpi, dpi);//����ͼƬ��ӡ��dpi
                ImageCodecInfo myImageCodecInfo;
                EncoderParameters myEncoderParameters;
                myImageCodecInfo = ImageHelper.GetCodecInfo("image/jpeg");
                myEncoderParameters = new EncoderParameters(2);
                myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, (long)ColorDepth.Depth32Bit);

                //image.Save(path,ImageFormat.Jpeg,)
                image.Save(path, myImageCodecInfo, myEncoderParameters);
                image.Dispose();
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(ex, "ͼ�����쳣", "ImageHelper");
            }
        }

        #endregion

        #region BASE64����ת��
        public static Bitmap Base64StrToBmp(string ImgBase64Str)
        {
            byte[] ImgBuffer = Convert.FromBase64String(ImgBase64Str);
            MemoryStream MStream = new MemoryStream(ImgBuffer);
            Bitmap Bmp = new Bitmap(MStream);
            return Bmp;
        }

        public static string ImageToBase64Str(string ImgName)
        {
            Image Img = Image.FromFile(ImgName);
            System.IO.MemoryStream MStream = new System.IO.MemoryStream();
            Img.Save(MStream, ImageFormat.Jpeg);
            byte[] ImgBuffer = MStream.GetBuffer();
            string ImgBase64Str = Convert.ToBase64String(ImgBuffer);
            //�ͷ���Դ���ñ��ʹ��
            Img.Dispose();
            return ImgBase64Str;
        }
        public static string ImageToBase64Str(Image Img)
        {
            System.IO.MemoryStream MStream = new System.IO.MemoryStream();
            Img.Save(MStream, ImageFormat.Jpeg);
            byte[] ImgBuffer = MStream.GetBuffer();
            string ImgBase64Str = Convert.ToBase64String(ImgBuffer);
            return ImgBase64Str;
        }
        #endregion

        #region ɫ�ʴ���
        /// <summary>  
        /// ����ͼ����ɫ  ��Ե��ɫ�ʸ������µ���ɫ  
        /// </summary>  
        /// <param name="p_Image">ͼƬ</param>  
        /// <param name="p_OldColor">�ϵı�Եɫ��</param>  
        /// <param name="p_NewColor">�µı�Եɫ��</param>  
        /// <param name="p_Float">�ܲ�</param>  
        /// <returns>������ͼ��</returns>  
        public static Image SetImageColorBrim(Image p_Image, Color p_OldColor, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);

            int _Index = 0;
            #region ��
            for (int z = 0; z != _Height; z++)
            {
                _Index = z * _Data.Stride;
                for (int i = 0; i != _Width; i++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);

                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index += 4;
                    }
                    else
                    {
                        break;
                    }
                }
                _Index = (z + 1) * _Data.Stride;
                for (int i = 0; i != _Width; i++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index - 1], _DataBytes[_Index - 2], _DataBytes[_Index - 3], _DataBytes[_Index - 4]);

                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index - 1] = (byte)p_NewColor.A;
                        _DataBytes[_Index - 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index - 3] = (byte)p_NewColor.G;
                        _DataBytes[_Index - 4] = (byte)p_NewColor.B;
                        _Index -= 4;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            #region ��

            for (int i = 0; i != _Width; i++)
            {
                _Index = i * 4;
                for (int z = 0; z != _Height; z++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index += _Data.Stride;
                    }
                    else
                    {
                        break;
                    }
                }
                _Index = (i * 4) + ((_Height - 1) * _Data.Stride);
                for (int z = 0; z != _Height; z++)
                {
                    Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                    if (ScanColor(_Color, p_OldColor, p_Float))
                    {
                        _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                        _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                        _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                        _DataBytes[_Index] = (byte)p_NewColor.B;
                        _Index -= _Data.Stride;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            #endregion
            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>  
        /// ����ͼ����ɫ  ���е�ɫ�ʸ������µ���ɫ  
        /// </summary>  
        /// <param name="p_Image">ͼƬ</param>  
        /// <param name="p_OdlColor">�ϵ���ɫ</param>  
        /// <param name="p_NewColor">�µ���ɫ</param>  
        /// <param name="p_Float">�ܲ�</param>  
        /// <returns>������ͼ��</returns>  
        public static Image SetImageColorAll(Image p_Image, Color p_OdlColor, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);

            int _WhileCount = _Width * _Height;
            int _Index = 0;
            for (int i = 0; i != _WhileCount; i++)
            {
                Color _Color = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);
                if (ScanColor(_Color, p_OdlColor, p_Float))
                {
                    _DataBytes[_Index + 3] = (byte)p_NewColor.A;
                    _DataBytes[_Index + 2] = (byte)p_NewColor.R;
                    _DataBytes[_Index + 1] = (byte)p_NewColor.G;
                    _DataBytes[_Index] = (byte)p_NewColor.B;
                }
                _Index += 4;
            }
            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>  
        /// ����ͼ����ɫ  �������ɫ�������µ�ɫ�� ��©����  
        /// </summary>  
        /// <param name="p_Image">��ͼ��</param>  
        /// <param name="p_Point">λ��</param>  
        /// <param name="p_NewColor">�µ�ɫ��</param>  
        /// <param name="p_Float">�ܲ�</param>  
        /// <returns>������ͼ��</returns>  
        public static Image SetImageColorPoint(Image p_Image, Point p_Point, Color p_NewColor, int p_Float)
        {
            int _Width = p_Image.Width;
            int _Height = p_Image.Height;

            if (p_Point.X > _Width - 1) return p_Image;
            if (p_Point.Y > _Height - 1) return p_Image;

            Bitmap _SS = (Bitmap)p_Image;
            Color _Scolor = _SS.GetPixel(p_Point.X, p_Point.Y);
            Bitmap _NewBmp = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphics = Graphics.FromImage(_NewBmp);
            _Graphics.DrawImage(p_Image, new Rectangle(0, 0, _Width, _Height));
            _Graphics.Dispose();

            BitmapData _Data = _NewBmp.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _Data.PixelFormat = PixelFormat.Format32bppArgb;
            int _ByteSize = _Data.Stride * _Height;
            byte[] _DataBytes = new byte[_ByteSize];
            Marshal.Copy(_Data.Scan0, _DataBytes, 0, _ByteSize);


            int _Index = (p_Point.Y * _Data.Stride) + (p_Point.X * 4);

            Color _OldColor = Color.FromArgb(_DataBytes[_Index + 3], _DataBytes[_Index + 2], _DataBytes[_Index + 1], _DataBytes[_Index]);

            if (_OldColor.Equals(p_NewColor)) return p_Image;
            Stack<Point> _ColorStack = new Stack<Point>(1000);
            _ColorStack.Push(p_Point);

            _DataBytes[_Index + 3] = (byte)p_NewColor.A;
            _DataBytes[_Index + 2] = (byte)p_NewColor.R;
            _DataBytes[_Index + 1] = (byte)p_NewColor.G;
            _DataBytes[_Index] = (byte)p_NewColor.B;

            do
            {
                Point _NewPoint = (Point)_ColorStack.Pop();

                if (_NewPoint.X > 0) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X - 1, _NewPoint.Y, _OldColor, p_NewColor, p_Float);
                if (_NewPoint.Y > 0) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X, _NewPoint.Y - 1, _OldColor, p_NewColor, p_Float);

                if (_NewPoint.X < _Width - 1) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X + 1, _NewPoint.Y, _OldColor, p_NewColor, p_Float);
                if (_NewPoint.Y < _Height - 1) SetImageColorPoint(_DataBytes, _Data.Stride, _ColorStack, _NewPoint.X, _NewPoint.Y + 1, _OldColor, p_NewColor, p_Float);

            }
            while (_ColorStack.Count > 0);

            Marshal.Copy(_DataBytes, 0, _Data.Scan0, _ByteSize);
            _NewBmp.UnlockBits(_Data);
            return _NewBmp;
        }
        /// <summary>  
        /// SetImageColorPoint ѭ������ ����µ������Ƿ�������� ����������д��ջp_ColorStack ��������ɫ  
        /// </summary>  
        /// <param name="p_DataBytes">������</param>  
        /// <param name="p_Stride">��ɨ���ֽ���</param>  
        /// <param name="p_ColorStack">��Ҫ����λ��ջ</param>  
        /// <param name="p_X">λ��X</param>  
        /// <param name="p_Y">λ��Y</param>  
        /// <param name="p_OldColor">��ɫ��</param>  
        /// <param name="p_NewColor">��ɫ��</param>  
        /// <param name="p_Float">�ܲ�</param>  
        private static void SetImageColorPoint(byte[] p_DataBytes, int p_Stride, Stack<Point> p_ColorStack, int p_X, int p_Y, Color p_OldColor, Color p_NewColor, int p_Float)
        {

            int _Index = (p_Y * p_Stride) + (p_X * 4);
            Color _OldColor = Color.FromArgb(p_DataBytes[_Index + 3], p_DataBytes[_Index + 2], p_DataBytes[_Index + 1], p_DataBytes[_Index]);

            if (ScanColor(_OldColor, p_OldColor, p_Float))
            {
                p_ColorStack.Push(new Point(p_X, p_Y));

                p_DataBytes[_Index + 3] = (byte)p_NewColor.A;
                p_DataBytes[_Index + 2] = (byte)p_NewColor.R;
                p_DataBytes[_Index + 1] = (byte)p_NewColor.G;
                p_DataBytes[_Index] = (byte)p_NewColor.B;
            }
        }

        /// <summary>  
        /// ���ɫ��(���Ը���������ıȽϷ�ʽ  
        /// </summary>  
        /// <param name="p_CurrentlyColor">��ǰɫ��</param>  
        /// <param name="p_CompareColor">�Ƚ�ɫ��</param>  
        /// <param name="p_Float">�ܲ�</param>  
        /// <returns></returns>  
        private static bool ScanColor(Color p_CurrentlyColor, Color p_CompareColor, int p_Float)
        {
            int _R = p_CurrentlyColor.R;
            int _G = p_CurrentlyColor.G;
            int _B = p_CurrentlyColor.B;

            return (_R <= p_CompareColor.R + p_Float && _R >= p_CompareColor.R - p_Float) && (_G <= p_CompareColor.G + p_Float && _G >= p_CompareColor.G - p_Float) && (_B <= p_CompareColor.B + p_Float && _B >= p_CompareColor.B - p_Float);

        }
        #endregion

        #region ͼ��ѹ��
        /// <summary>
        /// ��Bitmap����ѹ��ΪJPGͼƬ����
        /// </summary>
        /// </summary>
        /// <param name="bmp">Դbitmap����</param>
        /// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
        /// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
        public static void CompressAsJPG(Bitmap bmp, string saveFilePath, int quality)
        {
            EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality); ;
            EncoderParameters ps = new EncoderParameters(1);
            ps.Param[0] = p;
            bmp.Save(saveFilePath, GetCodecInfo("image/jpeg"), ps);
            bmp.Dispose();
        }


        /// <summary>
        /// ��Bitmap����ѹ��ΪJPGͼƬ����
        /// </summary>
        /// </summary>
        /// <param name="bmp">Դbitmap����</param>
        /// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
        /// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
        public static void CompressAsJPGAndMark(Stream inputStream, string saveFilePath, int quality,string waterMark)
        {
            EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality); ;
            EncoderParameters ps = new EncoderParameters(1);
            ps.Param[0] = p;
            System.Drawing.Image bitmap = WatermarkText(new Bitmap(inputStream), waterMark);
            bitmap.Save(saveFilePath, GetCodecInfo("image/jpeg"), ps);
            bitmap.Dispose();
        }


        /// <summary>
        /// ��inputStream�еĶ���ѹ��ΪJPGͼƬ����
        /// </summary>
        /// <param name="inputStream">ԴStream����</param>
        /// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
        /// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
        public static void CompressAsJPG(Stream inputStream, string saveFilePath, int quality)
        {
            CompressAsJPG(new Bitmap(inputStream), saveFilePath, quality);
        }



        /// <summary>
        /// ��path�еĶ���ѹ��ΪJPGͼƬ����
        /// </summary>
        /// <param name="path">Path����</param>
        /// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
        /// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
        public static void CompressAsJPG(string path, string saveFilePath, int quality)
        {
            CompressAsJPG(new Bitmap(path), saveFilePath, quality);
        }
        #endregion

        #region ͼƬת��
        /// <summary>
        /// Converts the specified <see cref="System.Drawing.Image"/> into an array of bytes
        /// </summary>
        /// <param name="image"><see cref="System.Drawing.Image"/></param>
        /// <returns>Array of bytes</returns>
        public static byte[] ImageToBytes(System.Drawing.Image image)
        {
            byte[] bytes = null;
            if (image != null)
            {
                lock (image)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (ms)
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        bytes = ms.GetBuffer();
                    }
                }
            }
            return bytes;
        }

        /// <summary>
        /// Converts an array of bytes into an <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="bytes">The array of bytes</param>
        /// <returns>The resulting <see cref="System.Drawing.Image"/></returns>
        public static System.Drawing.Image ImageFromBytes(byte[] bytes)
        {
            System.Drawing.Image image = null;
            try
            {
                if (bytes != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes, false);
                    using (ms)
                    {
                        image = ImageFromStream(ms);
                    }
                }
            }
            catch
            {
            }
            return image;
        }

        /// <summary>
        /// Converts a url (filesystem or web) into an <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="url">The url path to the image</param>
        /// <returns>The resulting <see cref="System.Drawing.Image"/></returns>
        public static System.Drawing.Image ImageFromUrl(string url)
        {
            System.Drawing.Image image = null;
            try
            {
                if (!String.IsNullOrEmpty(url))
                {
                    Uri uri = new Uri(url);
                    if (uri.IsFile)
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(uri.LocalPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        using (fs)
                        {
                            image = ImageFromStream(fs);
                        }
                    }
                    else
                    {
                        System.Net.WebClient wc = new System.Net.WebClient();   // TODO: consider changing this to WebClientEx
                        using (wc)
                        {
                            byte[] bytes = wc.DownloadData(uri);
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes, false);
                            using (ms)
                            {
                                image = ImageFromStream(ms);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return image;
        }

        private static System.Drawing.Image ImageFromStream(System.IO.Stream stream)
        {
            System.Drawing.Image image = null;
            try
            {
                stream.Position = 0;
                System.Drawing.Image tempImage = System.Drawing.Bitmap.FromStream(stream);
                // dont close stream yet, first create a copy
                using (tempImage)
                {
                    image = new Bitmap(tempImage);
                }
            }
            catch
            {
                // the file could be an .ico file that is not usable by the Image class, so try that just in case
                try
                {
                    stream.Position = 0;
                    Icon icon = new Icon(stream);
                    if (icon != null) image = icon.ToBitmap();
                }
                catch
                {
                    // give up - we cant open this file type
                }
            }

            return image;
        }
        #endregion
     
    }
}