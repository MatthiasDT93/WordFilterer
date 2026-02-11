using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.UI;

public interface IUserInput
{
    public void EnterTargetLength(int targetLength = 6);
}
