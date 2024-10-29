namespace DiffCalculatorApi.ViewModels;

public class DiffResult
{
    public string DiffResultType { get; set; } = string.Empty;
    public List<Diff> Diffs { get; set; } = [];
}

public record Diff(int Offset, int Length);
