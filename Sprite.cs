using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Exam
{
    class Sprite
    {
        /// <summary>
        /// Path to sprite map
        /// </summary>
        private Uri _spritemapPath;
        /// <summary>
        /// X,Y coordinates of first frame
        /// </summary>
        private DirectionPoint _framePosition;
        /// <summary>
        /// Size of frameIndex in pix.
        /// </summary>
        private Size _frameSizePix;
        /// <summary>
        /// Is animation order is horizontal on sprite map
        /// </summary>
        private bool _isHorizDir;

        /// <summary>
        /// Create animated sprite from sprite map
        /// </summary>
        /// <param name="mapPath">Path to sprite map</param>
        /// <param name="framePosition">X,Y coordinates of first frame</param>
        /// <param name="frameSizePix">Size of frameIndex in pix.</param>
        /// <param name="isHorizDir">Is animation order is horizontal on sprite map</param>
        public Sprite(Uri mapPath, DirectionPoint framePosition, Size frameSizePix, bool isHorizDir)
        {
            _spritemapPath = mapPath;
            _framePosition = framePosition;
            _frameSizePix = frameSizePix;
            _isHorizDir = isHorizDir;
        }

        public Uri Url { get { return _spritemapPath; } set { _spritemapPath = value; } }
        public DirectionPoint Position { get { return _framePosition; } set { _framePosition = value; } }
        public Size FrameSize { get { return _frameSizePix; } set { _frameSizePix = value; } }
        public bool IsHorizontalDirection { get { return _isHorizDir; } set { _isHorizDir = value; } }

        public BitmapSource GetRenderedImage()
        {
            var x = _framePosition.X;
            var y = _framePosition.Y;

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = _spritemapPath;
            bi.EndInit();
            try
            {
                int bytesPerPix = bi.Format.BitsPerPixel / 8;
                int stride = bi.PixelWidth * bytesPerPix;

                var pixelBuffer = new byte[bi.PixelHeight * stride];

                bi.CopyPixels(new System.Windows.Int32Rect(Convert.ToInt32(x), Convert.ToInt32(y),
                        Convert.ToInt32(_frameSizePix.Width), Convert.ToInt32(_frameSizePix.Height)),
                    pixelBuffer, stride, 0);

                //bi.SourceRect = new System.Windows.Int32Rect(Convert.ToInt32(x), Convert.ToInt32(y), 
                //    Convert.ToInt32(_frameSizePix.Width), Convert.ToInt32(_frameSizePix.Height));
                var result = BitmapImage.Create(Convert.ToInt32(_frameSizePix.Width), Convert.ToInt32(_frameSizePix.Width),
                    bi.DpiX, bi.DpiY, bi.Format, bi.Palette, pixelBuffer, stride);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
