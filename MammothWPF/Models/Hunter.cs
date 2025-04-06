using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MammothWPF.Models
{
    /// <summary>
    /// Охотник
    /// </summary>
    public class Hunter
    {
        private readonly object _lock = new object();
        internal readonly Canvas _canvas;

        public Hunter(Canvas canvas, double initialX, double initialY, Brush headColor, Brush bodyColor, Brush spearColor)
        {
            _canvas = canvas;
            _headColor = headColor;
            _bodyColor = bodyColor;

            Head = CreateRectangle(initialX, initialY, headColor);
            Body = new List<Rectangle>
            {
                CreateRectangle(initialX, initialY + 20, bodyColor),
                CreateRectangle(initialX, initialY + 40, bodyColor)
            };
            Spear = new Spear(canvas, initialX + 20, initialY + 20, spearColor);

            Draw();
        }

        private Rectangle CreateRectangle(double x, double y, Brush color)
        {
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = color
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            return rect;
        }

        /// <summary>
        /// Метод перемещения охотника
        /// </summary>
        /// <param name="target"></param>
        public void Move(Point target)
        {
            lock (_lock)
            {
                Clear();

                double deltaX = 0;
                double deltaY = 0;

                double headX = Canvas.GetLeft(Head);
                double headY = Canvas.GetTop(Head);

                if (headX < target.X)
                    deltaX = 10;
                else if (headX > target.X)
                    deltaX = -10;

                if (headY < target.Y)
                    deltaY = 10;
                else if (headY > target.Y)
                    deltaY = -10;

                // Перемещаем голову и тело охотника
                Canvas.SetLeft(Head, headX + deltaX);
                Canvas.SetTop(Head, headY + deltaY);

                foreach (var rect in Body)
                {
                    Canvas.SetLeft(rect, Canvas.GetLeft(rect) + deltaX);
                    Canvas.SetTop(rect, Canvas.GetTop(rect) + deltaY);
                }

                // Перемещаем копье вместе с охотником
                Spear.Move(deltaX, deltaY);

                Draw();
            }
        }

        /// <summary>
        /// Метод для отрисовки охотника
        /// </summary>
        public void Draw()
        {
            if (!_canvas.Children.Contains(Head))
            {
                _canvas.Children.Add(Head);
            }
            foreach (var rect in Body)
            {
                if (!_canvas.Children.Contains(rect))
                {
                    _canvas.Children.Add(rect);
                }
            }
            if (!_canvas.Children.Contains(Spear.Shape))
            {
                _canvas.Children.Add(Spear.Shape);
            }
        }

        /// <summary>
        /// Метод для очистки охотника
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _canvas.Children.Remove(Head);
                foreach (var rect in Body)
                {
                    _canvas.Children.Remove(rect);
                }
                Spear.Clear();
            }
        }

		/// <summary>
		/// Фабричный метод для создания охотника
		/// </summary>
		public static Hunter Create(Canvas canvas, double initialX, double initialY, Brush headColor, Brush bodyColor, Brush spearColor)
		{
			return new Hunter(canvas, initialX, initialY, headColor, bodyColor, spearColor);
		}

		/// <summary>
		/// Голова охотника
		/// </summary>
		public Rectangle Head { get; private set; }
        /// <summary>
        /// Тело охотника
        /// </summary>
        public List<Rectangle> Body { get; }
        /// <summary>
        /// Копье охотника
        /// </summary>
        public Spear Spear { get; private set; }
        /// <summary>
        /// Цвета охотника
        /// </summary>
        private readonly Brush _headColor;
        private readonly Brush _bodyColor;
    }
}