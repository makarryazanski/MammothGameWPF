using System;

namespace MammothWPF.Models
{
	/// <summary>
	/// Интерфейс, представляющий запись рекорда в игре
	/// </summary>
	public interface IRecord : IComparable<IRecord>
	{
		/// <summary>
		/// Имя пользователя, связанного с рекордом
		/// </summary>
		string UserName { get; }

		/// <summary>
		/// Количество очков, набранных пользователем
		/// </summary>
		int Point { get; }

		/// <summary>
		/// Сравнение текущего рекорда с другим рекордом
		/// </summary>
		/// <param name="parRecord">Рекорд для сравнения</param>
		/// <returns>True, если рекорды равны, иначе false</returns>
		bool Equals(IRecord parRecord);
	}
}