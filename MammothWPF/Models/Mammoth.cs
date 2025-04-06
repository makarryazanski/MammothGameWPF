using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MammothWPF.Models
{
	/// <summary>
	/// Класс мамонта
	/// </summary>
	public class Mammoth
	{
		private readonly object _lock = new object();
		private readonly Canvas _canvas;

		/// <summary>
		/// Конструктор мамонта
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="initialX"></param>
		/// <param name="initialY"></param>
		/// <param name="mammothColor"></param>
		/// <param name="tuskColor"></param>
		public Mammoth(Canvas canvas, double initialX, double initialY, Brush mammothColor, Brush tuskColor)
		{
			_canvas = canvas;
			_mammothColor = mammothColor;
			_tuskColor = tuskColor;

			// Основное тело
			Body = new List<Rectangle>
			{
				CreateRectangle(initialX - 40, initialY, mammothColor),
				CreateRectangle(initialX - 20, initialY, mammothColor),
				CreateRectangle(initialX - 40, initialY + 20, mammothColor),
				CreateRectangle(initialX - 20, initialY + 20, mammothColor),
				CreateRectangle(initialX - 20, initialY + 40, Brushes.DarkGoldenrod),
				CreateRectangle(initialX - 40, initialY + 40, Brushes.DarkGoldenrod),
			};

			// Голова (отдельный прямоугольник сверху)
			Head = CreateRectangle(initialX, initialY, mammothColor);

			// Бивень (наискосок справа от головы)
			Tusk = new List<Rectangle>
			{
				CreateRectangle(initialX, initialY + 20, tuskColor),
				CreateRectangle(initialX + 20, initialY + 40, tuskColor),
			};

			Draw();
		}

		private Rectangle CreateRectangle(double x, double y, Brush color)
		{
			var rect = new Rectangle
			{
				Width = 20, // Увеличиваем ширину в два раза
				Height = 20, // Увеличиваем высоту в два раза
				Fill = color
			};
			Canvas.SetLeft(rect, x);
			Canvas.SetTop(rect, y);
			return rect;
		}

		/// <summary>
		/// Метод отрисовки мамонта
		/// </summary>
		public void Draw()
		{
			lock (_lock)
			{
				foreach (var rect in Body)
				{
					if (!_canvas.Children.Contains(rect))
					{
						_canvas.Children.Add(rect);
					}
				}
				if (!_canvas.Children.Contains(Head))
				{
					_canvas.Children.Add(Head);
				}
				foreach (var rect in Tusk)
				{
					if (!_canvas.Children.Contains(rect))
					{
						_canvas.Children.Add(rect);
					}
				}
			}
		}

		/// <summary>
		/// Метод очистки мамонта
		/// </summary>
		public void Clear()
		{
			lock (_lock)
			{
				foreach (var rect in Body)
				{
					_canvas.Children.Remove(rect);
				}
				_canvas.Children.Remove(Head);
				foreach (var rect in Tusk)
				{
					_canvas.Children.Remove(rect);
				}
			}
		}

		//public static Mammoth Create(Canvas canvas, double initialX, double initialY, Brush headColor, Brush bodyColor, Brush spearColor)
		//{
		//	return new Mammoth(canvas, initialX, initialY, headColor, bodyColor, spearColor);
		//}

		/// <summary>
		/// Метод движения мамонта
		/// </summary>
		/// <param name="currentTarget"></param>
		internal void MoveTowards(Point currentTarget)
		{
			lock (_lock)
			{
				Clear();

				double deltaX = 0;
				double deltaY = 0;

				double headX = Canvas.GetLeft(Head);
				double headY = Canvas.GetTop(Head);

				if (headX < currentTarget.X)
					deltaX = 20; // Увеличиваем шаг в два раза
				else if (headX > currentTarget.X)
					deltaX = -20; // Увеличиваем шаг в два раза

				if (headY < currentTarget.Y)
					deltaY = 20; // Увеличиваем шаг в два раза
				else if (headY > currentTarget.Y)
					deltaY = -20; // Увеличиваем шаг в два раза

				Canvas.SetLeft(Head, headX + deltaX);
				Canvas.SetTop(Head, headY + deltaY);

				foreach (var rect in Body)
				{
					Canvas.SetLeft(rect, Canvas.GetLeft(rect) + deltaX);
					Canvas.SetTop(rect, Canvas.GetTop(rect) + deltaY);
				}

				foreach (var rect in Tusk)
				{
					Canvas.SetLeft(rect, Canvas.GetLeft(rect) + deltaX);
					Canvas.SetTop(rect, Canvas.GetTop(rect) + deltaY);
				}

				Draw();
			}
		}

		/// <summary>
		/// тело мамонта
		/// </summary>
		public List<Rectangle> Body { get; }
		/// <summary>
		/// голова мамонта
		/// </summary>
		public Rectangle Head { get; private set; }
		/// <summary>
		/// бивень мамонта
		/// </summary>
		public List<Rectangle> Tusk { get; private set; }

		/// <summary>
		/// цвет тела
		/// </summary>
		private readonly Brush _mammothColor;
		/// <summary>
		/// цвет бивня
		/// </summary>
		private readonly Brush _tuskColor;
	}
}
