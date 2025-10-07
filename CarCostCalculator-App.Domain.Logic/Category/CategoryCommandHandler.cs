using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic.Category
{
    public class CategoryCommandHandler(ICategoryRepository repository)
        : IRequestHandler<DeleteCategory>,
          IRequestHandler<UpdateCategory, CategoryCore?>,
          IRequestHandler<CreateCategory, CategoryCore?>
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task Handle(DeleteCategory request, CancellationToken cancellationToken)
            => await _repository.Delete(request.Id, cancellationToken);

        public async Task<CategoryCore?> Handle(UpdateCategory request, CancellationToken cancellationToken)
            => await _repository.Update(request.Category, cancellationToken);

        public async Task<CategoryCore?> Handle(CreateCategory request, CancellationToken cancellationToken)
            => await _repository.Create(request.Category, cancellationToken);
    }
}
