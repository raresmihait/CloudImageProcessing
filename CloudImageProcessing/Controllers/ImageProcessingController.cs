using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.DrawingCore;

namespace CloudImageProcessing.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        [HttpPost]
        public byte[] ApplyGrayscale(IFormFile file)
        {
            var inputImage = new Bitmap(file.OpenReadStream());
            var outputImage = ToGrayscale(inputImage);

            byte[] result = ImageToByte(outputImage);

            return result;
        }

        [HttpPost]
        public byte[] ApplyInvert(IFormFile file)
        {
            var inputImage = new Bitmap(file.OpenReadStream());
            var outputImage = ToInvert(inputImage);

            byte[] result = ImageToByte(outputImage);

            return result;
        }

        [HttpPost]
        public byte[] ApplyContrast(IFormFile file)
        {
            var inputImage = new Bitmap(file.OpenReadStream());
            var outputImage = ToContrast(inputImage);

            byte[] result = ImageToByte(outputImage);

            return result;
        }

        private Bitmap ToGrayscale(Bitmap input)
        {
            Bitmap temp = input;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return (Bitmap)bmap.Clone();
        }

        public Bitmap ToInvert(Bitmap input)
        {
            Bitmap temp = input;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            return (Bitmap)bmap.Clone();
        }

        public Bitmap ToContrast(Bitmap input, double contrast = 50)
        {
            Bitmap bmap = (Bitmap)input.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
            return (Bitmap)bmap.Clone();
        }

        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}