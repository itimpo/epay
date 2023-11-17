namespace Denominator;

public class DenominatorService
{
    private static readonly int[] _denominations = [10, 50, 100];

    /// <summary>
    /// Calculates all possible combinations
    /// </summary>
    /// <param name="amount">Money amount</param>
    /// <returns>List of possible combinations</returns>
    public static List<List<int>> CalculateCombinations(int amount)
    {
        return CalculateCombinations(amount, new List<int>());
    }

    static List<List<int>> CalculateCombinations(int remainingAmount, List<int> currentCombination, int startIndex = 0)
    {
        var result = new List<List<int>>();

        if (remainingAmount == 0)
        {
            result.Add(new List<int>(currentCombination));
            return result;
        }

        for (int i = startIndex; i < _denominations.Length; i++)
        {
            if (remainingAmount >= _denominations[i])
            {
                currentCombination.Add(_denominations[i]);

                var subCombinations = CalculateCombinations(remainingAmount - _denominations[i], currentCombination, i);

                result.AddRange(subCombinations);

                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        return result;
    }
}
