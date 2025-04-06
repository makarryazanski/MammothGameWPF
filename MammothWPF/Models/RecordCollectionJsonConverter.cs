using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MammothWPF.Models;

namespace MammothWPF
{
	public class RecordCollectionJsonConverter : JsonConverter<RecordCollection>
	{
		public override RecordCollection Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			// Десериализуем в List<Record>, затем преобразуем в RecordCollection
			var records = JsonSerializer.Deserialize<List<Record>>(ref reader, options);
			return new RecordCollection(records?.Select(r => (IRecord)r).ToList() ?? new List<IRecord>());
		}

		public override void Write(
			Utf8JsonWriter writer,
			RecordCollection value,
			JsonSerializerOptions options)
		{
			// Сериализуем только Record объекты, отфильтровывая возможные другие реализации IRecord
			var recordsToSerialize = value.OfType<Record>().ToList();
			JsonSerializer.Serialize(writer, recordsToSerialize, options);
		}
	}
}