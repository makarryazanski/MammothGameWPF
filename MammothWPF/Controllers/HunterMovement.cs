using MammothWPF.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MammothWPF.Controllers
{
    /// <summary>
    /// Класс движения охотника
    /// </summary>
    public class HunterMovement
    {
        private readonly object _lock = new object();
        private readonly Canvas _canvas;

        public HunterMovement(Canvas canvas, double initialX, double initialY, Brush headColor, Brush bodyColor, Brush spearColor)
        {
            _canvas = canvas;
            _headColor = headColor;
            _bodyColor = bodyColor;
            _spearColor = spearColor;

            Head = CreateRectangle(initialX, initialY, headColor);
            Body = new List<Rectangle>
            {
                CreateRectangle(initialX, initialY + 10, bodyColor),
                CreateRectangle(initialX, initialY + 20, bodyColor)
            };
            Spear = CreateRectangle(initialX + 10, initialY + 10, spearColor);

            // Добавляем части охотника на Canvas
            _canvas.Children.Add(Head);
            foreach (var rect in Body)
            {
                _canvas.Children.Add(rect);
            }
            _canvas.Children.Add(Spear);
        }

        private Rectangle CreateRectangle(double x, double y, Brush color)
        {
            var rect = new Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = color
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            return rect;
        }

        public void Move(Point target)
        {
            lock (_lock)
            {
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

                // Проверка границ Canvas
                double newHeadX = headX + deltaX;
                double newHeadY = headY + deltaY;

                if (newHeadX < 0 || newHeadX > _canvas.ActualWidth - Head.Width)
                    deltaX = 0;
                if (newHeadY < 0 || newHeadY > _canvas.ActualHeight - Head.Height)
                    deltaY = 0;

                // Перемещаем голову и тело охотника
                Canvas.SetLeft(Head, headX + deltaX);
                Canvas.SetTop(Head, headY + deltaY);

                foreach (var rect in Body)
                {
                    Canvas.SetLeft(rect, Canvas.GetLeft(rect) + deltaX);
                    Canvas.SetTop(rect, Canvas.GetTop(rect) + deltaY);
                }

                // Перемещаем копье вместе с охотником
                Canvas.SetLeft(Spear, Canvas.GetLeft(Spear) + deltaX);
                Canvas.SetTop(Spear, Canvas.GetTop(Spear) + deltaY);
            }
        }

        public Direction ReadMovement(Direction currentDirection)
        {
            if (Keyboard.IsKeyDown(Key.Up))
            {
                return Direction.Up;
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                return Direction.Down;
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                return Direction.Left;
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                return Direction.Right;
            }

            return currentDirection;
        }

        public Point GetPoint(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Point(0, -10),
                Direction.Down => new Point(0, 10),
                Direction.Left => new Point(-10, 0),
                Direction.Right => new Point(10, 0),
                _ => new Point(0, 0),
            };
        }

        public Rectangle Head { get; private set; }
        public List<Rectangle> Body { get; }
        public Rectangle Spear { get; private set; }
        private readonly Brush _headColor;
        private readonly Brush _bodyColor;
        private readonly Brush _spearColor;
    }
}