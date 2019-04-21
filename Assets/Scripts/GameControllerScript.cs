using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    private int direction;
    private Vector3 nextPosition;
    private int maxSize;
    private int currentSize;
    private GameObject foodObject;

    public Material snakeHead;
    public Material snakeBody;
    public GameObject snakePrefab;
    public SnakeScript head;
    public SnakeScript tail;
    public GameObject foodPrefab;

    public float snakeSpeed;
    public bool isGameEnd;
    public int score;
    public Text scoreText;
    public Text gameEndText;
    public Button backToMenuButton;
    public Button restartButton;


    void Start()
    {
        snakeSpeed = 0.5f;
        maxSize = 2;
        currentSize = 0;
        score = 0;
        isGameEnd = false;

        scoreText.text = "Score : " + score.ToString();
        gameEndText.text = "";
        backToMenuButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        InvokeRepeating("InvokeFunction", 0, snakeSpeed);
        FoodFunction();
    }

    void Update()
    {
        scoreText.text = "Score : " + score.ToString();
        switch (head.GetHitStatus())
        {
            case "Food":
                maxSize++;
                score++;
                if (score % 15 == 0 && snakeSpeed>=0.23f)
                {
                    snakeSpeed -= 0.03f;
                    CancelInvoke("InvokeFunction");
                    InvokeRepeating("InvokeFunction", 0, snakeSpeed);
                }
                FoodFunction();
                break;

            case "Wall":
                isGameEnd = true;
                EndGame();
                break;

            case "Snake":
                print("snake");
                isGameEnd = true;
                EndGame();
                break;
        }
        ChangeDirection();
    }

    void InvokeFunction()
    {
        if (isGameEnd == false)
        {
            Movement();
            if (currentSize >= maxSize)
            {
                TailController();
            }
            else
            {
                currentSize++;
            }
        }
    }

    void Movement()
    {
        GameObject temp;
        nextPosition = head.transform.position;
        switch (direction)
        {
            case 0:
                nextPosition.x -= 1;
                break;

            case 1:
                nextPosition.z += 1;
                break;

            case 2:
                nextPosition.x += 1;
                break;

            case 3:
                nextPosition.z -= 1;
                break;
        }

        temp = Instantiate<GameObject>(snakePrefab, nextPosition, transform.rotation);
        temp.GetComponent<MeshRenderer>().material = snakeHead;

        head.SetNext(temp.GetComponent<SnakeScript>());
        head.GetComponent<MeshRenderer>().material = snakeBody;

        head = temp.GetComponent<SnakeScript>();
    }

    void ChangeDirection()
    {
        if (direction != 2 && Input.GetKeyDown(KeyCode.W))
        {
            direction = 0;
        }
        if (direction != 3 && Input.GetKeyDown(KeyCode.D))
        {
            direction = 1;
        }
        if (direction != 0 && Input.GetKeyDown(KeyCode.S))
        {
            direction = 2;
        }
        if (direction != 1 && Input.GetKeyDown(KeyCode.A))
        {
            direction = 3;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            score += 100;
        }
    }

    void TailController()
    {
        SnakeScript temp = tail;
        tail = temp.GetNext();
        temp.Remove();
    }

    void FoodFunction()
    {
        int vertical = Random.Range(0, -19);
        int horizontal = Random.Range(0, 19);

        Vector3 position = new Vector3(vertical, 0.5f, horizontal);
        foodObject = Instantiate<GameObject>(foodPrefab, position, transform.rotation);
    }

    void EndGame()
    {
        CancelInvoke("InvokeFunction");
        int temp = PlayerPrefs.GetInt("HighScore");
        if(score > temp)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        backToMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameEndText.text = "Your score : " + score.ToString();
    }

    IEnumerator CheckFood(GameObject obj)
    {
        yield return new WaitForEndOfFrame();

        if(obj.GetComponent<Renderer>().isVisible == false && obj.CompareTag("Food"))
        {
            Destroy(obj.gameObject);
            FoodFunction();           
        }
    }
}