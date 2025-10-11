using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.KilometerEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic.KilometerEntry
{
    public class KilometerEntryQueryHandler(IKilometerEntryRepository repository)
    : IRequestHandler<KilometerEntryByPrimaryKey, KilometerEntryCore?>
    {
        private readonly IKilometerEntryRepository _repository = repository;
        public Task<KilometerEntryCore?> Handle(KilometerEntryByPrimaryKey request, CancellationToken cancellationToken)
            => _repository.LoadByPrimaryKey(request.Id, cancellationToken);
    }
}
