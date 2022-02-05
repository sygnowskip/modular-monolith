using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class RegistrationCreationTests : BaseScenariosTests
    {
        [Test]
        public async Task ShouldCreateRegistration()
        {
            var existingExam = await Scenarios.Exams.Given().HaveCreatedExamAsync(capacity: 10, examDateAddDays: 50, 1, 1);
            await Scenarios.Exams.And().ExamIsAvailableAsync(existingExam.Id);

            var createdRegistration = await Scenarios.Registrations.When().CreateRegistrationAsync(existingExam.Id);

            await Scenarios.Orders.Then().OrderShouldBeCreatedAsync(createdRegistration.OrderId);
            await Scenarios.Exams.And().ExamShouldHaveBooking(existingExam.Id);
            createdRegistration.Should().NotBeNull();
        }
    }
}