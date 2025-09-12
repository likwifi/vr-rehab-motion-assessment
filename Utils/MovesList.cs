using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace utils
{
    [Serializable]
    public class MovesList<T> : IEnumerable
    {
        public List<T> moves = new List<T>();
        public int Count => moves.Count;

        public void Add(T moveObj)
        {
            moves.Add(moveObj);
        }
    }
}
