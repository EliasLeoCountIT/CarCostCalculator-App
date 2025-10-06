using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.KilometerEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic
{
    public class KilometerEntryCommandHandler(IKilometerEntryRepository repository)
        : IRequestHandler<AddKilometerEntry, KilometerEntryCore?>
    {
        private readonly IKilometerEntryRepository _repository = repository;

        public async Task<KilometerEntryCore?> Handle(AddKilometerEntry request, CancellationToken cancellationToken)
            => await _repository.Create(request.KilometerEntry, cancellationToken);

    }
}
