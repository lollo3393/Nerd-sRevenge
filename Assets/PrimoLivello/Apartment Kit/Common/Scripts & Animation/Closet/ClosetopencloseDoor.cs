using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class ClosetopencloseDoor : MonoBehaviour
	{

		public Animator Closetopenandclose;
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
			print("Porta Scorrevole aperta");
			Closetopenandclose.Play("ClosetOpening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("Porta Scorrevole chiusa");
			Closetopenandclose.Play("ClosetClosing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}