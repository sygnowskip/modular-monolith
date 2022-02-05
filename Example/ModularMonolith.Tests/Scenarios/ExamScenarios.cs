using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Tests.Common;
using Newtonsoft.Json;

namespace ModularMonolith.Tests.Scenarios
{
    public class ExamScenarios
    {
        private readonly MonolithApiSettings _monolithApiSettings;
        private readonly IHttpClientProvider _httpClientProvider;

        public ExamScenarios(MonolithApiSettings monolithApiSettings, IHttpClientProvider httpClientProvider)
        {
            _monolithApiSettings = monolithApiSettings;
            _httpClientProvider = httpClientProvider;
        }

        public ExamScenarios Given() => this;
        public ExamScenarios When() => this;
        public ExamScenarios Then() => this;
        public ExamScenarios And() => this;

        public async Task DeleteExamAsync(long examId)
        {
            var httpClient = await PrepareClientAsync();
            var deleteResult = await httpClient.DeleteAsync(new Uri(_monolithApiSettings.BaseUrl,
                $"/api/exams/{examId}"));
            deleteResult.IsSuccessStatusCode.Should().Be(true);
        }

        public async Task UpdateExamAsync(long examId, int capacity, int examDateAddDays)
        {
            var httpClient = await PrepareClientAsync();
            var examUpdateRequest = _httpClientProvider.Serialize(new EditExamRequest()
            {
                Capacity = capacity,
                RegistrationStartDate = DateTime.UtcNow.AddDays(examDateAddDays - 15).Date,
                RegistrationEndDate = DateTime.UtcNow.AddDays(examDateAddDays - 5).Date
            });
            var examUpdateResult = await httpClient.PutAsync(new Uri(_monolithApiSettings.BaseUrl,
                $"/api/exams/{examId}"), examUpdateRequest);
            examUpdateResult.IsSuccessStatusCode.Should().Be(true);
        }

        public async Task<ExamDto> HaveCreatedExamAsync(int capacity, int examDateAddDays, long locationId, long subjectId)
        {
            var httpClient = await PrepareClientAsync();
            var examCreationRequest = _httpClientProvider.Serialize(new CreateExamRequest()
            {
                Capacity = capacity,
                LocationId = locationId,
                ExamDateTime = DateTime.UtcNow.AddDays(examDateAddDays).Date,
                SubjectId = subjectId,
                RegistrationStartDate = DateTime.UtcNow.AddDays(examDateAddDays - 15).Date,
                RegistrationEndDate = DateTime.UtcNow.AddDays(examDateAddDays - 5).Date
            });
            var examCreationResult = await httpClient.PostAsync(new Uri(_monolithApiSettings.BaseUrl, "/api/exams"),
                examCreationRequest);

            var response = await examCreationResult.Content.ReadAsStringAsync();
            examCreationResult.IsSuccessStatusCode.Should().BeTrue();

            return await GetExamAsync(examCreationResult.Headers.Location);
        }

        public async Task ExamIsAvailableAsync(long examId)
        {
            var uri = new Uri(_monolithApiSettings.BaseUrl, $"/api/exams/{examId}/open");
            var httpClient = await PrepareClientAsync();
            var openingExamResult =
                await httpClient.PatchAsync(new Uri(_monolithApiSettings.BaseUrl, $"/api/exams/{examId}/open"), null);

            openingExamResult.IsSuccessStatusCode.Should().BeTrue();
        }

        public async Task ExamsCountShouldBeEqualToAsync(int expectedExamsCount)
        {
            var allExams = await GetAllExamsAsync();
            allExams.Count().Should().Be(expectedExamsCount);
        }

        public async Task ExamShouldHaveBooking(long examId)
        {
            var exam = await GetExamAsync(examId);
            exam.Booked.Should().Be(1);
        }

        private async Task<IEnumerable<ExamDto>> GetAllExamsAsync()
        {
            var httpClient = await PrepareClientAsync();
            var getResult = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, "/api/exams"));
            getResult.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<IEnumerable<ExamDto>>(await getResult.Content.ReadAsStringAsync());
        }

        private async Task<ExamDto> GetExamAsync(Uri resourceLocation)
        {
            var httpClient = await PrepareClientAsync();
            var getResult = await httpClient.GetAsync(new Uri(_monolithApiSettings.BaseUrl, resourceLocation));
            getResult.IsSuccessStatusCode.Should().Be(true);

            return JsonConvert.DeserializeObject<ExamDto>(await getResult.Content.ReadAsStringAsync());
        }
        
        private async Task<ExamDto> GetExamAsync(long examId)
        {
            return await GetExamAsync(new Uri($"/api/exams/{examId}", UriKind.Relative));
        }

        private Task<HttpClient> PrepareClientAsync() =>
            _httpClientProvider.PrepareClientWithTokenForScopesAsync(nameof(ExamScenarios));
    }
}