using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float time = 2f;
    public string color;
    public AudioSource piecemoveAudioSource;

    [Header("Set Dynamically")]
    public Vector3 initialPosition;
    public List<Vector3> finalPosition = new List<Vector3>();
    public float tempTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tempTime > time)
        {
            piecemoveAudioSource.Play();
            tempTime = 0;
            finalPosition.RemoveAt(0);
            if (finalPosition.Count != 0)
            {
                initialPosition = transform.position;
            }
        }
        if (finalPosition.Count > 0)
        {
            transform.position = Vector3.Lerp(initialPosition, finalPosition[0], tempTime / time);
            tempTime += Time.deltaTime;
        }
    }

    public void Move(Vector3 newPos)
    { //Set up to smoothly move piece in Update
        initialPosition = transform.position;
        finalPosition.Add(newPos);
        tempTime = 0;
    }

    public Color32 GetPlayerColor32()
    {
        switch (color)
        {
            case "Red":
                return Color.red;
            case "Yellow":
                return Color.yellow;
            case "Green":
                return Color.green;
            case "Blue":
                return Color.blue;
            case "Cyan":
                return Color.cyan;
            case "Purple":
                return new Color32(104, 43, 193, 255);
            case "White":
                return Color.white;
            case "Grey":
                return Color.grey;
            case "Black":
                return Color.black;
        }
        return Color.white;
    }
}
