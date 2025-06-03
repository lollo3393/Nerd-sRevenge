using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class CardMuseum : CardGenerator
    {
        List<string> cardNames = new List<string>();   
        private void Start()
        {
            foreach (string cardName in Enum.GetNames(typeof(cardDatabase)))
            {
                cardNames.Add(cardName);
            }
        }

        public override (cardDatabase, cardRarity) randomCard()
        {
            bool flag = true;
            cardRarity rarity = cardRarity.Legendaria;
            while (cardNames.Count > 0)
            {
                cardDatabase name = GetRandomEnumValue<cardDatabase>();
                if (cardNames.Contains(name.ToString()))
                {
                    cardNames.Remove(name.ToString());
                    return (name, rarity);
                }
                
            }
            
            return (cardDatabase.jelfo, rarity);
        }
    }
}