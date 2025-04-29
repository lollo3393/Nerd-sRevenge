using UnityEngine;

public class MauseLook : MonoBehaviour

{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;
    public float miniumVert = -45.0f;
    public float maximumVert = 45.0f;
    private float _rotationX = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)

            body.freezeRotation = true; //queste righe di codice servono a verificare che l'oggetto a cui lo script è attaccato sia un corpo rigido, 
                                        // se esiste un corpo rigido attaccato a qeusto oggetto, posso attaccare la freeze rotation quindi la fisica non affligge la rotazione di qeusto codice. 
                                        //noi non lo dobbiamo usare perchè non abbiamo un player con un corpo rigido
    }

        // Update is called once per frame
        void Update()
        {
            if (axes == RotationAxes.MouseX)
            {
                //Rotazione orizzontale qui
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0); // ruotiamo sulle Y per via della mano sinistra come avevamo visto tuttavia così non dipende dal mouse
            }
            else if (axes == RotationAxes.MouseY)
            {
                //rotazione verticale qui, quando ruotiamo le assi y non possiamo completare tutta la rotazione perchè la testa è blocata
                // con un massimo e un minimo e defniiamo il metodo mathf.clamp()
                _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, miniumVert, maximumVert);
                float rotationY = transform.localEulerAngles.y;//importanti perchè in questo pezzo di codice ci prendiamo cura solo della rotazione Y quindi quella che avviene nelle x, quindi ogni volta che entriamo qui 
                                                               //collezioniamo i dati, della Y e li curtiamo in un nuovo vettore 3 dove abbiamo la rotationX calcolata e la rotationY che abbiamo già salvato
                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);// perchè non possiamo cambiare la rotazione Y ma solo la X altrimenti sarebbe un problema nel salvare tutto.

            }
            else
            {
                //entrambe le rotazion iqui 
                _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, miniumVert, maximumVert);
                float delta = Input.GetAxis("Mouse X") * sensitivityHor;
                float rotationY = transform.localEulerAngles.y + delta;
                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);//vogliamo che la x ruoti il giocatore, e la Y invece muova la testa
                                                                                   //la rotazione Y deve muovere gli occhi, la X invece il resto del corpo. perchè altrimenti tiltiamo il giocatore, apparentemente sembrerebbe funzionare tuttavia non è il risultato sperato, perchè quando guardo in alto vogli evitare di tiltare il giocatore.

            }

        }
    }

