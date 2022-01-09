using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Language.Pricing;

namespace ModularMonolith.Exams.Language.Providers
{
    public interface IExamPricingProvider
    {
        Task<Result<Price>> GetPriceAsync(ExamId examId);
    }
}