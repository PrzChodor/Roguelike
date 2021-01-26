using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject balls;
    public int numberOfBalls;
    public float speed;

    public void Spawn()
    {
        var dir = Vector2.up;
        var angle = 360.0f / numberOfBalls;

        for (int i = 0; i < numberOfBalls; i++)
        {
            var ball = GameObject.Instantiate(balls, transform.position, Quaternion.identity);
            ball.transform.parent = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().currentLevel.transform;
            ball.GetComponent<Rigidbody2D>().AddForce(dir.Rotate(i * angle) * speed);
        }
    }
}
