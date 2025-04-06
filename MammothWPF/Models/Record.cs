using System;
using System.Text.Json.Serialization;

namespace MammothWPF.Models
{
	public class Record : IRecord
	{
		[JsonPropertyName("userName")]
		public string UserName { get; }

		[JsonPropertyName("point")]
		public int Point { get; }

		[JsonConstructor]
		public Record(string userName, int point)
		{
			UserName = userName;
			Point = point;
		}

		public Record(User user) : this(user.Name, user.Point) { }

		public int CompareTo(IRecord other) => Point.CompareTo(other.Point);

		public bool Equals(IRecord other) => UserName.Equals(other?.UserName);
	}
}