using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum difficolta
{
    facile,
    medio, 
    difficile
}
public class HackingMingame : MonoBehaviour
{
    private string funzione;
    private int nVar;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private difficolta diff;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private GameObject switchZonePrefab;
     public Dictionary<string, bool> varState = new Dictionary<string, bool>();
     [SerializeField] private GameObject inverter;
     public GameObject laser;
     public int errori;

     private GameObject centerOutWire;
     [SerializeField] GameObject finalWire;
     
     public bool fineMinigame = false;
   
    private void Start()
    {
        ResettaMinigame();
       
    }

    public void ResettaMinigame()
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
        generaZoneDraggabili();
    }

    private void generaZoneDraggabili()
    {
        char nomeVar = 'a';
        foreach (Transform g in gridLayout.transform)
        {
            Destroy(g.gameObject);
        }
        for (int i = 0; i < nVar; i++)
        {
            GameObject dragZone = Instantiate(switchZonePrefab, gridLayout.transform);
            DraggableSwitchZone dsz=   dragZone.GetComponent<DraggableSwitchZone>();
            dsz.cambiaNomeVar(nomeVar);
            nomeVar++;
        }
    }

    private string generaFunzione()
    {
        string function="";
        char var = 'a';
        varState.Add(var.ToString(),false);
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
            varState.Add(var.ToString(),false);
            
        }
        
        return function;
    }
    
    void Update()
    {
        
        
       
    }

    IEnumerator provaCircuito()
    {
        
        string[] tokens = funzione.Replace(" ","").Split('+', '*' );
        
        

        CenterManager cm = GetComponent<CenterManager>();
        if (cm.controllaCollegamento()) {
            foreach (string token in tokens)
            {
                if (token.Length == 1)
                {
                    varState[token] = true;
                }
            }
            
            foreach (string varname in varState.Keys.ToList())
            {
                Debug.Log(varname+" "+varState[varname]);
            }
            cm.coloraPDN();
            yield return new WaitForSeconds(1f);
            if (finalWire.GetComponent<Image>().color == Color.blue || (inverter.GetComponent<Inverter>().inverter == false && finalWire.GetComponent<Image>().color == Color.brown))
            {
                cm.resettaPDN();
                yield return new WaitForSeconds(1f);
                foreach (string token in tokens)
                {
                    if (token.Length == 1)
                    {
                        varState[token] = false;
                    }
                    else
                    {
                        string var = token.Replace("not(", "").TrimEnd(')');
                        varState[var] = true;
                    }
                }
                cm.coloraPUN();
                yield return new WaitForSeconds(1f);
                if (finalWire.GetComponent<Image>().color == Color.brown)
                {
                    //minigameVinto
                    fineMinigame = true;
                }
            }
        }else
        {
            errori++;
        }
        
        yield  return new WaitForEndOfFrame(); 
    }
    

    public void Test()
    {
        StartCoroutine(provaCircuito());
        
    }

    public void SuicideButton()
    {
        Destroy(transform.root.gameObject);
    }

    public void debugSkipButton()
    {
        fineMinigame = true;
    }
}
