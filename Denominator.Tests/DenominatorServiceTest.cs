using System.Text.Json;
using Xunit.Abstractions;

namespace Denominator.Tests;

public class DenominatorServiceTest
{
    private readonly ITestOutputHelper output;

    public DenominatorServiceTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [InlineData(30)]
    [InlineData(50)]
    [InlineData(60)]
    [InlineData(80)]
    [InlineData(140)]
    [InlineData(230)]
    [InlineData(370)]
    [InlineData(610)]
    [InlineData(980)]
    public void T1TestPayouts(int payout)
    {
        output.WriteLine(JsonSerializer.Serialize(payout));
        var combinations = DenominatorService.CalculateCombinations(payout);
        foreach (var comb in combinations)
        {
            output.WriteLine(JsonSerializer.Serialize(combinations));
            Assert.Equal(comb.Sum(), payout);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(13)]
    public void T2TestInvalidPayouts(int payout)
    {
        output.WriteLine(JsonSerializer.Serialize(payout));
        var combinations = DenominatorService.CalculateCombinations(payout);
        foreach (var comb in combinations)
        {
            output.WriteLine(JsonSerializer.Serialize(combinations));
            Assert.True(!comb.Any());
        }
    }
}