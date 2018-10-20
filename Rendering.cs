using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Exam
{
    class Rendering : Snake
    {
        private Sprite _spriteTongue;
        private Sprite _spriteHead;
        private Sprite _spriteBody;
        private Sprite _spriteBodyRotate;
        private Sprite _spriteTail;

        public Sprite TongueSprite { get { return _spriteTongue; } }
        public Sprite HeadSprite { get { return _spriteHead; } }
        public Sprite BodySprite { get { return _spriteBody; } }
        public Sprite BodyRotateSprite { get { return _spriteBodyRotate; } }
        public Sprite TailSprite { get { return _spriteTail; } }

        public Rendering(DirectionPoint tongue, DirectionPoint head, DirectionPoint tail, List<DirectionPoint> body, Uri spriteMapUrl)
        {
            Image img = new Image();
            Tongue = tongue;
            Head = head;
            Tail = tail;
            BodyPoints = body;

            //Tongue
            _spriteTongue = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 96), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Head
            _spriteHead = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 32), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Body
            _spriteBody = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 0), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            _spriteBodyRotate = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 16), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Tail
            _spriteTail = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 48), // Top left coords
                new Size(16, 16), // Frame size
                true); // Horizontal animation
        }

        public Rendering(DirectionPoint tongue, DirectionPoint head, DirectionPoint tail, List<DirectionPoint> body,
            Sprite tongueSprite, Sprite headSprite, Sprite bodySprite, Sprite bodyRotateSprite, Sprite tailSprite)
        {
            Image img = new Image();
            //_snake = snake;
            Tongue = tongue;
            Head = head;
            Tail = tail;
            BodyPoints = body;
            //Tongue
            _spriteTongue = tongueSprite;
            // Head
            _spriteHead = headSprite;
            // Body
            _spriteBody = bodySprite;
            _spriteBodyRotate = bodyRotateSprite;
            // Tail
            _spriteTail = tailSprite;
        }

        public Rendering(DirectionPoint tongue, DirectionPoint head, DirectionPoint tail, Uri spriteMapUrl)
        {
            Image img = new Image();
            //_snake = snake;
            Tongue = tongue;
            Head = head;
            Tail = tail;
            BodyPoints = new List<DirectionPoint>();
            //Tongue
            _spriteTongue = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 96), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Head
            _spriteHead = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 32), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Body
            _spriteBody = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 0), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation                
            _spriteBodyRotate = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 16), // Top left coords
                new Size(16, 16), // Frame size
                true);  // Horizontal animation
            // Tail
            _spriteTail = new Sprite(spriteMapUrl,
                new DirectionPoint(0, 48), // Top left coords
                new Size(16, 16), // Frame size
                true); // Horizontal animation
        }
        public Image GetTongueImage()
        {
            const double half = 0.5;
            var rotate = new RotateTransform(0);
            var snakeTongue = new Image();
            snakeTongue.Source = this.TongueSprite.GetRenderedImage();
            switch (Tongue.Direction)
            {
                case SnakeDirection.UP:
                    rotate = new RotateTransform(270);
                    break;
                case SnakeDirection.RIGHT:
                    rotate = new RotateTransform(0);
                    break;
                case SnakeDirection.DOWN:
                    rotate = new RotateTransform(90);
                    break;
                case SnakeDirection.LEFT:
                    rotate = new RotateTransform(180);
                    break;
                default:
                    break;
            }
            snakeTongue.RenderTransform = rotate;
            snakeTongue.RenderTransformOrigin = new System.Windows.Point(half, half);

            return snakeTongue;
        }
        public Image GetHeadImage()
        {
            const double half = 0.5;
            var rotate = new RotateTransform(0);
            var snakeHead = new Image();
            snakeHead.Source = this.HeadSprite.GetRenderedImage();
            switch (Head.Direction)
            {
                case SnakeDirection.UP:
                    rotate = new RotateTransform(270);
                    break;
                case SnakeDirection.RIGHT:
                    rotate = new RotateTransform(0);
                    break;
                case SnakeDirection.DOWN:
                    rotate = new RotateTransform(90);
                    break;
                case SnakeDirection.LEFT:
                    rotate = new RotateTransform(180);
                    break;
                default:
                    break;
            }
            snakeHead.RenderTransform = rotate;
            snakeHead.RenderTransformOrigin = new System.Windows.Point(half, half);

            return snakeHead;
        }

        public List<Image> GetBodyImages()
        {
            const double half = 0.5;
            var rotate = new RotateTransform(0);
            var snakeBody = new List<Image>();

            for (int i = 0; i < BodyLength; i++)
            {
                snakeBody.Add(new Image());
                switch (BodyPoints[i].Direction)
                {
                    case SnakeDirection.UP:
                        snakeBody[i].Source = BodySprite.GetRenderedImage();
                        rotate = new RotateTransform(270);
                        break;
                    case SnakeDirection.RIGHT:
                        snakeBody[i].Source = BodySprite.GetRenderedImage();
                        rotate = new RotateTransform(0);
                        break;
                    case SnakeDirection.DOWN:
                        snakeBody[i].Source = BodySprite.GetRenderedImage();
                        rotate = new RotateTransform(90);
                        break;
                    case SnakeDirection.LEFT:
                        snakeBody[i].Source = BodySprite.GetRenderedImage();
                        rotate = new RotateTransform(180);
                        break;
                    case SnakeDirection.RIGHT_TO_DOWN:
                    case SnakeDirection.UP_TO_LEFT:
                        snakeBody[i].Source = BodyRotateSprite.GetRenderedImage();
                        rotate = new RotateTransform(0);
                        break;
                    case SnakeDirection.UP_TO_RIGHT:
                    case SnakeDirection.LEFT_TO_DOWN:
                        snakeBody[i].Source = BodyRotateSprite.GetRenderedImage();
                        rotate = new RotateTransform(270);
                        break;
                    case SnakeDirection.DOWN_TO_LEFT:
                    case SnakeDirection.RIGHT_TO_UP:
                        snakeBody[i].Source = BodyRotateSprite.GetRenderedImage();
                        rotate = new RotateTransform(90);
                        break;
                    case SnakeDirection.DOWN_TO_RIGHT:
                    case SnakeDirection.LEFT_TO_UP:
                        snakeBody[i].Source = BodyRotateSprite.GetRenderedImage();
                        rotate = new RotateTransform(180);
                        break;
                    default:
                        break;
                }
                snakeBody[i].RenderTransform = rotate;
                snakeBody[i].RenderTransformOrigin = new System.Windows.Point(half, half);
            }
            return snakeBody;
        }

        public Image GetTailImage()
        {
            const double half = 0.5;
            var rotate = new RotateTransform(0);
            var snakeTail = new Image();
            snakeTail.Source = TailSprite.GetRenderedImage();
            switch (Tail.Direction)
            {
                case SnakeDirection.UP:
                    rotate = new RotateTransform(270);
                    break;
                case SnakeDirection.RIGHT:
                    rotate = new RotateTransform(0);
                    break;
                case SnakeDirection.DOWN:
                    rotate = new RotateTransform(90);
                    break;
                case SnakeDirection.LEFT:
                    rotate = new RotateTransform(180);
                    break;
                default:
                    break;
            }
            snakeTail.RenderTransform = rotate;
            snakeTail.RenderTransformOrigin = new System.Windows.Point(half, half);

            return snakeTail;
        }
    }
}
