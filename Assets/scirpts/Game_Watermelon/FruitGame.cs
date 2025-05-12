using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public GameObject[] fruitPrefabs;

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };

    public GameObject currentFruit;
    public int currentFruitTpye;

    public float fruitStartHeidht = 6f;

    public float gameWidth = 5f;

    public bool isGameOver = false;

    public Camera mainCamera;

    public float fruitTimer;

    public float gameHeight;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        SpawnNewFruit();
        fruitTimer = -3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;

        if (fruitTimer >= 0)
            fruitTimer -= Time.deltaTime;
        
        if(fruitTimer < 0 & fruitTimer > -2)
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;
        }

        if(currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPdosition = currentFruit.transform.position;
            newPdosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitTpye] / 2;
            if(newPdosition.x <-gameWidth / 2 + halfFruitSize)
            {
                newPdosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if(newPdosition.x > gameWidth / 2 - halfFruitSize)
            {
                newPdosition.x = gameWidth / 2 - halfFruitSize;
            }
            currentFruit.transform.position = newPdosition;
        }

        if(Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }


    }

    public void MergeFruits(int fruitType, Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length-1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);

            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);
        }
    }

    



    void SpawnNewFruit()
    {
        if(!isGameOver)
        {
            currentFruitTpye = Random.Range(0, 3);

            Vector3 mousePostion = Input.mousePosition;
            Vector3 worldPostion = mainCamera.ScreenToWorldPoint(mousePostion);

            Vector3 spawnPosition =new Vector3(worldPostion.x, fruitStartHeidht, 0);

            float halfFruitSize = fruitSizes[currentFruitTpye] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitTpye], spawnPosition, Quaternion.identity);

            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitTpye], fruitSizes[currentFruitTpye], 1f);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.gravityScale = 0f;
            }
        }
    }


    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent< Rigidbody2D > ();
        if(rb !=null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;

            fruitTimer = 1.0f;
        }
    }


    void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();

        float gameOverHeight = gameHeight - 2f;

        for(int i = 0; i < allFruits.Length; i++)
        {
            Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

            if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight);
            {
                isGameOver = true;
                Debug.Log("게임오버");

                break;
            }
        }
    }
}
