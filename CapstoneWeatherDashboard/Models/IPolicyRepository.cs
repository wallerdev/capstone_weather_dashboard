using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapstoneWeatherDashboard.Models
{
    public interface IPolicyRepository
    {
        IList<Policy> ListAll();
    }
}
