﻿using System.Collections.Generic;

namespace Framework.Shared.Collections
{
    public interface IReadonlyBoard<T> : IBoardReference, IReadOnlyCollection<T>
    {
        IReadOnlyList<T> All { get; }
        IReadOnlyList<T> Attacks { get; }
        IReadOnlyList<T> Defends { get; }
        IReadOnlyList<T> Seen { get; }
        bool IsAttacksPlace { get; }
        bool IsDefendsPlace { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }
        bool IsAttacksFull { get; }
        bool IsDefendsFull { get; }
    }
}
