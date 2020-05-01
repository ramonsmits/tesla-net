// Copyright (c) 2018 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Tesla.NET
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Tesla.NET.Models;
    using Tesla.NET.Models.Internal;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class GetVehicleConfigSuccessTestsBase : ClientRequestContext
    {
        private readonly ResponseDataWrapper<VehicleConfig> _expected;
        private readonly long _vehicleId;
        private readonly Uri _expectedRequestUri;

        protected GetVehicleConfigSuccessTestsBase(ITestOutputHelper output, bool useCustomBaseUri)
            : base(output, useCustomBaseUri)
        {
            // Arrange
            _expected = Fixture.Create<ResponseDataWrapper<VehicleConfig>>();
            _vehicleId = Fixture.Create<long>();
            Handler.SetResponseContent(_expected);
            _expectedRequestUri = new Uri(BaseUri, $"/api/1/vehicles/{_vehicleId}/data_request/vehicle_config");
        }

        [Fact]
        public async Task Should_make_a_GET_request()
        {
            // Act
            await Sut.GetVehicleConfigAsync(_vehicleId, AccessToken).ConfigureAwait(false);

            // Assert
            Handler.Request.Method.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public async Task Should_request_the_vehicle_config_endpoint()
        {
            // Act
            await Sut.GetVehicleConfigAsync(_vehicleId, AccessToken).ConfigureAwait(false);

            // Assert
            Handler.Request.RequestUri.Should().Be(_expectedRequestUri);
        }

        [Fact]
        public async Task Should_return_the_expected_vehicle_config()
        {
            // Act
            IMessageResponse<IResponseDataWrapper<IVehicleConfig>> actual =
                await Sut.GetVehicleConfigAsync(_vehicleId, AccessToken).ConfigureAwait(false);

            // Assert
            actual.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            actual.Data.Should().BeEquivalentTo(_expected, WithStrictOrdering);
        }

        [Fact]
        public async Task Should_set_the_bearer_token_with_the_specified_access_token()
        {
            // Act
            await Sut.GetVehicleConfigAsync(_vehicleId, AccessToken).ConfigureAwait(false);

            // Assert
            Handler.Request.Headers.Authorization.Scheme.Should().Be("Bearer");
            Handler.Request.Headers.Authorization.Parameter.Should().Be(AccessToken);
        }

        [Fact]
        public async Task Should_NOT_set_the_bearer_token_if_the_access_token_is_not_specified()
        {
            // Act
            await Sut.GetVehicleConfigAsync(_vehicleId).ConfigureAwait(false);

            // Assert
            Handler.Request.Headers.Authorization.Should().BeNull();
        }
    }

    public class When_getting_the_vehicle_config_for_a_vehicle_using_the_default_base_Uri : GetVehicleConfigSuccessTestsBase
    {
        public When_getting_the_vehicle_config_for_a_vehicle_using_the_default_base_Uri(ITestOutputHelper output)
            : base(output, useCustomBaseUri: false)
        {
        }
    }

    public class When_getting_the_vehicle_config_for_a_vehicle_using_a_custom_base_Uri : GetVehicleConfigSuccessTestsBase
    {
        public When_getting_the_vehicle_config_for_a_vehicle_using_a_custom_base_Uri(ITestOutputHelper output)
            : base(output, useCustomBaseUri: true)
        {
        }
    }

    public abstract class GetVehicleConfigFailureTestsBase : ClientRequestContext
    {
        private readonly long _vehicleId;

        protected GetVehicleConfigFailureTestsBase(ITestOutputHelper output, bool useCustomBaseUri)
            : base(output, useCustomBaseUri)
        {
            // Arrange
            _vehicleId = Fixture.Create<long>();
            Handler.SetResponse(HttpStatusCode.BadGateway);
        }

        [Fact]
        public async Task Should_return_the_error_status_code()
        {
            // Act
            IMessageResponse actual = await Sut.GetVehicleConfigAsync(_vehicleId, AccessToken).ConfigureAwait(false);

            // Assert
            actual.HttpStatusCode.Should().Be(HttpStatusCode.BadGateway);
        }
    }

    public class When_failing_to_get_the_vehicle_config_for_a_vehicle_using_the_default_base_Uri
        : GetVehicleConfigFailureTestsBase
    {
        public When_failing_to_get_the_vehicle_config_for_a_vehicle_using_the_default_base_Uri(ITestOutputHelper output)
            : base(output, useCustomBaseUri: false)
        {
        }
    }

    public class When_failing_to_get_the_vehicle_config_for_a_vehicle_using_a_custom_base_Uri
        : GetVehicleConfigFailureTestsBase
    {
        public When_failing_to_get_the_vehicle_config_for_a_vehicle_using_a_custom_base_Uri(ITestOutputHelper output)
            : base(output, useCustomBaseUri: true)
        {
        }
    }

    public class When_getting_the_vehicle_config_for_a_vehicle_the_raw_JSON : ClientRequestContext
    {
        private readonly JObject _expected;
        private readonly long _vehicleId;

        public When_getting_the_vehicle_config_for_a_vehicle_the_raw_JSON(ITestOutputHelper output)
            : base(output, useCustomBaseUri: false)
        {
            // Arrange
            _expected = SampleJson.GetVehicleConfigResponse;
            _vehicleId = Fixture.Create<long>();

            // Add random values to test whether it is correctly passed through.
            _expected["randomValue1"] = Fixture.Create("randomValue1");
            _expected["randomValue2"] = JObject.FromObject(new { fakeId = Guid.NewGuid() });
            _expected["response"]["randomValue3"] = Fixture.Create("randomValue3");

            Handler.SetResponseContent(_expected);
        }

        [Fact]
        public async Task Should_be_passed_through_in_the_response()
        {
            // Act
            IMessageResponse response = await Sut.GetVehicleConfigAsync(_vehicleId).ConfigureAwait(false);

            // Assert
            response.RawJsonAsString.Should().Be(_expected.ToString(Formatting.None));
        }
    }
}
