using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Contract;

public record CategoryByPrimaryKey(int Id) : IRequest<Category?>;
