using System;
using System.Drawing;
using System.IO;

namespace DotNet.Common
{
    /// <summary>
    /// 验证码操作类
    /// </summary>
    public class ValidCodeHelper /*: System.Web.UI.Page*/
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Page_Load(object sender, EventArgs e)
        //{
        //    var verificationCode = RandomHelper.GetRandomString(4);
        //    Session["verificationCode"] = verificationCode;
        //    CreateImage(verificationCode);
        //}

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="checkCode"></param>
        public static MemoryStream CreateImage(string checkCode)
        {
            System.IO.MemoryStream ms = null;

            //创建画布
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);

            //清空图片背景色
            g.Clear(Color.White);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                
                //画验证码字符串
                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Strikeout));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Maroon, Color.Red, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //输出到浏览器
                ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);                
                //Response.ClearContent();
                //Response.ContentType = "image/Gif";
                //Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return ms;
        }
    }
}
