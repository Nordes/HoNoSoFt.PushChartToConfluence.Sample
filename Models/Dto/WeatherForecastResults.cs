using System.Collections.Generic;

namespace HoNoSoFt.PushChartToConfluence.Sample.Models.Dto
{
    public class WeatherForecastResults
    {
        public int Total { get; set; }
        public IEnumerable<WeatherForecast> Forecasts;
    }
}