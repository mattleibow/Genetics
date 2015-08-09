using System;
using System.Collections;
using System.Collections.Generic;

namespace GeneticsTests.TestCases
{
    public class CustomEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class CustomList : List<int>
    {
    }

    public abstract class CustomAbstractList : List<int>
    {
    }

    public class CustomType
    {
    }
}
