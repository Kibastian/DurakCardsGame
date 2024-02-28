using System;
using System.Collections.Generic;

using Framework.Durak.Players;
using Framework.Shared.Cards.Entities;
using Framework.Shared.Cards.Views;
using Framework.Shared.Collections;

using UnityEngine;

namespace Framework.Durak.Game.EntityCreators
{
    [Serializable]
    internal class PlayerListCreator
    {
        [SerializeField] private Info top;
        [SerializeField] private Info bottom;
        [SerializeField] private Info left;

        public (IReadOnlyList<IPlayer> players, IMap<Place, ICardOwner> map) Create()
        {
            var infos = new Info[] { top, bottom, left };

            int count = infos.Length;

            var players = new Player[count];
            var map = new Map<Place, ICardOwner>(count);

            for (int i = 0; i < count; i++)
            {
                ref readonly Info info = ref infos[i];

                Player player = new()
                {
                    Id = Guid.NewGuid(),
                    Name = info.name,
                    Type = info.type,

                    Place = info.place,

                    Hand = new Hand(info.lookSide),
                    tree = new Players.Tree()
                };

                map.Add(player.Place, new CardOwner(info.transform));

                players[i] = player;
            }

            return (players, map);
        }

        public IMap<Place, ICardOwner> CreateMap()
        {
            var infos = new Info[] { top, bottom, left };

            var map = new Map<Place, ICardOwner>(infos.Length);

            foreach (var item in infos)
            {
                map.Add(item.place, new CardOwner(item.transform));
            }

            return map;
        }

        [Serializable]
        private struct Info
        {
            public string name;
            public PlayerType type;
            public CardLookSide lookSide;
            public Place place;
            public Transform transform;
        }
    }
}