﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using FlightsEngine.Models;

namespace FlightsServices
{
    public interface ITripsService
    {


        bool InsertTrips(int SearchTripProviderId);

    }
}
