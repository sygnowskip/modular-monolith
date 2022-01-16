using System.Threading.Tasks;
using Hexure.Results;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Providers;
using ModularMonolith.Language.Pricing;

namespace ModularMonolith.Exams.Persistence.Providers
{
    public class ExamPricingProvider : IExamPricingProvider
    {
        private readonly ISingleCurrencyPolicy _singleCurrencyPolicy;

        public ExamPricingProvider(ISingleCurrencyPolicy singleCurrencyPolicy)
        {
            _singleCurrencyPolicy = singleCurrencyPolicy;
        }

        public async Task<Result<Price>> GetPriceAsync(ExamId examId)
        {
            return Price.Create(
                net: Money.Create(100, SupportedCurrencies.USD).Value,
                tax: Money.Create(0, SupportedCurrencies.USD).Value,
                _singleCurrencyPolicy);
        }
    }
}