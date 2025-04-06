namespace MammothWPF.Models
{
	/// <summary>
	/// Интерфейс, представляющий пользователя в игре
	/// </summary>
	public interface IUser
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Количество очков, набранных пользователем
		/// </summary>
		int Point { get; set; }
	}
}