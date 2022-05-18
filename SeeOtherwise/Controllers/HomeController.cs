using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeeOtherwise.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

using System.Drawing;
using System.Drawing.Imaging;
namespace SeeOtherwise.Controllers
{
    public class HomeController : Controller
    {
        
        
        public HomeController()
        {

            
       
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    
        [HttpGet]
        public IActionResult Filter1()
        {


            return View();

        }

        [HttpPost]
        public IActionResult Filter1(Filter1 filter1)
        {
            
            using (StreamWriter sw = new StreamWriter(@"Save.txt"))
            {

                sw.WriteLine(filter1.Photo);

            }
            ViewBag.Photo = filter1.Photo;
            Image image1 = Image.FromFile(filter1.Photo);

            Bitmap newBitmap = new Bitmap(image1);
            Bitmap tmpBitmap = new Bitmap(image1);
            Bitmap tmpBitmap2 = new Bitmap(image1);
            Color IMGcolor;
            int Rpx, Gpx, Bpx, ERpx, EGpx, EBpx; double Apx;
            int rsume, gsume, bsume, Size = 3, margin = ((Size - 1) / 2);
            int[] Filter = { -1, -1, -1, -1, 8, -1, -1, -1, -1 };
            int nom = 1;
            for (int i = 0; i < Size * Size; i++) nom += Filter[i];

            //gray
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    IMGcolor = newBitmap.GetPixel(i, j);
                    Rpx = (int)IMGcolor.R;
                    Gpx = (int)IMGcolor.G;
                    Bpx = (int)IMGcolor.B;

                    Bpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Gpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Rpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);

                    IMGcolor = Color.FromArgb((byte)Rpx, (byte)Gpx, (byte)Bpx);

                    newBitmap.SetPixel(i, j, IMGcolor);
                }
            }

            for (int i = margin; i < newBitmap.Width - margin; i++)
            {
                for (int j = margin; j < newBitmap.Height - margin; j++)
                {
                    rsume = 0;
                    gsume = 0;
                    bsume = 0;

                    for (int k = 0; k < Size; k++)
                        for (int l = 0; l < Size; l++)
                        {
                            IMGcolor = newBitmap.GetPixel(i + k - margin, j + l - margin);
                            Rpx = (int)IMGcolor.R;
                            Gpx = (int)IMGcolor.G;
                            Bpx = (int)IMGcolor.B;

                            rsume += Filter[k * Size + l] * Rpx;
                            gsume += Filter[k * Size + l] * Gpx;
                            bsume += Filter[k * Size + l] * Bpx;
                        }

                    rsume /= nom;
                    gsume /= nom;
                    bsume /= nom;

                    if (rsume > 255) { rsume = 255; }
                    else if (rsume < 0) { rsume = 0; }
                    if (gsume > 255) { gsume = 255; }
                    else if (gsume < 0) { gsume = 0; }
                    if (bsume > 255) { bsume = 255; }
                    else if (bsume < 0) { bsume = 0; }
                    IMGcolor = Color.FromArgb((byte)rsume, (byte)gsume, (byte)bsume);
                    tmpBitmap.SetPixel(i, j, IMGcolor);

                }

            }
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    IMGcolor = tmpBitmap.GetPixel(i, j);
                    Rpx = (int)IMGcolor.R;
                    Gpx = (int)IMGcolor.G;
                    Bpx = (int)IMGcolor.B;

                    if (Rpx < 190)
                    {
                        Rpx = 255;
                        Gpx = 255;
                        Bpx = 255;
                    }
                    else if (Rpx >= 190)
                    {
                        Rpx = 0;
                        Gpx = 0;
                        Bpx = 0;
                    }
                    if (Rpx == 0)
                    {
                        Rpx = newBitmap.GetPixel(i, j).R;
                        Gpx = newBitmap.GetPixel(i, j).G;
                        Bpx = newBitmap.GetPixel(i, j).B;
                    }
                    else
                    {
                        Rpx = newBitmap.GetPixel(i, j).R + 100;
                        if (Rpx > 255) { Rpx = 255; }
                        Gpx = newBitmap.GetPixel(i, j).G + 100;
                        if (Gpx > 255) { Gpx = 255; }
                        Bpx = newBitmap.GetPixel(i, j).B + 100;
                        if (Bpx > 255) { Bpx = 255; }
                    }

