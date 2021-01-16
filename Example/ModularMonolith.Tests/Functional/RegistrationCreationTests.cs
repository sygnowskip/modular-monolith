using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    [Ignore("Registrations are currently in obsolete architecture, will be changed soon")]
    public class RegistrationCreationTests : BaseHttpTests
    {
        [Test]
        public async Task ShouldCreateRegistration()
        {
            var httpClient = await PrepareClientWithTokenForScopes();

            var creationRequestContent = Serialize(new RegistrationCreationRequest("John", "Smith",
                new DateTime(1980, 03, 01)));
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