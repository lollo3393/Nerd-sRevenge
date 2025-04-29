using System.IO;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    private CharacterController _charController;
   


  
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);//serve a store le informazioni che premiamo con la keyboard 
        movement= Vector3.ClampMagnitude(movement, speed);// in molti giochi se premiamo W e D allo stesso tempo la velocità del nostro giocatore dovrebbe incrementale e dovrebbe essere più veloce
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);


      //  transform.Translate(deltaX*Time.deltaTime,0,deltaZ*Time.deltaTime);
   
        
    }
}
