using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace text
{
    public interface IStemmer
    {
        string Stem(string s);
    }
}
