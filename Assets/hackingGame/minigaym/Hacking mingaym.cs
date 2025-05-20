using UnityEngine;

public class Hackingmingaym : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject canvas;
    private void Start()
    {
        
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
