using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    class DirectionPoint
    {
        private double _x, _y;
        private SnakeDirection _dir;

        public DirectionPoint()
        {
            _x = _y = 0;
        }

        public DirectionPoint(double x, double y)
        {
            _x = x;
            _y = y;
            _dir = SnakeDirection.RIGHT;
        }

        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public SnakeDirection Direction { get { return _dir; } set { _dir = value; } }
    }
}
