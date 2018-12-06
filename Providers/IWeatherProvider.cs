using System.Collections.Generic;
using HoNoSoFt.PushChartToConfluence.Sample.Models;

namespace HoNoSoFt.PushChartToConfluence.Sample.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}