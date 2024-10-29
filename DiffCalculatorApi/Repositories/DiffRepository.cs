using DiffCalculatorApi.Models;
using DiffCalculatorApi.Repositories.Interfaces;
using System.Buffers.Text;

namespace DiffCalculatorApi.Repositories;

public class DiffRepository : IDiffRepository
{
    private static readonly Dictionary<int, Left> _left = [];
    private static readonly Dictionary<int, Right> _right = [];

    public void AddLeft(int id, string data)
    {
        Left left = new(id, Convert.FromBase64String(data));

        _left.TryAdd(id, left);
        _left[id] = left;
    }

    public Left? GetLeft(int id)
    {
        _ = _left.TryGetValue(id, out Left? left);
        return left;
    }

    public void AddRight(int id, string data)
    {
        if (!Base64.IsValid(data))
        {
            throw new FormatException("The input is not a valid Base-64 string");
        }

        Right right = new(id, Convert.FromBase64String(data));

        _right.TryAdd(id, right);
        _right[id] = right;
    }

    public Right? GetRight(int id)
    {
        _ = _right.TryGetValue(id, out Right? right);
        return right;
    }
}
