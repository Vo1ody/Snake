using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Exam
{
    public partial class MainWindow : Window
    {
        string spriteMapPath = System.IO.Directory.GetCurrentDirectory().ToString() + "\\snake-graphics.gif";
        // Time
        System.Windows.Threading.DispatcherTimer gameTimer;
        TimeSpan time, timerInterval = new TimeSpan(1000000);    // 300 000 = 00:00:00.3
        bool paused = false;
        bool _endOfGame = false;
        int _score = 0;
        int _appleScoreCost = 10;
        Point GameArea;

        // Snake
        Rendering snake;
        Image snakeHead, snakeTail, snakeTongue;
        List<Image> snakeBody;
        Apple apple;
        Image appleImage;
        Rectangle fieldBorder;

        // Snake moving
        int distance = 16;   // images height&width

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // Gaming initializations
            time = new TimeSpan(0, 0, 0);

            // temp
            // Menu_Play_Click(new object(), new RoutedEventArgs());
        }

        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Menu_Play_Click(object sender, RoutedEventArgs e)
        {
            GameGrid.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Collapsed;
            if (GameField.ActualWidth > 0 && GameField.ActualHeight > 0)
            {
                GameArea = new Point(GameField.ActualWidth, GameField.ActualHeight);
            }
            else if (GameField.Width > 0 && GameField.Height > 0)
            {
                GameArea = new Point(GameField.Width, GameField.Height);
            }
            else
            {
                GameArea = new Point(400, 400); // 18 - menu place
            }

            //GameArea = new Point(200, 200);
            fieldBorder = new Rectangle();
            fieldBorder.Stroke = Brushes.Black;
            fieldBorder.HorizontalAlignment = HorizontalAlignment.Left;
            fieldBorder.VerticalAlignment = VerticalAlignment.Top;
            fieldBorder.Width = GameArea.X;
            fieldBorder.Height = GameArea.Y;

            var tempBody = new List<DirectionPoint>();
            tempBody.Add(new DirectionPoint(GameArea.X / 2 - distance, GameArea.Y / 2));
            //for (int i = 2; i < 15; i++)
            //{
            //    tempBody.Add(new Code.DirectionPoint(GameField.ActualWidth / 2 - i * distance, GameField.ActualHeight / 2));
            //}
            snake = new Rendering(
                new DirectionPoint(GameArea.X / 2 + distance, GameArea.Y / 2),        // Tongue
                new DirectionPoint(GameArea.X / 2, GameArea.Y / 2),                   // Head
                new DirectionPoint(GameArea.X / 2 - 2 * distance, GameArea.Y / 2),    // Tail
                tempBody,                                                                                   // Body
                new Uri(spriteMapPath, UriKind.Relative));                                                  // Sprite map     

            paused = false;
            _endOfGame = false;
            _score = 0;
            ScoreValue.Header = "0";
            apple = new Apple(
                (GameArea.X / 2) + 2 * distance,            // X
                GameArea.Y / 2,                             // Y
                new Uri(spriteMapPath, UriKind.Relative));  // Sprite map

            Game.MakeNewApple(snake, GameArea.X, GameArea.Y, ref apple);

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += new EventHandler(FrameTick);
            gameTimer.Interval = timerInterval;
            gameTimer.Start();
        }

        private void FrameTick(object sender, EventArgs e)
        {
            time = time.Add(timerInterval);
            TimeValue.Header =
                (time.Hours < 10 ? "0" + time.Hours.ToString() : time.Hours.ToString())
                + ":" + (time.Minutes < 10 ? "0" + time.Minutes.ToString() : time.Minutes.ToString())
                + ":" + (time.Seconds < 10 ? "0" + time.Seconds.ToString() : time.Seconds.ToString())
                + "." + (time.Milliseconds / 100).ToString();

            snake.Move(distance, new Size(GameArea.X, GameArea.Y), snake.HeadSprite.FrameSize);

            Game.CheckSnakeCollisions(snake, out _endOfGame);

            if (apple.GetType() != typeof(SpoiledApple) && Game.IsSnakeAteApple(snake, apple, ref _score, _appleScoreCost))
            {
                // Set new apple
                Game.MakeNewApple(snake, GameArea.X, GameArea.Y, ref apple);
                ScoreValue.Header = _score.ToString();
                // Add snake segment
                snake.AddBodyPointToEnd();
            }
            else if (apple.GetType() == typeof(SpoiledApple) && Game.IsSnakeAteApple(snake, apple, ref _score, _appleScoreCost))
            {
                Game.MakeNewApple(snake, GameArea.X, GameArea.Y, ref apple);
                ScoreValue.Header = _score.ToString();
                // Add snake segment
                snake.AddBodyPointToEnd();
                snake.AddBodyPointToEnd();
                gameTimer.Interval -= new TimeSpan(100000);
            }

            try
            {
                RedrawObjects();
            }
            catch (Exception ex)
            {
                gameTimer.Stop();
                time = new TimeSpan(0, 0, 0);
                MenuStatus.Content = App.Current.Resources["StatusError"].ToString() + "!"; // "Something went wrong, we got error!"
                MessageBox.Show(ex.Message, App.Current.Resources["Error"].ToString() + "!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                MenuGrid.Visibility = Visibility.Visible;
                GameGrid.Visibility = Visibility.Hidden;
            }

            if (_endOfGame)
            {
                gameTimer.Stop();
                time = new TimeSpan(0, 0, 0);
                MenuStatus.Content = App.Current.Resources["End_of_game"].ToString() + "! \n\n"
                                     + App.Current.Resources["Your_score"].ToString() + ":" + _score.ToString();
                MenuGrid.Visibility = Visibility.Visible;
                GameGrid.Visibility = Visibility.Hidden;
            }
        }


        private void RedrawObjects()
        {
            try
            {
                // Clear and redraw objects
                GameField.Children.Clear();

                GameField.Children.Add(fieldBorder);

                RotateTransform rotate = new RotateTransform(0);
                // Add head
                snakeHead = snake.GetHeadImage();
                Canvas.SetLeft(snakeHead, snake.Head.X);
                Canvas.SetTop(snakeHead, snake.Head.Y);
                GameField.Children.Add(snakeHead);
                // Add body
                snakeBody = snake.GetBodyImages();
                for (int i = 0; i < snake.BodyLength; i++)
                {
                    Canvas.SetLeft(snakeBody[i], snake.BodyPoints[i].X);
                    Canvas.SetTop(snakeBody[i], snake.BodyPoints[i].Y);
                    GameField.Children.Add(snakeBody[i]);
                }
                // Add tail
                snakeTail = snake.GetTailImage();
                Canvas.SetLeft(snakeTail, snake.Tail.X);
                Canvas.SetTop(snakeTail, snake.Tail.Y);
                GameField.Children.Add(snakeTail);
                // Add apple
                appleImage = apple.GetImage();
                Canvas.SetLeft(appleImage, apple.X);
                Canvas.SetTop(appleImage, apple.Y);
                GameField.Children.Add(appleImage);

                GameField.UpdateLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            debug_lastButton.Content = e.Key.ToString();
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    if (snake.BodyPoints[0].Direction != SnakeDirection.DOWN)
                    {
                        snake.Head.Direction = SnakeDirection.UP;
                    }
                    break;
                case Key.S:
                case Key.Down:
                    if (snake.BodyPoints[0].Direction != SnakeDirection.UP)
                    {
                        snake.Head.Direction = SnakeDirection.DOWN;
                    }
                    break;
                case Key.D:
                case Key.Right:
                    if (snake.BodyPoints[0].Direction != SnakeDirection.LEFT)
                    {
                        snake.Head.Direction = SnakeDirection.RIGHT;
                    }
                    break;
                case Key.A:
                case Key.Left:
                    if (snake.BodyPoints[0].Direction != SnakeDirection.RIGHT)
                    {
                        snake.Head.Direction = SnakeDirection.LEFT;
                    }
                    break;
                case Key.Space:
                    snake.Tongue.Direction = snake.Head.Direction;
                    snakeTongue = snake.GetTongueImage();
                    switch (snake.Tongue.Direction)
                    {
                        case SnakeDirection.UP:
                            snake.Tongue.X = snake.Head.X;
                            snake.Tongue.Y = snake.Head.Y - distance;
                            Canvas.SetLeft(snakeTongue, snake.Head.X);
                            Canvas.SetTop(snakeTongue, snake.Head.Y - distance);
                            break;
                        case SnakeDirection.DOWN:
                            snake.Tongue.X = snake.Head.X;
                            snake.Tongue.Y = snake.Head.Y + distance;
                            Canvas.SetLeft(snakeTongue, snake.Head.X);
                            Canvas.SetTop(snakeTongue, snake.Head.Y + distance);
                            break;
                        case SnakeDirection.LEFT:
                            snake.Tongue.X = snake.Head.X - distance;
                            snake.Tongue.Y = snake.Head.Y;
                            Canvas.SetLeft(snakeTongue, snake.Head.X - distance);
                            Canvas.SetTop(snakeTongue, snake.Head.Y);
                            break;
                        case SnakeDirection.RIGHT:
                            snake.Tongue.X = snake.Head.X + distance;
                            snake.Tongue.Y = snake.Head.Y;
                            Canvas.SetLeft(snakeTongue, snake.Head.X + distance);
                            Canvas.SetTop(snakeTongue, snake.Head.Y);
                            break;
                        default: break;
                    }
                    GameField.Children.Add(snakeTongue);
                    if (apple.GetType() == typeof(SpoiledApple) && Game.IsSnakeTouchTongueApple(snake, apple, ref _score, 20))
                    {
                        // Set new apple
                        Game.MakeNewApple(snake, GameArea.X, GameArea.Y, ref apple);
                        ScoreValue.Header = _score.ToString();
                    }
                    break;
                case Key.P:
                case Key.Escape:
                    if (paused)
                    {
                        gameTimer.Start();
                    }
                    else
                    {
                        gameTimer.Stop();
                    }
                    paused = !paused;
                    break;
                default:
                    break;
            };
        }
    }
}
