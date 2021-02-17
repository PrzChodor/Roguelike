using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject balls;
    public int numberOfBalls;
    public float timeBetweenBalls;
    public float speed;

    public void Spawn()
    {
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        var dir = Vector2.up.Rotate(Random.Range(0f, 360f));
        var angle = 360.0f / numberOfBalls;

        if (timeBetweenBalls == 0f)
        {
            var ball = Instantiate(balls, transform.position, Quaternion.identity);
            ball.transform.parent = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().currentLevel.transform;
            ball.GetComponent<Rigidbody2D>().AddForce(dir * speed);
            ball.GetComponent<Ball>().SpawnedSound();

            for (int i = 1; i < numberOfBalls; i++)
            {
                ball = Instantiate(balls, transform.position, Quaternion.identity);
                ball.transform.parent = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().currentLevel.transform;
                ball.GetComponent<Rigidbody2D>().AddForce(dir.Rotate(i * angle) * speed);
            }
        }
        else
        {
            for (int i = 0; i < numberOfBalls; i++)
            {
                var ball = Instantiate(balls, transform.position, Quaternion.identity);
                ball.transform.parent = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().currentLevel.transform;
                ball.GetComponent<Rigidbody2D>().AddForce(dir.Rotate(i * angle) * speed);
                ball.GetComponent<Ball>().SpawnedSound();
                yield return new WaitForSeconds(timeBetweenBalls);
            }
        }
        Destroy(gameObject);
    }
}
