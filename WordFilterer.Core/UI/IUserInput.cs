using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.UI;

public interface IUserInput
{
    public void CalculateCombinations(int targetLength = 6, bool binaryCombinations = true);
}
