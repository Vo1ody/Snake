using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    public enum SnakeDirection : short
    {
        UP, RIGHT, DOWN, LEFT,
        LEFT_TO_UP, LEFT_TO_DOWN,
        RIGHT_TO_UP, RIGHT_TO_DOWN,
        UP_TO_LEFT, UP_TO_RIGHT,
        DOWN_TO_LEFT, DOWN_TO_RIGHT
    };
    class Snake
    {
        private DirectionPoint _head, _tail, _tongue;
        private List<DirectionPoint> _body;

        public Snake()
        {
            _tongue = new DirectionPoint(1, 0);
            _head = new DirectionPoint(0, 0);
            _tail = new DirectionPoint(-1, 0);
            _body = new List<DirectionPoint>();
        }

        public Snake(DirectionPoint tongue, DirectionPoint head, DirectionPoint tail, List<DirectionPoint> body)
        {
            _tongue = tongue;
            _head = head;
            _tail = tail;
            _body = body;
        }

        public Snake(DirectionPoint tongue, DirectionPoint head, DirectionPoint tail)
        {
            _tongue = tongue;
            _head = head;
            _tail = tail;
            _body = new List<DirectionPoint>();
        }

        public DirectionPoint Tongue { get { return _tongue; } set { _tongue = value; } }
        public DirectionPoint Head { get { return _head; } set { _head = value; } }
        public DirectionPoint Tail { get { return _tail; } set { _tail = value; } }
        public SnakeDirection Direction { get { return _head.Direction; } set { _head.Direction = value; } }
        public List<DirectionPoint> BodyPoints { get { return _body; } set { _body = value; } }
        public int BodyLength { get { return _body.Count; } }

        public void AddBodyPointToEnd()
        {
            // Copy last point
            var newPoint = new DirectionPoint();
            newPoint.X = _body.Last<DirectionPoint>().X;
            newPoint.Y = _body.Last<DirectionPoint>().Y;
            newPoint.Direction = _body.Last<DirectionPoint>().Direction;
            // And add to the end
            _body.Add(newPoint);
        }

        public void Move(int distance, Size field, Size headSize)
        {
            var tempPoint = new DirectionPoint();
            tempPoint.X = _head.X;
            tempPoint.Y = _head.Y;
            tempPoint.Direction = _head.Direction;
            if (_body[0].Direction != _head.Direction)
            {
                switch (_body[0].Direction)
                {
                    case SnakeDirection.UP_TO_RIGHT:
                    case SnakeDirection.DOWN_TO_RIGHT:
                    case SnakeDirection.RIGHT:
                        if (tempPoint.Direction == SnakeDirection.UP)
                        {
                            tempPoint.Direction = SnakeDirection.RIGHT_TO_UP;
                        }
                        else if (tempPoint.Direction == SnakeDirection.RIGHT)
                        {
                            tempPoint.Direction = SnakeDirection.RIGHT;
                        }
                        else
                        {
                            tempPoint.Direction = SnakeDirection.RIGHT_TO_DOWN;
                        }
                        break;
                    case SnakeDirection.LEFT_TO_DOWN:
                    case SnakeDirection.RIGHT_TO_DOWN:
                    case SnakeDirection.DOWN:
                        if (tempPoint.Direction == SnakeDirection.LEFT)
                        {
                            tempPoint.Direction = SnakeDirection.DOWN_TO_LEFT;
                        }
                        else if (tempPoint.Direction == SnakeDirection.DOWN)
                        {
                            tempPoint.Direction = SnakeDirection.DOWN;
                        }
                        else
                        {
                            tempPoint.Direction = SnakeDirection.DOWN_TO_RIGHT;
                        }
                        break;
                    case SnakeDirection.UP_TO_LEFT:
                    case SnakeDirection.DOWN_TO_LEFT:
                    case SnakeDirection.LEFT:
                        if (tempPoint.Direction == SnakeDirection.UP)
                        {
                            tempPoint.Direction = SnakeDirection.LEFT_TO_UP;
                        }
                        else if (tempPoint.Direction == SnakeDirection.LEFT)
                        {
                            tempPoint.Direction = SnakeDirection.LEFT;
                        }
                        else
                        {
                            tempPoint.Direction = SnakeDirection.LEFT_TO_DOWN;
                        }
                        break;
                    case SnakeDirection.LEFT_TO_UP:
                    case SnakeDirection.RIGHT_TO_UP:
                    case SnakeDirection.UP:
                        if (tempPoint.Direction == SnakeDirection.LEFT)
                        {
                            tempPoint.Direction = SnakeDirection.UP_TO_LEFT;
                        }
                        else if (tempPoint.Direction == SnakeDirection.UP)
                        {
                            tempPoint.Direction = SnakeDirection.UP;
                        }
                        else
                        {
                            tempPoint.Direction = SnakeDirection.UP_TO_RIGHT;
                        }
                        break;
                    default:
                        break;
                }
            }
            _tail = _body.Last<DirectionPoint>(); // Tail = last body point
            switch (_tail.Direction)
            {
                case SnakeDirection.UP_TO_LEFT:
                case SnakeDirection.DOWN_TO_LEFT:
                    _tail.Direction = SnakeDirection.LEFT;                  // Pre-last point direction
                    break;
                case SnakeDirection.UP_TO_RIGHT:
                case SnakeDirection.DOWN_TO_RIGHT:
                    _tail.Direction = SnakeDirection.RIGHT;                  // Pre-last point direction
                    break;
                case SnakeDirection.RIGHT_TO_UP:
                case SnakeDirection.LEFT_TO_UP:
                    _tail.Direction = SnakeDirection.UP;
                    break;
                case SnakeDirection.LEFT_TO_DOWN:
                case SnakeDirection.RIGHT_TO_DOWN:
                    _tail.Direction = SnakeDirection.DOWN;
                    break;
                default:
                    /*              
                    if (_body.Count < 2)
                    {
                        _tail.Direction = _head.Direction;                  // Pre-last point direction
                    }
                    else
                    {
                        _tail.Direction = _body[_body.Count - 2].Direction; // Pre-last point direction
                    }      
                    */
                    break;
            }
            _body.RemoveAt(_body.Count - 1);            // Remove last
            _body.Insert(0, tempPoint);                 // Old head position = first body point
            // Make new head
            switch (_head.Direction)
            {
                case SnakeDirection.UP:
                    _head.Y -= distance;
                    if (_head.Y < 0)
                    {
                        _head.Y = field.Height;
                    }
                    break;
                case SnakeDirection.RIGHT:
                    _head.X += distance;
                    if (_head.X > field.Width)
                    {
                        _head.X = 0;
                    }
                    break;
                case SnakeDirection.DOWN:
                    _head.Y += distance;
                    if (_head.Y > field.Height)
                    {
                        _head.Y = 0;
                    }
                    break;
                case SnakeDirection.LEFT:
                    _head.X -= distance;
                    if (_head.X < 0)
                    {
                        _head.X = field.Width;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
