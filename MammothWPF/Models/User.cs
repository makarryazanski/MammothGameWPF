using System;

namespace MammothWPF.Models
{
	/// <summary>
	/// Пользователь в игре
	/// Реализует интерфейс IUser для работы с данными пользователя, такими как имя и набранные очки
	/// Реализует паттерн Singleton для обеспечения единственного экземпляра класса
	/// </summary>
	public sealed class User : IUser
	{
		/// <summary>
		/// Количество очков, набранных пользователем
		/// </summary>
		private int _point;

		/// <summary>
		/// Хранитель экземпляра класса для реализации паттерна Singleton
		/// </summary>
		private static readonly Lazy<User> _instanceHolder =
			new Lazy<User>(() => new User());

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Свойство для управления количеством очков пользователя
		/// </summary>
		public int Point
		{
			get
			{
				return _point;
			}
			set
			{
				// Убеждаемся, что количество очков не становится отрицательным
				if (value >= 0)
				{
					_point = value;
				}

				// // Если экземпляр класса уже создан, вызываем перерисовку очков
				// if (_instanceHolder.IsValueCreated)
				// {
				// 	RedrawerGameUI.Instance.RedrawPoint();
				// }
			}
		}

		/// <summary>
		/// Единственный экземпляр класса User (реализация паттерна Singleton)
		/// </summary>
		public static User Instance
		{
			get
			{
				return _instanceHolder.Value;
			}
		}

		/// <summary>
		/// Приватный конструктор для предотвращения создания экземпляров класса извне
		/// </summary>
		private User()
		{
			Name = string.Empty;
			_point = 0;
		}
	}
}