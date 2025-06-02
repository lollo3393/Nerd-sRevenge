using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class CardGenerator : MonoBehaviour
    {
        // oggetto da cui si può manipolare lo spawn delle carte
        public (cardDatabase,cardRarity) randomCard()
        {
            
            cardDatabase cardname = GetRandomEnumValue<cardDatabase>() ;
            cardRarity rarity = GetRandomEnumValue<cardRarity>();
            return (cardname,rarity);
        }
        public T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(index);
        }
        
    }
}