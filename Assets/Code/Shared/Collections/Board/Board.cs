﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Shared.Collections
{
    [Serializable]
    public class Board<T> : IBoard<T>
    {
        private readonly List<T> all;
        private readonly List<T>[] places;
        private readonly List<T> seen;

        private readonly int rowItemsCount;

        private int index;

        public IReadOnlyList<T> All => all;
        public IReadOnlyList<T> Attacks => places[BoardIndexes.attacks];
        public IReadOnlyList<T> Defends => places[BoardIndexes.defends];
        public IReadOnlyList<T> Seen => seen;

        public bool IsAttacksPlace => index == BoardIndexes.attacks;
        public bool IsDefendsPlace => index == BoardIndexes.defends;

        public bool IsEmpty => Count == 0;
        public bool IsFull => all.Count == rowItemsCount * 2;
        public bool IsAttacksFull => Attacks.Count == rowItemsCount;
        public bool IsDefendsFull => Defends.Count == rowItemsCount;

        public int Count => all.Count;


        public Board(int rowItemsCount)
        {
            all = new List<T>(rowItemsCount * BoardIndexes.count);
            places = new List<T>[BoardIndexes.count];
            seen = new List<T>();
            this.rowItemsCount = rowItemsCount;

            for (int i = 0; i < BoardIndexes.count; i++)
            {
                places[i] = new List<T>(rowItemsCount);
            }
        }


        public void Add(T item)
        {
            List<T> place = places[index];

            if (place.Count == rowItemsCount)
            {
                throw new System.IndexOutOfRangeException();
            }
            if (place.Contains(item))
            {
                throw new System.ArgumentException();
            }

            index = (index + 1) % BoardIndexes.count;

            all.Add(item);
            place.Add(item);
        }

        public void AddToAttacks(T item)
        {
            index = BoardIndexes.attacks;

            Add(item);
        }
        public void AddToDefends(T item)
        {
            index = BoardIndexes.defends;

            Add(item);
        }

        public void Clear()
        {
            foreach (var place in places)
            {
                place.Clear();
            }

            all.Clear();

            index = 0;
        }

        public void AddSeen(T item)
        {
            seen.Add(item);
        }
        public void RemoveSeen(T item)
        {
            seen.Remove(item);
        }
        public List<T>.Enumerator GetEnumerator() => all.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => all.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => all.GetEnumerator();

        private readonly struct BoardIndexes
        {
            public const int attacks = 0;
            public const int defends = 1;
            public const int count = 2;
        }
    }
}