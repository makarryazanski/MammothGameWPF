using System.Collections.Generic;

namespace MammothWPF.Models
{
	/// <summary>
	/// Интерфейс, представляющий коллекцию рекордов игры
	/// </summary>
	public interface IRecordCollection : IEnumerable<IRecord>, IComparer<IRecord>
	{
		/// <summary>
		/// Добавление нового рекора в коллекцию
		/// </summary>
		/// <param name="parUserName">Имя пользователя, связанного с рекордом</param>
		/// <param name="parPoint">Количество очков, набранных пользователем</param>
		void Add(string parUserName, int parPoint);

		/// <summary>
		/// Сортировка коллекции рекордов
		/// </summary>
		void Sort();
	}
}