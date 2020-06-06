
using System;
using System.Globalization;
using Hackaton.CrmDbModel.Model.Ai;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Model
{
    public partial class AotModel
    {
        [JsonProperty("variants")]
        public Variant[] Variants { get; set; }

        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }

    public partial class Variant
    {
        [JsonProperty("groups")]
        public Group[] Groups { get; set; }

        [JsonProperty("units")]
        public Unit[] Units { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("descr")]
        public string Descr { get; set; }

        [JsonProperty("isGroup")]
        public bool IsGroup { get; set; }

        [JsonProperty("isSubj")]
        public bool IsSubj { get; set; }

        [JsonProperty("last")]
        public long Last { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        public AiGroup AiGroup { get; set; }
    }

    public partial class Unit
    {
        [JsonProperty("grm")]
        public string[] Grm { get; set; }

        [JsonProperty("homNo")]
        public long HomNo { get; set; }
    }

    public partial class Word
    {
        [JsonProperty("homonyms")]
        public string[] Homonyms { get; set; }

        [JsonProperty("str")]
        public string Str { get; set; }

        public AiWord AiWord { get; set; }
    }

    //public enum Descr { Empty, ГенитИг, Оборот, ОднорИг, Пг, ПрилСущ };

    public partial class AotModel
    {
        public static AotModel[][] FromJson(string json) => JsonConvert.DeserializeObject<AotModel[][]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AotModel[][] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    //internal class DescrConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Descr) || t == typeof(Descr?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        switch (value)
    //        {
    //            case " ":
    //                return Descr.Empty;
    //            case "генит_иг":
    //                return Descr.ГенитИг;
    //            case "оборот":
    //                return Descr.Оборот;
    //            case "однор_иг":
    //                return Descr.ОднорИг;
    //            case "пг":
    //                return Descr.Пг;
    //            case "прил_сущ":
    //                return Descr.ПрилСущ;
    //        }
    //        throw new Exception("Cannot unmarshal type Descr");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Descr)untypedValue;
    //        switch (value)
    //        {
    //            case Descr.Empty:
    //                serializer.Serialize(writer, " ");
    //                return;
    //            case Descr.ГенитИг:
    //                serializer.Serialize(writer, "генит_иг");
    //                return;
    //            case Descr.Оборот:
    //                serializer.Serialize(writer, "оборот");
    //                return;
    //            case Descr.ОднорИг:
    //                serializer.Serialize(writer, "однор_иг");
    //                return;
    //            case Descr.Пг:
    //                serializer.Serialize(writer, "пг");
    //                return;
    //            case Descr.ПрилСущ:
    //                serializer.Serialize(writer, "прил_сущ");
    //                return;
    //        }
    //        throw new Exception("Cannot marshal type Descr");
    //    }

    //    public static readonly DescrConverter Singleton = new DescrConverter();
    //}
}
