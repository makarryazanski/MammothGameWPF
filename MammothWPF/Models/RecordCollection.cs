using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MammothWPF.Models
{
	public class RecordCollection : List<IRecord>, IRecordCollection
	{
		public RecordCollection() { }

		public RecordCollection(IEnumerable<IRecord> records) : base(records) { }

		public void Add(string userName, int point)
		{
			Add(new Record(userName, point));
		}

		public new void Sort()
		{
			base.Sort(Compare);
		}

		public int Compare(IRecord x, IRecord y)
		{
			return y.Point.CompareTo(x.Point);
		}
	}
}