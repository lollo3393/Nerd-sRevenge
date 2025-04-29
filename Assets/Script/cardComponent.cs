using UnityEngine;

public class cardComponent : MonoBehaviour
{
    
    [SerializeField] private string cardName;
    [SerializeField] private GameObject cardPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform sfondo = cardPrefab.transform.Find("sfondo");
        Transform corner = cardPrefab.transform.Find("corner");
        Transform outlayer = cardPrefab.transform.Find("outlayer");
        Transform back = cardPrefab.transform.Find("back");
        
        
        Object[] layers = Resources.LoadAll<Sprite>("card/"+cardName) as Sprite[];
        sfondo.GetComponent<SpriteRenderer>().sprite = (Sprite)layers[0];
        corner.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blankCorner") as Sprite;
        outlayer.GetComponent<SpriteRenderer>().sprite = (Sprite)layers[1];
        back.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cardBack") as Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
