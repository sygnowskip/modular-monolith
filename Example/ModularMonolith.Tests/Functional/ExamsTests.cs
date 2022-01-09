using System.Threading.Tasks;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class ExamsTests : BaseScenariosTests
    {
        private readonly long _physicsSubjectId = 2;
        private readonly long _londonLocationId = 3;

        [Test]
        public async Task ShouldAddThenEditExamAndReturnItOnList()
        {
            var createdExam = await Scenarios.Exams.Given().HaveCreatedExamAsync(capacity: 10, examDateAddDays: 20, locationId: _londonLocationId,
                subjectId: _physicsSubjectId);

            await Scenarios.Exams.When().UpdateExamAsync(createdExam.Id, capacity: 20, examDateAddDays: 20);

            await Scenarios.Exams.Then().ExamsCountShouldBeEqualToAsync(1);

            await Scenarios.Exams.When().DeleteExamAsync(createdExam.Id);
            
            await Scenarios.Exams.Then().ExamsCountShouldBeEqualToAsync(0);
        }
    }
}