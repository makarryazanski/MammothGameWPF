using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MammothWPF.Controllers;

namespace MammothWPF.Models
{
	public class Spear
	{
		public Rectangle Shape { get; private set; }
		private Canvas _canvas;

		public Spear(Canvas canvas, double x, double y, Brush color)
		{
			_canvas = canvas;
			Shape = CreateRectangle(x, y, color);
			_canvas.Children.Add(Shape);
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

		public void Move(double deltaX, double deltaY)
		{
			double currentX = Canvas.GetLeft(Shape);
			double currentY = Canvas.GetTop(Shape);

			Canvas.SetLeft(Shape, currentX + deltaX);
			Canvas.SetTop(Shape, currentY + deltaY);

			System.Diagnostics.Debug.WriteLine($"Копье перемещено на ({currentX + deltaX}, {currentY + deltaY})");
		}

		public void Clear()
		{
			_canvas.Children.Remove(Shape);
			System.Diagnostics.Debug.WriteLine("Копье удалено с Canvas");
		}
	}
}