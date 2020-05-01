// Copyright (c) 2018 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Tesla.NET.Models.Internal
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The vehicle config of a <see cref="IVehicle"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class VehicleConfig : IVehicleConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleConfig"/> class.
        /// </summary>
        /// <param name="carType">The <see cref="CarType"/>The car type.</param>
        /// <param name="timestamp">The <see cref="Timestamp"/>.</param>
        public VehicleConfig(
            string carType,
            long timestamp)
        {
            CarType = carType;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets the car type of the vehicle.
        /// </summary>
        public string CarType { get; }

        /// <summary>
        /// Gets the millisecond Epoch timestamp when the <see cref="IVehicleConfig"/> was captured.
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Gets the UTC <see cref="DateTime"/> when the <see cref="IClimateState"/> was captured.
        /// </summary>
        public DateTime TimestampUtc => EpochConversion.FromMilliseconds(Timestamp);

        private string DebuggerDisplay =>
            $"{GetType().Name}";
    }
}
