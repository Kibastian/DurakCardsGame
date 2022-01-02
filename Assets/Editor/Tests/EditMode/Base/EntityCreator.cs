﻿
using System;
using System.Collections.Generic;

using Framework.Durak.Cards;
using Framework.Durak.Players;
using Framework.Shared.Collections;

using CardLookSide = Framework.Shared.Cards.Views.CardLookSide;

namespace ProjectCard.Editor.TestModule.TestData
{
    public static class EntityCreator
    {
        public static Deck<Data> CreateDeck(int count)
        {
            Data[] datas = CreateDatas(count);

            Deck<Data> deck = new Deck<Data>(datas);

            return deck;
        }

        public static Board<Data> CreateBoard(int row_count)
        {
            Board<Data> board = new Board<Data>(row_count);

            int count = row_count * 2;

            foreach (var item in CreateDatas(count))
            {
                board.Add(item);
            }

            return board;
        }

        public static Data[] CreateDatas(int length)
        {
            Data[] data = new Data[length];

            for (int i = 0; i < length; i++)
            {
                int suit = UnityEngine.Random.Range(0, int.MaxValue);
                int rank = UnityEngine.Random.Range(0, int.MaxValue);

                data[i] = new Data(suit, rank);
            }

            return data;
        }

        public static Guid[] CreateGuids(int length)
        {
            Guid[] data = new Guid[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = Guid.NewGuid();
            }

            return data;
        }

        public static DecrementalIndexes CreateDecrementalIndexes(int count)
        {
            DecrementalIndexes indexes = new DecrementalIndexes(count);

            indexes.Fill();

            return indexes;
        }

        public static Dictionary<Guid, Data> CreateDictionary(int count)
        {
            var dictionary = new Dictionary<Guid, Data>(count);

            var guids = CreateGuids(count);
            var datas = CreateDatas(count);

            for (int i = 0; i < count; i++)
            {
                dictionary.Add(guids[i], datas[i]);
            }

            return dictionary;
        }

        public static Map<Guid, Data> CreateMap(int count)
        {
            var map = new Map<Guid, Data>(count);

            var guids = CreateGuids(count);
            var datas = CreateDatas(count);

            for (int i = 0; i < count; i++)
            {
                map.Add(guids[i], datas[i]);
            }

            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Return 3 players. attacker = 0, defender = 1, removed = 2</returns>
        public static IPlayer[] CreatePlayers()
        {
            IPlayer[] players = new Player[]
            {
                new Player()
                {
                    Name = "Attacker",
                    LookSide = CardLookSide.Back,
                    Position = PlayerPosition.Top,
                    Hand = new List<Data>(),
                    Type = PlayerType.Ai
                },
                new Player()
                {
                    Name = "Defender",
                    LookSide = CardLookSide.Face,
                    Position = PlayerPosition.Bottom,
                    Hand = new List<Data>(),
                    Type = PlayerType.Real
                },
                new Player()
                {
                    Name = "Removed",
                    LookSide = CardLookSide.Face,
                    Position = PlayerPosition.Bottom,
                    Hand = new List<Data>(),
                    Type = PlayerType.Real
                },
            };

            return players;
        }
        public static void CreatePlayerPack(out IPlayer[] players, out PlayerStorage storage, out PlayerQueue queue)
        {
            int attacker = 0;
            int defender = 1;
            int removed = 2;

            players = CreatePlayers();

            storage = new PlayerStorage(players);
            queue = new PlayerQueue(storage);

            storage.Remove(players[removed]);

            queue.Set(players[attacker], players[defender], PlayerActionType.Attack);
        }
    }
}