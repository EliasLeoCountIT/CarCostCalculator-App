using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.KilometerEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic
{
    public class KilometerEntryQueryHandler(IKilometerEntryRepository repository)
    : IRequestHandler<KilometerEntryByPrimaryKey, KilometerEntry?>
    {
        private readonly IKilometerEntryRepository _repository = repository;
        public Task<KilometerEntry?> Handle(KilometerEntryByPrimaryKey request, CancellationToken cancellationToken)
            => _repository.LoadByPrimaryKey(request.Id, cancellationToken);
    }
}
