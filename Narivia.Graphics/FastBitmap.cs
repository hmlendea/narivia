using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Narivia.Graphics
{
    /// <summary>
    /// Fast Bitmap.
    /// </summary>
    public class FastBitmap : IDisposable
    {
        readonly Bitmap source;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData;
        bool bitsLocked;

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>The pixels.</value>
        public byte[] Pixels { get; set; }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; private set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="sourceBitmap">Source bitmap.</param>
        public FastBitmap(Bitmap sourceBitmap)
        {
            source = sourceBitmap;
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="sourceImage">Source image.</param>
        public FastBitmap(Image sourceImage)
        {
            source = (Bitmap)sourceImage;
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FastBitmap(string fileName)
        {
            source = new Bitmap(fileName);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="resource">Resource.</param>
        public FastBitmap(Type type, string resource)
        {
            source = new Bitmap(type, resource);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public FastBitmap(int width, int height)
        {
            source = new Bitmap(width, height);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="format">Format.</param>
        public FastBitmap(int width, int height, PixelFormat format)
        {
            source = new Bitmap(width, height, format);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBitmap"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="stride">Stride.</param>
        /// <param name="format">Format.</param>
        /// <param name="scan0">Scan0.</param>
        public FastBitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            source = new Bitmap(width, height, stride, format, scan0);
            LockBits();
        }

        /// <summary>
        /// Locks the bitmap data
        /// </summary>
        public void LockBits()
        {
            if (bitsLocked)
            {
                return;
            }

            bitsLocked = true;

            Width = source.Width;
            Height = source.Height;

            int pixelCount = Width * Height;

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            Depth = Image.GetPixelFormatSize(source.PixelFormat);

            if (Depth != 8 && Depth != 24 && Depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }

            bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite, source.PixelFormat);

            int step = Depth / 8;
            Pixels = new byte[pixelCount * step];
            Iptr = bitmapData.Scan0;

            Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
        }

        /// <summary>
        /// Unlocks the bitmap data
        /// </summary>
        public void UnlockBits()
        {
            if (!bitsLocked)
            {
                return;
            }

            Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);
            source.UnlockBits(bitmapData);
        }

        /// <summary>
        /// Gets the colour of the specified pixel
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Pixel colour</returns>
        public Color GetPixel(int x, int y)
        {
            Color colour = new Color();
            byte a, r, g, b;
            int colorComponentsCount = Depth / 8;
            int index = ((y * Width) + x) * colorComponentsCount;

            if (index > Pixels.Length - colorComponentsCount)
            {
                throw new IndexOutOfRangeException();
            }

            switch (Depth)
            {
                case 32:
                    b = Pixels[index];
                    g = Pixels[index + 1];
                    r = Pixels[index + 2];
                    a = Pixels[index + 3];
                    colour = Color.FromArgb(a, r, g, b);
                    break;

                case 24:
                    b = Pixels[index];
                    g = Pixels[index + 1];
                    r = Pixels[index + 2];
                    colour = Color.FromArgb(r, g, b);
                    break;

                case 8:
                    b = Pixels[index];
                    colour = Color.FromArgb(b, b, b);
                    break;
            }

            return colour;
        }

        /// <summary>
        /// Sets the colour of the specified pixel
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="colour">Pixel colour</param>
        public void SetPixel(int x, int y, Color colour)
        {
            int colorComponentsCount = Depth / 8;
            int index = ((y * Width) + x) * colorComponentsCount;

            switch (Depth)
            {
                case 32:
                    Pixels[index] = colour.B;
                    Pixels[index + 1] = colour.G;
                    Pixels[index + 2] = colour.R;
                    Pixels[index + 3] = colour.A;
                    break;

                case 24:
                    Pixels[index] = colour.B;
                    Pixels[index + 1] = colour.G;
                    Pixels[index + 2] = colour.R;
                    break;

                case 8:
                    Pixels[index] = colour.B;
                    break;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="FastBitmap"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="FastBitmap"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="FastBitmap"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="FastBitmap"/>
        /// so the garbage collector can reclaim the memory that the <see cref="FastBitmap"/> was occupying.</remarks>
        public void Dispose()
        {
            UnlockBits();
            source.Dispose();
        }

        /// <param name="fbmp">FastBitmap.</param>
        public static implicit operator Bitmap(FastBitmap fbmp)
        {
            return fbmp.source;
        }

        /// <param name="bmp">Bitmap.</param>
        public static implicit operator FastBitmap(Bitmap bmp)
        {
            return new FastBitmap(bmp);
        }

        /// <param name="fbmp">FastBitmap.</param>
        public static implicit operator Image(FastBitmap fbmp)
        {
            return fbmp.source;
        }

        /// <param name="img">Image.</param>
        public static implicit operator FastBitmap(Image img)
        {
            return new FastBitmap(img);
        }
    }
}
