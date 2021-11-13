using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language.Validators;
using ModularMonolith.Exams.Persistence.Validators;

namespace ModularMonolith.Exams.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExamsWritePersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : IExamDbContext
        {
            services.AddTransient<IExamDbContext>(provider => provider.GetService<TDbContext>());
            services.AddTransient<IExamRepository, ExamRepository>();
            services.AddTransient<IExamExistenceValidator, ExamExistenceValidator>();
        }
    }
}