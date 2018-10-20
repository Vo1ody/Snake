using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Exam
{
    class Apple
    {
        protected Sprite _appleSprite;
        protected double _x, _y;

        public Sprite Sprite { get { return _appleSprite; } }
        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }

        public Apple(double x, double y, Uri spriteMapUrl)
        {
            _x = x;
            _y = y;

            Image img = new Image();
            _appleSprite = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 16 * 4), // Top left coords
                //new DirPoint(1, 69),              // Top left coords
                new Size(16, 16), // Frame size
                true); // Horizontal animation
        }

        public Image GetImage()
        {
            var appleImage = new Image();
            appleImage.Source = this.Sprite.GetRenderedImage();

            return appleImage;
        }
    }

    class SpoiledApple:Apple
    {
        public SpoiledApple(double x, double y, Uri spriteMapUrl):base(x, y, spriteMapUrl)
        {
            _x = x;
            _y = y;

            Image img = new Image();
            _appleSprite = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 16 * 5), // Top left coords
                //new DirPoint(1, 69),              // Top left coords
                new Size(16, 16), // Frame size
                true); // Horizontal animation
        }
    }
}
