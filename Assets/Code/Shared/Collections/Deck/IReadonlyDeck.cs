﻿using System.Collections.Generic;

namespace Framework.Shared.Collections
{
    public interface IReadonlyDeck<T> : IReadOnlyCollection<T>
    {
        int Capacity { get; }

        bool IsEmpty { get; }

        T Top { get; }
        T Bottom { get; }
    }
}