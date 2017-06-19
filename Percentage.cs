using System;

namespace HpkArchiver
{
    /// <summary>
    /// Displays "[===------]" or "[===------] 30%" to show percent complete.
    /// </summary>
    public class Percentage
    {
        private int _X;
        /// <summary>
        /// The X position in the console buffer where to start the cursor display.
        /// </summary>
        public int X
        {
            get { return _X; }
        }

        private int _Y;
        /// <summary>
        /// The Y position in the console buffer where to start the cursor display.
        /// </summary>
        public int Y
        {
            get { return _Y; }
        }

        /// <summary>
        /// Creates a new instance of Percentage.  Use Value() to update the percentage bar.
        /// </summary>
        /// <param name="x">Start position x.</param>
        /// <param name="y">Start position y.</param>
        /// <param name="displaynumber">Whether or not to display the percentage in numeric form after the percentage bar.</param>
        public Percentage(int x, int y, double todo)
        {
            _X = x;
            _Y = y;
            _ToDo = todo;
        }
        private double _ToDo = 0;
        private double _Count = 0;

        /// <summary>
        /// Used to update the percentage display.
        /// </summary>
        /// <param name="todo">How much of what is being represented is left to do (the Denominator of the fraction).</param>
        public void Update()
        {
            _Count++;
            Console.CursorLeft = _X;
            Console.CursorTop = _Y;
            double percent = (_Count / _ToDo);
            int rounded = Convert.ToInt32(Math.Round(percent * 100, 0));
            int display = Convert.ToInt32(Math.Round(percent * 10, 0));
            string bar = "[";
            for (int i = 0; i <= 9; i++)
            {
                if (display <= i)
                    bar += "-";
                else
                    bar += "=";
            }
            Console.Write(bar + "] " + rounded + "%");
        }
    }
}
