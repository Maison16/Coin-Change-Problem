using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class BoundedCoinChange
{
    static int CountCombinations(int[] coins, int[] counts, int m, int amount, List<int> usedCoins)
    {
        int[] dp = new int[amount + 1];
        dp[0] = 1;

        for (int i = 0; i < m; i++)
        {
            for (int j = amount; j >= 0; j--)
            {
                for (int k = 1; k <= counts[i] && j - k * coins[i] >= 0; k++)
                {
                    dp[j] += dp[j - k * coins[i]];
                }
            }
        }

        GetUsedCoins(coins, counts, amount, usedCoins, dp);

        return dp[amount];
    }

    static void GetUsedCoins(int[] coins, int[] counts, int amount, List<int> usedCoins, int[] dp)
    {
        int remainingAmount = amount;
        int coinIndex = coins.Length - 1;

        while (remainingAmount > 0 && coinIndex >= 0)
        {
            for (int k = counts[coinIndex]; k > 0; k--)
            {
                usedCoins.Add(coins[coinIndex]);
                remainingAmount -= k * coins[coinIndex];
            }

            coinIndex--;
        }
    }

    static void Main()
    {
        string inputFile = "PKO_in_5_Izdebski.txt";
        string outputFile = "PKO_out_5_Izdebski.txt";

        int[] coins;
        int[] counts;
        int amount;
        List<int> usedCoins = new List<int>();
        using (StreamReader reader = new StreamReader(inputFile))
        {
            int n = int.Parse(reader.ReadLine());
            coins = reader.ReadLine().Split().Select(int.Parse).ToArray();
            counts = reader.ReadLine().Split().Select(int.Parse).ToArray();
            amount = int.Parse(reader.ReadLine());
        }

        int result = CountCombinations(coins, counts, coins.Length, amount, usedCoins);

        Console.WriteLine($"Ilość różnych kombinacji wymiany dla kwoty {amount}: {result}");

        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            writer.WriteLine($"liczba monet: {usedCoins.Count}");

            writer.Write("Użyte monety: ");
            var coinCounts = usedCoins.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            var reversedCoinCounts = coinCounts.Reverse();
            foreach (var kvp in reversedCoinCounts)
            {
                writer.Write($" {kvp.Value} ");
            }
        }

        Console.ReadLine();
    }
}