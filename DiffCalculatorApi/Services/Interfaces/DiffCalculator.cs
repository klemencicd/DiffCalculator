using DiffCalculatorApi.ViewModels;

namespace DiffCalculatorApi.Services.Interfaces;

public interface IDiffCalculator
{
    DiffResult Calculate(int id);
}
