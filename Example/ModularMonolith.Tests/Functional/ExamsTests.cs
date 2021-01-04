using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class ExamsTests : BaseHttpTests
    {
        private readonly long _physicsSubjectId = 2;
        private readonly long _londonLocationId = 3;

        [Test]
        public async Task ShouldAddThenEditExamAndReturnItOnList()
        {
            var httpClient = await PrepareClientWithTokenForScopes();
            var createdExamLocation = await CreateExam(httpClient, capacity: 10, examDateAddDays: 20);
            var examDto = await GetExam(httpClient, createdExamLocation);

            examDto.SubjectId.Should().Be(_physicsSubjectId);
            examDto.LocationId.Should().Be(_londonLocationId);
            examDto.Capacity.Should().Be(10);

            await UpdateExam(httpClient, createdExamLocation, capacity: 20, examDateAddDays: 20);
            var updatedExamDto = await GetExam(httpClient, createdExamLocation);

            updatedExamDto.SubjectId.Should().Be(_physicsSubjectId);
            updatedExamDto.LocationId.Should().Be(_londonLocationId);
            updatedExamDto.Capacity.Should().Be(20);

            var allExams = await GetAllExams(httpClient);
            allExams.Count().Should().Be(1);

            await DeleteExam(httpClient, createdExamLocation);
            
            var allExamsAfterDeletion = await GetAllExams(httpClient);
            allExamsAfterDeletion.Count().Should().Be(0);
        }

        private async Task<IEnumerable<ExamDto>> GetAllExams(HttpClient httpClient)
        {
            var getResult = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl, "/api/exams"));
            getResult.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<IEnumerable<ExamDto>>(await getResult.Content.ReadAsStringAsync());
        }

        private async Task<ExamDto> GetExam(HttpClient httpClient, Uri resourceLocation)
        {
            var getResult = await httpClient.GetAsync(new Uri(MonolithSettings.BaseUrl,
                resourceLocation));
            getResult.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<ExamDto>(await getResult.Content.ReadAsStringAsync());
        }

        private async Task DeleteExam(HttpClient httpClient, Uri resourceLocation)
        {
            var deleteResult = await httpClient.DeleteAsync(new Uri(MonolithSettings.BaseUrl,
                resourceLocation));
            deleteResult.IsSuccessStatusCode.Should().Be(true);
        }

        private async Task UpdateExam(HttpClient httpClient, Uri resourceLocation, int capacity, int examDateAddDays)
        {
            var examUpdateRequest = Serialize(new EditExamRequest()
            {
                Capacity = capacity,
                RegistrationStartDate = DateTime.UtcNow.AddDays(examDateAddDays - 15).Date,
                RegistrationEndDate = DateTime.UtcNow.AddDays(examDateAddDays - 5).Date
            });
            var examUpdateResult = await httpClient.PutAsync(new Uri(MonolithSettings.BaseUrl,
                resourceLocation), examUpdateRequest);
            examUpdateResult.IsSuccessStatusCode.Should().Be(true);
        }

        private async Task<Uri> CreateExam(HttpClient httpClient, int capacity, int examDateAddDays)
        {
            var examCreationRequest = Serialize(new CreateExamRequest()
            {
                Capacity = capacity,
                LocationId = _londonLocationId,
                ExamDateTime = DateTime.UtcNow.AddDays(examDateAddDays).Date,
                SubjectId = _physicsSubjectId,
                RegistrationStartDate = DateTime.UtcNow.AddDays(examDateAddDays - 15).Date,
                RegistrationEndDate = DateTime.UtcNow.AddDays(examDateAddDays - 5).Date
            });
            var examCreationResult = await httpClient.PostAsync(new Uri(MonolithSettings.BaseUrl, "/api/exams"),
                examCreationRequest);

            examCreationResult.IsSuccessStatusCode.Should().Be(true);

            return examCreationResult.Headers.Location;
        }
    }
}