                    Bpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Gpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Rpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);

                    IMGcolor = Color.FromArgb((byte)Rpx, (byte)Gpx, (byte)Bpx);
                    tmpBitmap.SetPixel(i, j, IMGcolor);
                    IMGcolor = Color.FromArgb((byte)Rpx, (byte)Gpx, (byte)Bpx);
                    tmpBitmap.SetPixel(i, j, IMGcolor);
                }

            }


            //rewriting
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    newBitmap.SetPixel(i, j, tmpBitmap.GetPixel(i, j));
                }
            }

            int[] Filter2 = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
            nom = 1;
            for (int i = 0; i < Size * Size; i++) nom += Filter2[i];

            for (int i = margin; i < newBitmap.Width - margin; i++)
            {
                for (int j = margin; j < newBitmap.Height - margin; j++)
                {
                    rsume = 0;
                    gsume = 0;
                    bsume = 0;

                    for (int k = 0; k < Size; k++)
                        for (int l = 0; l < Size; l++)
                        {
                            IMGcolor = newBitmap.GetPixel(i + k - margin, j + l - margin);
                            Rpx = (int)IMGcolor.R;
                            Gpx = (int)IMGcolor.G;
                            Bpx = (int)IMGcolor.B;

                            rsume += Filter2[k * Size + l] * Rpx;
                            gsume += Filter2[k * Size + l] * Gpx;
                            bsume += Filter2[k * Size + l] * Bpx;
                        }

                    rsume /= nom;
                    gsume /= nom;
                    bsume /= nom;

                    if (rsume > 255) { rsume = 255; }
                    else if (rsume < 0) { rsume = 0; }
                    if (gsume > 255) { gsume = 255; }
                    else if (gsume < 0) { gsume = 0; }
                    if (bsume > 255) { bsume = 255; }
                    else if (bsume < 0) { bsume = 0; }

                    rsume = 255 - rsume;
                    gsume = 255 - gsume;
                    bsume = 255 - bsume;

                    IMGcolor = Color.FromArgb((byte)rsume, (byte)gsume, (byte)bsume);
                    tmpBitmap.SetPixel(i, j, IMGcolor);

                }

            }
            //rewriting
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    newBitmap.SetPixel(i, j, tmpBitmap.GetPixel(i, j));
                }
            }

            //dispose the Graphics object
            //g.Dispose();
              static byte[] BitmapToBytes(Image img)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }

            filter1.Bitmapa = BitmapToBytes(newBitmap);
            ViewBag.Bitmapa = filter1.Bitmapa;
            return View("filter1w");
        }
        [HttpGet]
        public IActionResult Filter2()
        {


            return View();

        }
        [HttpPost]
        public IActionResult Filter2(Filter2 filter2)
        {

            using (StreamWriter sw = new StreamWriter(@"Save.txt"))
            {

                sw.WriteLine(filter2.Photo);

            }
            ViewBag.Photo = filter2.Photo;
            Image image2 = Image.FromFile(filter2.Photo);

            Bitmap newBitmap = new Bitmap(image2);
            Bitmap tmpBitmap = new Bitmap(image2);
            Color IMGcolor;
            int Rpx, Gpx, Bpx, ERpx, EGpx, EBpx; double Apx;
            int rsume, gsume, bsume, Size = 3, margin = ((Size - 1) / 2);
            int[] Filter = { -1, -1, -1, -1, 8, -1, -1, -1, -1 };
            int nom = 1;
            for (int i = 0; i < Size * Size; i++) nom += Filter[i];

            //gray
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    IMGcolor = newBitmap.GetPixel(i, j);
                    Rpx = (int)IMGcolor.R;
                    Gpx = (int)IMGcolor.G;
                    Bpx = (int)IMGcolor.B;

                    Bpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Gpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Rpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);

                    IMGcolor = Color.FromArgb((byte)Rpx, (byte)Gpx, (byte)Bpx);

                    newBitmap.SetPixel(i, j, IMGcolor);
                }
            }

            for (int i = margin; i < newBitmap.Width - margin; i++)
            {
                for (int j = margin; j < newBitmap.Height - margin; j++)
                {
                    rsume = 0;
                    gsume = 0;
                    bsume = 0;

                    for (int k = 0; k < Size; k++)
                        for (int l = 0; l < Size; l++)
                        {
                            IMGcolor = newBitmap.GetPixel(i + k - margin, j + l - margin);
                            Rpx = (int)IMGcolor.R;
                            Gpx = (int)IMGcolor.G;
                            Bpx = (int)IMGcolor.B;

                            rsume += Filter[k * Size + l] * Rpx;
                            gsume += Filter[k * Size + l] * Gpx;
                            bsume += Filter[k * Size + l] * Bpx;
                        }

                    rsume /= nom;
                    gsume /= nom;
                    bsume /= nom;

                    if (rsume > 255) { rsume = 255; }
                    else if (rsume < 0) { rsume = 0; }
                    if (gsume > 255) { gsume = 255; }
                    else if (gsume < 0) { gsume = 0; }
                    if (bsume > 255) { bsume = 255; }
                    else if (bsume < 0) { bsume = 0; }
                    IMGcolor = Color.FromArgb((byte)rsume, (byte)gsume, (byte)bsume);
                    tmpBitmap.SetPixel(i, j, IMGcolor);

                }

            }
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    IMGcolor = tmpBitmap.GetPixel(i, j);
                    Rpx = (int)IMGcolor.R;
                    Gpx = (int)IMGcolor.G;
                    Bpx = (int)IMGcolor.B;

                    if (Rpx < 190)
                    {
                        Rpx = 255;
                        Gpx = 255;
                        Bpx = 255;
                    }
                    else if (Rpx >= 190)
                    {
                        Rpx = 20;
                        Gpx = 20;
                        Bpx = 20;
                    }

                    if (Rpx == 20)
                    {
                        Rpx = newBitmap.GetPixel(i, j).R;
                        Gpx = newBitmap.GetPixel(i, j).G;
                        Bpx = newBitmap.GetPixel(i, j).B;
                    }
                    else
                    {
                        Rpx = newBitmap.GetPixel(i, j).R + 100;
                        if (Rpx > 255) { Rpx = 255; }
                        Gpx = newBitmap.GetPixel(i, j).G + 100;
                        if (Gpx > 255) { Gpx = 255; }
                        Bpx = newBitmap.GetPixel(i, j).B + 100;
                        if (Bpx > 255) { Bpx = 255; }
                    }

                    Bpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Gpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);
                    Rpx = (int)(Bpx * 0.114 + Gpx * 0.587 + Rpx * 0.299);

                    IMGcolor = Color.FromArgb((byte)Rpx, (byte)Gpx, (byte)Bpx);
                    tmpBitmap.SetPixel(i, j, IMGcolor);
                }
            }

            //rewriting
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    newBitmap.SetPixel(i, j, tmpBitmap.GetPixel(i, j));
                }
            }

            int[] Filter2 = { 1, 1, 1, 0, 1, 0, -1, -1, -1 };
            nom = 0;
            for (int i = 0; i < Size * Size; i++) nom += Filter2[i];

            for (int i = margin; i < newBitmap.Width - margin; i++)
            {
                for (int j = margin; j < newBitmap.Height - margin; j++)
                {
                    rsume = 0;
                    gsume = 0;
                    bsume = 0;

                    for (int k = 0; k < Size; k++)
                        for (int l = 0; l < Size; l++)
                        {
                            IMGcolor = newBitmap.GetPixel(i + k - margin, j + l - margin);
                            Rpx = (int)IMGcolor.R;
                            Gpx = (int)IMGcolor.G;
                            Bpx = (int)IMGcolor.B;

                            rsume += Filter2[k * Size + l] * Rpx;
                            gsume += Filter2[k * Size + l] * Gpx;
                            bsume += Filter2[k * Size + l] * Bpx;
                        }

                    rsume /= nom;
                    gsume /= nom;
                    bsume /= nom;

                    if (rsume > 255) { rsume = 255; }
                    else if (rsume < 0) { rsume = 0; }
                    if (gsume > 255) { gsume = 255; }
                    else if (gsume < 0) { gsume = 0; }
                    if (bsume > 255) { bsume = 255; }
                    else if (bsume < 0) { bsume = 0; }

                    IMGcolor = Color.FromArgb((byte)rsume, (byte)gsume, (byte)bsume);
                    tmpBitmap.SetPixel(i, j, IMGcolor);

                }

            }

            //rewriting
            for (int i = 0; i < newBitmap.Width; i++)
            {
                for (int j = 0; j < newBitmap.Height; j++)
                {
                    newBitmap.SetPixel(i, j, tmpBitmap.GetPixel(i, j));
                }
            }
            //dispose the Graphics object
            //g.Dispose();
            static byte[] BitmapToBytes(Image img)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }

            filter2.Bitmapa = BitmapToBytes(newBitmap);
            ViewBag.Bitmapa = filter2.Bitmapa;
            return View("filter2w");
        }
        [HttpGet]
        public IActionResult Filter3()
        {


            return View();

        }
        [HttpPost]
        public IActionResult Filter3(Filter3 filter3)
        {
            using (StreamWriter sw = new StreamWriter(@"Save.txt"))
            {

                sw.WriteLine(filter3.Photo);

            }
            ViewBag.Photo = filter3.Photo;
            Image image3 = Image.FromFile(filter3.Photo);

            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(image3);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                new float[] {-1, 0, 0, 0, 0},
                new float[] {0, -1, 0, 0, 0},
                new float[] {0, 0, -1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {1, 1, 1, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(newBitmap, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
               0, 0, newBitmap.Width, newBitmap.Height, GraphicsUnit.Pixel, attributes);



            //dispose the Graphics object
            //g.Dispose();
            static byte[] BitmapToBytes(Image img)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }

            filter3.Bitmapa = BitmapToBytes(newBitmap);
            ViewBag.Bitmapa = filter3.Bitmapa;
            return View("filter3w");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
