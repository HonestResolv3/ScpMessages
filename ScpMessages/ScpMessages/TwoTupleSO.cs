using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScpMessages
{
    public class TwoTupleSO
    {
        public string Item1 { get; set; }
        public object Item2 { get; set; }

        public TwoTupleSO(string S, object O)
        {
            Item1 = S;
            Item2 = O;
        }
    }
}
