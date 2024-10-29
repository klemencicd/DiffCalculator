using DiffCalculatorApi.Enums;
using DiffCalculatorApi.Exceptions;
using DiffCalculatorApi.Models;
using DiffCalculatorApi.Repositories.Interfaces;
using DiffCalculatorApi.Services.Interfaces;
using DiffCalculatorApi.ViewModels;

namespace DiffCalculatorApi.Services;

public class DiffCalculator(IDiffRepository _repository) : IDiffCalculator
{
    public DiffResult Calculate(int id)
    {
        Right? right = _repository.GetRight(id) ?? throw new NotFoundException("Right cannot be null");
        Left? left = _repository.GetLeft(id) ?? throw new NotFoundException("Left cannot be null");

        if (right.Data.Length != left.Data.Length)
        {
            return new DiffResult
            {
                DiffResultType = $"{DiffResultTypes.SizeDoNotMatch}"
            };
        }

        DiffResult diffResult = new()
        {
            DiffResultType = string.Empty,
            Diffs = []
        };

        bool areEqual = true;
        for (int i = 0; i < right.Data.Length; i++)
        {
            if (right.Data[i] == left.Data[i]) continue;

            areEqual = false;
            int offset = i;

            while (i < right.Data.Length && right.Data[i] != left.Data[i])
            {
                i++;
            }

            diffResult.Diffs.Add(new Diff(offset, i - offset));
        }

        diffResult.DiffResultType = areEqual ? $"{DiffResultTypes.Equals}" : $"{DiffResultTypes.ContentDoNotMatch}";
        return diffResult;
    }
}
