using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hexure.Results;
using Hexure.Time;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Domain.ValueObjects;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Subjects;
using Moq;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Exams
{
    [TestFixture]
    public class ExamCreationTests
    {
        private SubjectId _subjectId;
        private LocationId _locationId;
        private Capacity _capacity;
        private Mock<ISystemTimeProvider> _systemTimeProvider;
        private Mock<IExamRepository> _examRepository;

        [SetUp]
        public void SetUp()
        {
            _subjectId = ValueObjectProvider.GetSubjectId(10);
            _locationId = ValueObjectProvider.GetLocationId(11);
            _capacity = Capacity.Create(15).Value;
            
            _systemTimeProvider = new Mock<ISystemTimeProvider>();
            _systemTimeProvider.Setup(provider => provider.UtcNow)
                .Returns(new DateTime(2020, 02, 01, 00, 00, 00, DateTimeKind.Utc));
            
            _examRepository = new Mock<IExamRepository>();
            _examRepository.Setup(repository => repository.SaveAsync(It.IsAny<Exam>()))
                .Returns<Exam>(exam => Task.FromResult(Result.Ok(exam)));
        }
        
        [Test]
        public async Task ShouldSuccessfullyCreateExam()
        {
            var examDateTime = UtcDateTime.Create(new DateTime(2020, 03, 10, 12, 00, 00, DateTimeKind.Utc)).Value;
            var registrationStartDate = UtcDate.Create(new DateTime(2020, 02, 08, 00, 00, 00, DateTimeKind.Utc)).Value;
            var registrationEndDate = UtcDate.Create(new DateTime(2020, 03, 08, 00, 00, 00, DateTimeKind.Utc)).Value;

            var examResult = await Exam.CreateAsync(_subjectId, _locationId, examDateTime, _capacity,
                registrationStartDate, registrationEndDate, _systemTimeProvider.Object, _examRepository.Object);

            examResult.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotCreateExamInThePast()
        {
            var examDateTime = UtcDateTime.Create(new DateTime(2020, 01, 10, 12, 00, 00, DateTimeKind.Utc)).Value;
            var registrationStartDate = UtcDate.Create(new DateTime(2020, 02, 08, 00, 00, 00, DateTimeKind.Utc)).Value;
            var registrationEndDate = UtcDate.Create(new DateTime(2020, 03, 08, 00, 00, 00, DateTimeKind.Utc)).Value;

            var examResult = await Exam.CreateAsync(_subjectId, _locationId, examDateTime, _capacity,
                registrationStartDate, registrationEndDate, _systemTimeProvider.Object, _examRepository.Object);

            examResult.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task ShouldNotCreateExamWithRegistrationEndDateBeforeRegistrationStartDate()
        {
            var examDateTime = UtcDateTime.Create(new DateTime(2020, 03, 10, 12, 00, 00, DateTimeKind.Utc)).Value;
            var registrationStartDate = UtcDate.Create(new DateTime(2020, 02, 08, 00, 00, 00, DateTimeKind.Utc)).Value;
            var registrationEndDate = UtcDate.Create(new DateTime(2020, 02, 07, 00, 00, 00, DateTimeKind.Utc)).Value;

            var examResult = await Exam.CreateAsync(_subjectId, _locationId, examDateTime, _capacity,
                registrationStartDate, registrationEndDate, _systemTimeProvider.Object, _examRepository.Object);

            examResult.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task ShouldNotCreateExamWithRegistrationEndDateAfterExamDate()
        {
            var examDateTime = UtcDateTime.Create(new DateTime(2020, 03, 10, 12, 00, 00, DateTimeKind.Utc)).Value;
            var registrationStartDate = UtcDate.Create(new DateTime(2020, 02, 08, 00, 00, 00, DateTimeKind.Utc)).Value;
            var registrationEndDate = UtcDate.Create(new DateTime(2020, 03, 12, 00, 00, 00, DateTimeKind.Utc)).Value;

            var examResult = await Exam.CreateAsync(_subjectId, _locationId, examDateTime, _capacity,
                registrationStartDate, registrationEndDate, _systemTimeProvider.Object, _examRepository.Object);

            examResult.IsSuccess.Should().BeFalse();
        }
    }
}