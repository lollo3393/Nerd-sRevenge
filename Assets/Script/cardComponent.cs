using Script;
using UnityEngine;

public class cardComponent : MonoBehaviour
{
    
    [SerializeField] cardDatabase  cardName;
    [SerializeField] private cardRarity rarita;
    
    private Transform sfondo ;
    private Transform corner ;
    private Transform outlayer ;
    private Transform back ;
    void Start()
    {
         sfondo = gameObject.transform.Find("sfondo");
         corner = gameObject.transform.Find("corner");
         outlayer = gameObject.transform.Find("outlayer");
         back = gameObject.transform.Find("back");
        
        
        Object[] layers = Resources.LoadAll<Sprite>("card/"+cardName) as Sprite[];
        sfondo.GetComponent<SpriteRenderer>().sprite = (Sprite)layers[0];
        corner.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blankCorner") as Sprite;
        outlayer.GetComponent<SpriteRenderer>().sprite = (Sprite)layers[1];
        back.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cardBack") as Sprite;
        controllaOlografia();
    }

    void controllaOlografia()
    {
        Shader holoSfondo;
        Shader holoOutlayer;
        Material sfondoMaterial ;
        Material outlayerMaterial ;
        
       
        switch (rarita)
        {
            case cardRarity.comune:
                corner.gameObject.GetComponent<SpriteRenderer>().color = Color.sandyBrown;
                break;
            case cardRarity.rara:
                corner.gameObject.GetComponent<SpriteRenderer>().color = Color.softYellow;
                holoSfondo = Resources.Load<Shader>("Shader/sfondoHolo");
                holoOutlayer = Resources.Load<Shader>("Shader/rareHolo");
                sfondoMaterial = new Material(holoSfondo);
                outlayerMaterial = new Material(holoOutlayer);
                sfondo.GetComponent<SpriteRenderer>().material = sfondoMaterial;
                outlayer.GetComponent<SpriteRenderer>().material = outlayerMaterial;
                break;
            case cardRarity.epica:
                corner.gameObject.GetComponent<SpriteRenderer>().color = Color.mediumOrchid;
                holoSfondo = Resources.Load<Shader>("Shader/sfondoHolo");
                holoOutlayer = Resources.Load<Shader>("Shader/epicHolo");
                sfondoMaterial = new Material(holoSfondo);
                outlayerMaterial = new Material(holoOutlayer);
                sfondo.GetComponent<SpriteRenderer>().material = sfondoMaterial;
                outlayer.GetComponent<SpriteRenderer>().material = outlayerMaterial;
                break;
            case cardRarity.Legendaria:
                corner.gameObject.GetComponent<SpriteRenderer>().color = Color.crimson;
                holoSfondo = Resources.Load<Shader>("Shader/sfondoHolo");
                holoOutlayer = Resources.Load<Shader>("Shader/epicHolo");
                sfondoMaterial = new Material(holoSfondo);
                outlayerMaterial = new Material(holoOutlayer);
                sfondo.GetComponent<SpriteRenderer>().material = sfondoMaterial;
                outlayer.GetComponent<SpriteRenderer>().material = outlayerMaterial;
                break;
            default:
                break;
        }
    }
}
