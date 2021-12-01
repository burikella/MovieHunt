using System.Collections.Generic;

namespace MovieHunt
{
    public class ReportBuilder
    {
        private readonly List<string> _amounts = new List<string>();

        public ReportBuilder(bool includeZeroes)
        {
            IncludeZeroes = includeZeroes;
        }
        
        public bool IncludeZeroes { get; }

        public IReadOnlyList<string> Amounts => _amounts.AsReadOnly();

        public void AddAmountIfNeeded(decimal amount)
        {
            if (amount > 0 || (amount == 0 && IncludeZeroes))
            {
                _amounts.Add(amount.ToString("0.00"));
            }
            else if (amount < 0)
            {
                _amounts.Add($"({-amount:0.00})");
            }
        }
    }
}