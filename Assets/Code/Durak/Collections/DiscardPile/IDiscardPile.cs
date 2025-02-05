﻿
using System.Collections.Generic;

using Framework.Durak.Datas;

namespace Framework.Durak.Collections
{
    public interface IDiscardPile : IDiscardPileReference, IEnumerable<Data>
    {
        int Count { get; }

        void Add(Data data);
        void AddRange(IEnumerable<Data> datas);

        void Clear();
    }
}