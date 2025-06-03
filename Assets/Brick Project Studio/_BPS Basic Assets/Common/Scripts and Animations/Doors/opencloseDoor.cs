using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor : MonoBehaviour
	{

		public Animator openandclose;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
		}

        void OnMouseOver()
        {
            if (!Player) return;

            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist > 5f) return; 

            if (!open && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(opening());
            }
            else if (open && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(closing());
            }
        }

        IEnumerator opening()
		{
			print("Porta Aperta");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("Porta chiusa");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}