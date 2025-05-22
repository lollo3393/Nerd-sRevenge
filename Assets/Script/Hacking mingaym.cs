using UnityEngine;

public class Hackingmingaym : MonoBehaviour
{
    private string funzione;
    private int nVar;
    
    private void Start()
    {
        nVar = 3;
        funzione = generaFunzione();
        Debug.Log(funzione);

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
