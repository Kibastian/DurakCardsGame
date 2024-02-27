
using System;
using System.Collections;
using System.Collections.Generic;       
using Framework.Durak.Datas;

namespace Framework.Durak.Players
{
    [Serializable]
    public class Tree
    {
        
        private List<(Dictionary<Data,int>,double)> collection = new List<(Dictionary<Data, int>, double)>();



        public void AddRange(List<(Dictionary<Data, int>, double)> datas)
        {
            collection.AddRange(datas);
        }

        public void Clear()
        {
            collection.Clear();
        }

    }
}