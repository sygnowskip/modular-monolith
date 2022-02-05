using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language.Providers;
using ModularMonolith.Exams.Language.Validators;
using ModularMonolith.Exams.Persistence.Providers;
using ModularMonolith.Exams.Persistence.Validators;

namespace ModularMonolith.Exams.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExamsWritePersistence<TDbContext>(this IServiceCollection services)
            where TDbContext : IExamsDbContext
        {
            services.AddTransient<IExamsDbContext>(provider => provider.GetService<TDbContext>());
            services.TryAddTransient<IExamRepository, ExamRepository>();
            services.TryAddTransient<IExamExistenceValidator, ExamExistenceValidator>();
            services.TryAddTransient<IExamPricingProvider, ExamPricingProvider>();
        }
    }
}