using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScpMessages
{
    public class TwoTupleSOList
    {
        public List<TwoTupleSO> Tuples { get; set; }

        public TwoTupleSOList()
        {
            Tuples = new List<TwoTupleSO>();
        }

        public void AddTwoTupleSO(TwoTupleSO TT1)
        {
            Tuples.Clear();
            Tuples.Add(TT1);
        }

        public void AddTwoTupleSO(TwoTupleSO TT1, TwoTupleSO TT2)
        {
            Tuples.Clear();
            Tuples.Add(TT1);
            Tuples.Add(TT2);
        }

        public void AddTwoTupleSO(TwoTupleSO TT1, TwoTupleSO TT2, TwoTupleSO TT3)
        {
            Tuples.Clear();
            Tuples.Add(TT1);
            Tuples.Add(TT2);
            Tuples.Add(TT3);
        }
    }
}
