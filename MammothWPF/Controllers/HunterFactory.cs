using System.Windows.Controls;
using System.Windows.Media;

namespace MammothWPF.Models
{
	/// <summary>
	/// Фабрика для создания охотников
	/// </summary>
	public static class HunterFactory
	{
		public static Hunter CreateHunter(Canvas canvas)
		{
			return Hunter.Create(canvas, 50, 50, Brushes.Khaki, Brushes.LightYellow, Brushes.SandyBrown);
		}
	}

		//public static Mammoth CreateMammoth(Canvas canvas)
		//{
		//	return Mammoth.Create(canvas, 200, 200, Brushes.Gray, Brushes.White);

		//}
	}