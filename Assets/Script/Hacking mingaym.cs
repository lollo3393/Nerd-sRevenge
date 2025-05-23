using Script;
using TMPro;
using UnityEngine;

public enum difficolta
{
    facile,
    medio, 
    difficile
}
public class Hackingmingaym : MonoBehaviour
{
    private string funzione;
    private int nVar;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private difficolta diff;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private GameObject switchZonePrefab;
    
    private void Start()
    {
        switch (diff)
        {
            case difficolta.facile:
                nVar = 2;
                break;
            case difficolta.medio:
                nVar = 3;
                break;
            case difficolta.difficile:
                nVar = 4;
                break;
            
        }
        funzione = generaFunzione();
        tmp.text = "f = "+funzione;
        Debug.Log(funzione);

    }

    private void generaZoneDraggabili()
    {
        char nomeVar = 'a';
        foreach (GameObject g in gridLayout.transform)
        {
            GameObject.Destroy(g);
        }
        for (int i = 0; i < nVar; i++)
        {
            GameObject dragZone = Instantiate(switchZonePrefab, gridLayout.transform);
            DraggableSwitchZone dsz=   dragZone.GetComponent<DraggableSwitchZone>();
            dsz.varName = nomeVar.ToString();
            nomeVar++;
        }
    }

    private string generaFunzione()
    {
        string function="";
        char var = 'a';
        for (int i = 0; i < nVar; i++)
        {
            if (Random.value < 0.5f)
            {
                function = function + var ;
            }
            else
            {
                function = function +"not("+var+")";
            }

            if (i == nVar-1)
            {
                return function;
            }
            
            if (Random.value < 0.5f)
            {
                function = function + " + " ;
            }
            else
            {
                function = function + " * " ;
            }

            var++;
        }
        return function;
    }
    
    void Update()
    {
        
    }
}
