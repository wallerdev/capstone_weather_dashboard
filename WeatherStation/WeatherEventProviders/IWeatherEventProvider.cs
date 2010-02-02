using System;
using System.Collections.Generic;

namespace WeatherStation
{
    interface IWeatherEventProvider
    {
        IEnumerable<WeatherIncident> GetEvents(Address address, DateTime startDate, DateTime endDate);
    }
}
