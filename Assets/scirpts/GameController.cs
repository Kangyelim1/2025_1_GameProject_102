using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public float Timer = 1.0f;
    public GameObject EnemyObjet;


    // Update is called once per frame
    void Update()
    {
        
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = 1;

                GameObject Temp = Instantiate(EnemyObjet);
            Temp.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        //Debug.Log($"hit : {hit.collider.name}");
                        hit.collider.gameObject.GetComponent<Enemy>().CharcterHit(30);
                    }
                }
            }
      
    }
}
