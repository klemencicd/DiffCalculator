using DiffCalculatorApi.Exceptions;
using DiffCalculatorApi.Models;
using DiffCalculatorApi.Repositories.Interfaces;
using DiffCalculatorApi.Services;
using DiffCalculatorApi.ViewModels;
using Moq;

namespace DiffCalculatorTests;

public class UnitTests
{
    private readonly Mock<IDiffRepository> _repository = new();
    private readonly DiffCalculator _diffCalculator;
    private const int id = 1;

    public UnitTests()
    {
        _diffCalculator = new(_repository.Object);
    }

    [Fact]
    public void Calculate_ShouldThrowNotFoundException_WhenRightIsNull()
    {
        _repository.Setup(x => x.GetRight(id)).Returns((Right?)null);
        _repository.Setup(x => x.GetLeft(id)).Returns(new Left(id, Convert.FromBase64String("AAAAAA==")));

        var exception = Assert.Throws<NotFoundException>(() => _diffCalculator.Calculate(id));
        Assert.Equal("Right cannot be null", exception.Message);
    }

    [Fact]
    public void Calculate_ShouldThrowNotFoundException_WhenLeftIsNull()
    {
        _repository.Setup(x => x.GetRight(id)).Returns(new Right(id, Convert.FromBase64String("AAAAAA==")));
        _repository.Setup(x => x.GetLeft(id)).Returns((Left?)null);

        var exception = Assert.Throws<NotFoundException>(() => _diffCalculator.Calculate(id));
        Assert.Equal("Left cannot be null", exception.Message);
    }

    [Fact]
    public void Calculate_Equal()
    {
        _repository.Setup(x => x.GetRight(id)).Returns(new Right(id, Convert.FromBase64String("AAAAAA==")));
        _repository.Setup(x => x.GetLeft(id)).Returns(new Left(id, Convert.FromBase64String("AAAAAA==")));

        DiffResult result = _diffCalculator.Calculate(id);
        Assert.NotNull(result);
        Assert.Equal("Equals", result.DiffResultType);
        Assert.Empty(result.Diffs);
    }

    [Fact]
    public void Calculate_SizeDoNotMatch()
    {
        _repository.Setup(x => x.GetRight(id)).Returns(new Right(id, Convert.FromBase64String("bGlnaHQgd29yay4=")));
        _repository.Setup(x => x.GetLeft(id)).Returns(new Left(id, Convert.FromBase64String("AAAAAA==")));

        DiffResult result = _diffCalculator.Calculate(id);
        Assert.NotNull(result);
        Assert.Equal("SizeDoNotMatch", result.DiffResultType);
        Assert.Empty(result.Diffs);
    }

    [Fact]
    public void Calculate_ContentDoNotMatch()
    {
        _repository.Setup(x => x.GetRight(id)).Returns(new Right(id, Convert.FromBase64String("bGlnaHQgd29yay4=")));
        _repository.Setup(x => x.GetLeft(id)).Returns(new Left(id, Convert.FromBase64String("bGlaaHBgd29yAy5=")));

        DiffResult result = _diffCalculator.Calculate(id);
        Assert.NotNull(result);
        Assert.Equal("ContentDoNotMatch", result.DiffResultType);
        Assert.Equal(3, result.Diffs.Count);
        Assert.Equal(2, result.Diffs[0].Offset);
        Assert.Equal(1, result.Diffs[0].Length);
        Assert.Equal(4, result.Diffs[1].Offset);
        Assert.Equal(2, result.Diffs[1].Length);
        Assert.Equal(9, result.Diffs[2].Offset);
        Assert.Equal(1, result.Diffs[2].Length);
    }
}