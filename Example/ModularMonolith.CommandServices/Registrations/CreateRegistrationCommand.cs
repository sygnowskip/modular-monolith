using System.Threading;
using System.Threading.Tasks;
using Hexure.MediatR;
using Hexure.Results;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using ModularMonolith.Contracts.Registrations;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Providers;
using ModularMonolith.Exams.Language.Validators;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain;
using ModularMonolith.Orders.Domain.Policies;
using ModularMonolith.Orders.Domain.ValueObjects;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.CommandServices.Registrations
{
    public class CreateRegistrationCommand : ICommandRequest<RegistrationId>
    {
        private CreateRegistrationCommand(Candidate candidate, ExamId examId, ContactData buyer)
        {
            Candidate = candidate;
            ExamId = examId;
            Buyer = buyer;
        }

        public Candidate Candidate { get; }
        public ExamId ExamId { get; }
        public ContactData Buyer { get; }

        public static Result<CreateRegistrationCommand> Create(CreateRegistrationRequest request,
            ISystemTimeProvider systemTimeProvider, IExamExistenceValidator examExistenceValidator)
        {
            var candidateResult = DateOfBirth.Create(request.DateOfBirth, systemTimeProvider)
                .OnSuccess(dateOfBirth => Candidate.Create(request.FirstName, request.LastName, dateOfBirth));
            var examResult = ExamId.Create(request.ExamId, examExistenceValidator);
            var buyerResult = ContactData.Create(request.Buyer?.Name, request.Buyer?.StreetAddress, request.Buyer?.City,
                request.Buyer?.ZipCode);

            return Result.Combine(candidateResult, examResult, buyerResult)
                .OnSuccess(() =>
                    new CreateRegistrationCommand(candidateResult.Value, examResult.Value, buyerResult.Value));
        }
    }

    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, Result<RegistrationId>>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IExamRepository _examRepository;
        private readonly ISystemTimeProvider _systemTimeProvider;
        private readonly ISingleCurrencyPolicy _singleCurrencyPolicy;
        private readonly ISingleItemsCurrencyPolicy _singleItemsCurrencyPolicy;
        private readonly IExamPricingProvider _examPricingProvider;

        public CreateRegistrationCommandHandler(IRegistrationRepository registrationRepository,
            ISystemTimeProvider systemTimeProvider, ISingleCurrencyPolicy singleCurrencyPolicy,
            ISingleItemsCurrencyPolicy singleItemsCurrencyPolicy, IExamPricingProvider examPricingProvider,
            IOrderRepository orderRepository, IExamRepository examRepository)
        {
            _registrationRepository = registrationRepository;
            _systemTimeProvider = systemTimeProvider;
            _singleCurrencyPolicy = singleCurrencyPolicy;
            _singleItemsCurrencyPolicy = singleItemsCurrencyPolicy;
            _examPricingProvider = examPricingProvider;
            _orderRepository = orderRepository;
            _examRepository = examRepository;
        }

        public async Task<Result<RegistrationId>> Handle(CreateRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            var externalRegistrationId = new ExternalRegistrationId();
            var orderResult = await CreateOrder(externalRegistrationId, request.ExamId, request.Buyer);
            var bookPlaceResult = await BookPlace(request.ExamId);

            return await Result.Combine(orderResult, bookPlaceResult)
                .OnSuccess(async () =>
                    await Registration.CreateAsync(externalRegistrationId, request.ExamId, orderResult.Value.Id,
                        request.Candidate, _systemTimeProvider, _registrationRepository))
                .OnSuccess(registration => registration.Id);
        }

        private async Task<Result> BookPlace(ExamId examId)
        {
            return await _examRepository.GetAsync(examId)
                .OnSuccess(exam => exam.Book());
        }

        private async Task<Result<Order>> CreateOrder(ExternalRegistrationId externalRegistrationId, ExamId examId,
            ContactData buyer)
        {
            return await CreateRegistrationOrderItem(externalRegistrationId, examId)
                .OnSuccess(item => Order.CreateWithDefaultSellerAsync(buyer, new[] { item }, _systemTimeProvider,
                    _singleCurrencyPolicy,
                    _singleItemsCurrencyPolicy,
                    _orderRepository));
        }

        private async Task<Result<OrderItem>> CreateRegistrationOrderItem(ExternalRegistrationId externalRegistrationId,
            ExamId examId)
        {
            return await _examPricingProvider.GetPriceAsync(examId)
                .OnSuccess(price => OrderItem.Create(Products.Registration.ItemName, Products.Registration.ProductType,
                    externalRegistrationId, 1, price));
        }
    }
}