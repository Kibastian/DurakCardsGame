﻿
using UnityEngine;

namespace Framework.Durak.Gameplay.Scriptables
{
    [CreateAssetMenu(fileName = "PlayingDeckSize", menuName = "MyAsset/Durak/Gameplay/Scriptables/PlayingDeckSize")]
    public class PlayingDeckSize : ScriptableObject
    {
        [SerializeField] private int suits = 4;
        [SerializeField] private int ranks = 13;

        public int Suits => suits;
        public int Ranks => ranks;
    }
}