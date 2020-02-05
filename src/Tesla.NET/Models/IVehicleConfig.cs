// Copyright (c) 2018 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Tesla.NET.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// The vehicle config of a <see cref="IVehicle"/>.
    /// </summary>
    public interface IVehicleConfig : IState
    {
        /// <summary>
        /// Gets the car type.
        /// </summary>
        [JsonProperty("car_type")]
        string CarType { get; }
    }
}
