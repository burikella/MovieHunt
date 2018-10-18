using Newtonsoft.Json.Converters;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class IsoDateTimeByFormatConverter : IsoDateTimeConverter
    {
        public IsoDateTimeByFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}