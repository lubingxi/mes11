using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;

namespace PrintLib.Printers.Zebra
{
    using System = global::System;
    using ThoughtWorks.QRCode.Codec;
    using System.Drawing;
    using System.Drawing.Printing;
    public class Printer
    {
        public string Name;
        #region 从Web.config文件中获取打印机名称，如
        public Printer()
        {
            this.Name = System.Configuration.ConfigurationManager.AppSettings["Printer"];
        }

        public Printer(string name)
        {
            this.Name = name;
        }
        #endregion

        #region DLL声明
        //ZPL
        [DllImport(@"FNTHEX32.DLL", CharSet = CharSet.Ansi)]
        public static extern int GETFONTHEX(
                          string chnstr,
                          string fontname,
                          string chnname,
                          int orient,
                          int height,
                          int width,
                          int bold,
                          int italic,
                          StringBuilder param1);
        //EPL
        [DllImport(@"Eltronp.dll", CharSet = CharSet.Ansi)]
        public static extern int PrintHZ(int Lpt, //0：LPT1，1 LPT2
                                         int x,
                                         int y,
                                         string HZBuf,
                                         string FontName,
                                         int FontSize,
                                         int FontStyle);
        #endregion

        #region 指令说明
        /**
        ^XA 开始 ^XZ 结束
        ^LH起始坐标  ^PR进纸回纸速度 ^MD 对比度
        ^FO标签左上角坐标  ^XG打印图片参数1图片名称后两个为坐标
        ^FS标签结束符  ^CI切换国际字体 ^FT坐标 ^FD定义一个字符串
        ^A定义字体  ^FH十六进制数 ^BY模块化label ^BC条形码128  
        ^PQ打印设置 参数一 打印数量 参数二暂停 参数三重复数量  参数四为Y时表明无暂停
         **/
        #endregion



        #region 条码、二维码图片生成
        /// <summary>
        /// 生成条形码图片
        /// </summary>
        /// <param name="num">条形码序列号</param>
        /// <param name="path">图片存放路径（绝对路径）</param>
        /// <returns>返回图片</returns>
        public System.Drawing.Image CreateBarcodeImage(string num, string path)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);
            b.LabelFont = font;
            try
            {
                System.Drawing.Image image = b.Encode(BarcodeLib.TYPE.CODE128B, num);
                image.Save(path);
                return image;
            }
            catch (Exception)
            {

            }
            return null;
        }
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="num">二维码序列号</param>
        /// <param name="path">图片存放路径（绝对路径）</param>
        /// <returns>返回图片</returns>
        public System.Drawing.Image CreateQRCodeImage(string num,string path)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            String encoding = "Byte";
            if (encoding == "Byte")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16(4);
                qrCodeEncoder.QRCodeScale = scale;
            }
            catch (Exception)
            {

            }
            try
            {
                int version = Convert.ToInt16(7);
                qrCodeEncoder.QRCodeVersion = version;
            }
            catch (Exception)
            {

            }

            string errorCorrect = "M";
            if (errorCorrect == "L")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            try
            {
                Bitmap bm = qrCodeEncoder.Encode(num);
                bm.Save(path);
                MemoryStream ms = new MemoryStream();
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return System.Drawing.Image.FromStream(ms);
            }
            catch (Exception)
            {

            }
            return null;
        }
        #endregion

    }
}
