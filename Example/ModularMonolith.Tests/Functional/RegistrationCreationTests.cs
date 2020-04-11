using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class RegistrationCreationTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldCreateRegistration()
        {
            var httpClient = await PrepareClientWithTokenForScopes();

            var creationRequestContent =
                new StringContent(
                    JsonConvert.SerializeObject(new RegistrationCreationRequest("John", "Smith",
                        new DateTime(1980, 03, 01))), Encoding.UTF8, "application/json");
            var creationResult = await httpClient.PostAsync(new Uri(MonolithSettings.BaseUrl, "/api/registrations"),
                creationRequestContent);

            creationResult.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdResourceLocation = creationResult.Headers.Location;

            var getResult = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl,
                createdResourceLocation));
            getResult.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}