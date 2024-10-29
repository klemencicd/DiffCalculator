using DiffCalculatorApi.Models;

namespace DiffCalculatorApi.Repositories.Interfaces;

public interface IDiffRepository
{
    void AddRight(int id, string data);
    void AddLeft(int id, string data);
    Right? GetRight(int id);
    Left? GetLeft(int id);
}
