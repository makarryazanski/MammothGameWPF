using MammothWPF.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MammothWPF.Controllers
{
	/// <summary>
	/// Класс бросания копья
	/// </summary>
	public class ThrowingTheSpear
	{
		private readonly Hunter _hunter;
		private readonly Mammoth _mammoth;
		private Direction _direction;
		private bool _isThrown;
		private Action _onHit;

		public ThrowingTheSpear(Hunter hunter, Mammoth mammoth, Action onHit)
		{
			_hunter = hunter;
			_mammoth = mammoth;
			_isThrown = false;
			_onHit = onHit;
		}

		public void Throw(Direction direction)
		{
			_isThrown = true;
			_direction = direction;

			// Создаем новое копье
			var spear = new Spear(_hunter._canvas, Canvas.GetLeft(_hunter.Head) + 20, Canvas.GetTop(_hunter.Head) + 20, Brushes.SandyBrown);

			Task.Run(() => MoveSpear(spear));
		}

		// Метод движения копья
		private async Task MoveSpear(Spear spear)
		{
			Debug.WriteLine("Начало движения копья");

			try
			{
				while (_isThrown)
				{
					double deltaX = 0, deltaY = 0;

					switch (_direction)
					{
						case Direction.Up: deltaY = -20; break;
						case Direction.Down: deltaY = 20; break;
						case Direction.Left: deltaX = -20; break;
						case Direction.Right: deltaX = 20; break;
					}

					await Application.Current.Dispatcher.InvokeAsync(() =>
					{
						double newLeft = Canvas.GetLeft(spear.Shape) + deltaX;
						double newTop = Canvas.GetTop(spear.Shape) + deltaY;

						// Проверка границ
						if (newLeft < 0 || newLeft >= _hunter._canvas.ActualWidth ||
							newTop < 0 || newTop >= _hunter._canvas.ActualHeight)
						{
							_isThrown = false;
							_hunter._canvas.Children.Remove(spear.Shape);
							Debug.WriteLine("Копье вышло за границы карты");
							return;
						}

						// Перемещаем копье
						Canvas.SetLeft(spear.Shape, newLeft);
						Canvas.SetTop(spear.Shape, newTop);

						// Проверка попадания
						if (IsHit(spear))
						{
							_isThrown = false;
							_hunter._canvas.Children.Remove(spear.Shape);
							_onHit?.Invoke();
							Debug.WriteLine("Попадание в мамонта");
							return;
						}
					});

					await Task.Delay(100);
				}
			}
			finally
			{
				// Гарантированное удаление копья при выходе
				Application.Current.Dispatcher.Invoke(() =>
				{
					if (_hunter._canvas.Children.Contains(spear.Shape))
					{
						_hunter._canvas.Children.Remove(spear.Shape);
					}
				});
			}
		}

		// булево есть попадание или нет
		public bool IsHit(Spear spear)
		{
			const double hitThreshold = 15; // Допустимое расстояние для попадания
			double spearX = Canvas.GetLeft(spear.Shape);
			double spearY = Canvas.GetTop(spear.Shape);

			Debug.WriteLine($"Проверка попадания: копье ({spearX}, {spearY})");

			// Проверка попадания в голову
			if (Math.Abs(Canvas.GetLeft(_mammoth.Head) - spearX) < hitThreshold &&
				Math.Abs(Canvas.GetTop(_mammoth.Head) - spearY) < hitThreshold)
			{
				Debug.WriteLine("Попадание в голову мамонта");
				return true;
			}

			// Проверка попадания в тело
			foreach (var rect in _mammoth.Body)
			{
				if (Math.Abs(Canvas.GetLeft(rect) - spearX) < hitThreshold &&
					Math.Abs(Canvas.GetTop(rect) - spearY) < hitThreshold)
				{
					Debug.WriteLine("Попадание в тело мамонта");
					return true;
				}
			}

			// Проверка попадания в бивни
			foreach (var rect in _mammoth.Tusk)
			{
				if (Math.Abs(Canvas.GetLeft(rect) - spearX) < hitThreshold &&
					Math.Abs(Canvas.GetTop(rect) - spearY) < hitThreshold)
				{
					Debug.WriteLine("Попадание в бивень мамонта");
					return true;
				}
			}

			return false;
		}
	}
}