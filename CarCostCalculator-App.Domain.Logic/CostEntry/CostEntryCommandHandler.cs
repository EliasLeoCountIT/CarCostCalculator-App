using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.CostEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic.CostEntry
{
    public class CostEntryCommandHandler(ICostEntryRepository repository)
        : IRequestHandler<AddCostEntry, CostEntryCore?>
    {
        private readonly ICostEntryRepository _repository = repository;

        public async Task<CostEntryCore?> Handle(AddCostEntry request, CancellationToken cancellationToken)
            => await _repository.Create(request.CostEntry, cancellationToken);

    }
}
