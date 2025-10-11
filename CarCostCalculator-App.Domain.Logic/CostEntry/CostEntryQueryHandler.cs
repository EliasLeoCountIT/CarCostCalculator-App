using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.CostEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic.CostEntry
{
    public class CostEntryQueryHandler(ICostEntryRepository repository)
    : IRequestHandler<CostEntryByPrimaryKey, CostEntryCore?>
    {
        private readonly ICostEntryRepository _repository = repository;
        public Task<CostEntryCore?> Handle(CostEntryByPrimaryKey request, CancellationToken cancellationToken)
            => _repository.LoadByPrimaryKey(request.Id, cancellationToken);
    }

}
