using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;

    [SerializeField]
    private float vitesseChasse = 1.0f, vitessePatrouille = 0.5f;
    private Vector3 mouvementDirection;
    private Vector3 visionDirection;

    [SerializeField]
    private GameObject cible;
    [SerializeField]
    private LayerMask masqueRayon;

    [SerializeField]
    private float distanceVue = 10.0f;

    private enum TArbre
    {
        eInactif = 1,
        ePatrouille = 2,
        eChasse = 4
    };
    private TArbre etat;

    // Permet de déterminé s'il est actuellement en mouvement
    private bool EstMobile()
    {
        return (etat & (TArbre.ePatrouille | TArbre.eChasse)) != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        mouvementDirection.z = 0.0f;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (cible == null)
        {
            cible = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        visionDirection = (cible.transform.position - transform.position).normalized;
        RaycastHit2D frappe = Physics2D.Raycast(transform.position, visionDirection, distanceVue, masqueRayon);

        float distance_destination = distanceVue;
        TArbre vielEtat = etat;
        etat = TArbre.ePatrouille;

        if (frappe.collider != null)
        {
            distance_destination = frappe.distance;
            if (frappe.collider.gameObject.layer == cible.layer)
            {
                etat = TArbre.eChasse;
                mouvementDirection = visionDirection;
            }
        }

        Vector3 depart = transform.position;
        Vector3 delta = visionDirection * distance_destination;
        Vector3 destination = depart + delta;
        Vector3 perpendiculaire = new Vector3(visionDirection.y, -visionDirection.x, visionDirection.z);
        Debug.DrawRay(transform.position, delta);
        const float largeurT = 0.5f;
        Debug.DrawRay(destination, perpendiculaire * largeurT);
        Debug.DrawRay(destination, -perpendiculaire * largeurT);

        if (vielEtat != etat)
        {
            if (etat == TArbre.ePatrouille)
            {
                StartCoroutine(CPatrouille());
            }
            else if (etat == TArbre.eChasse)
            {
                StopCoroutine(CPatrouille());
            }
        }
    }

    private void FixedUpdate()
    {
        if (EstMobile())
        {
            anim.SetFloat("moveX", mouvementDirection.x);
            anim.SetFloat("moveY", mouvementDirection.y);
            anim.SetFloat("Speed", mouvementDirection.sqrMagnitude);
            float vitesse = etat == TArbre.eChasse ? vitesseChasse : vitessePatrouille;
            RaycastHit2D frappe = Physics2D.Raycast(transform.position, mouvementDirection, distanceVue / 5, masqueRayon);
            if (frappe.collider != null && frappe.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                mouvementDirection = Vector3.zero;
            }
            rig.velocity = mouvementDirection * vitesse;
            if (mouvementDirection.sqrMagnitude > 0)
            {
                anim.SetFloat("lastMoveX", mouvementDirection.x);
                anim.SetFloat("lastMoveY", mouvementDirection.y);
            }
        }
        else
        {
            rig.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0.0f);
        }
    }

    IEnumerator CPatrouille()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            bool valid = false;
            int tentatives = 0;
            do
            {
                mouvementDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                mouvementDirection.Normalize();
                RaycastHit2D frappe = Physics2D.Raycast(transform.position, mouvementDirection, distanceVue / 2, masqueRayon);
                if (frappe.collider != null)
                {
                    tentatives++;
                    if (tentatives > 1000)
                    {
                        mouvementDirection = Vector3.zero;
                        valid = true;
                    }
                }
                else
                {
                    valid = true;
                }
            } while (!valid);
            yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
            mouvementDirection = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        }
    }

}
