﻿
using System;
using System.Collections;
using System.Collections.Generic;

using Framework.Durak.Datas;
using Framework.Shared.Cards.Views;

namespace Framework.Durak.Players
{
    [Serializable]
    public class Hand : IHand
    {
        private readonly List<Data> collection = new List<Data>();

        private readonly CardLookSide lookSide;

        public int Count => collection.Count;

        public CardLookSide LookSide => lookSide;
        public int Hash => GetHashCode();
        public Hand(CardLookSide lookSide)
        {
            this.lookSide = lookSide;
        }


        public Data this[int index] => collection[index];

        public void Add(Data data)
        {
            collection.Add(data);
        }

        public void AddRange(IEnumerable<Data> datas)
        {
            collection.AddRange(datas);
        }

        public void Remove(Data data)
        {
            collection.Remove(data);
        }

        public bool Contains(Data data)
        {
            return collection.Contains(data);
        }
        
        public void Clear()
        {
            collection.Clear();
        }
        public object Clone() => MemberwiseClone();
        public IEnumerator<Data> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
    }
}