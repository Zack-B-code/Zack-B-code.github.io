using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSpace : GameSpace
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void MovePiece() { // Based on amount of players on space, move pieces to positions relative to space
        Vector3 newPos;
        switch (playersOnSpace.Count) {
            case 1:
                newPos = playersOnSpace[0].transform.localPosition;
                newPos.x = 0;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[0].GetComponent<Player>().Move(newPos);
                break;
            case 2:
                newPos = playersOnSpace[0].transform.localPosition;
                newPos.x = -0.25f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[0].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[1].transform.localPosition;
                newPos.x = 0.25f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[1].GetComponent<Player>().Move(newPos);
                break;
            case 3:
                newPos = playersOnSpace[0].transform.localPosition;
                newPos.x = -0.4f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[0].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[1].transform.localPosition;
                newPos.x = 0;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[1].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[2].transform.localPosition;
                newPos.x = 0.4f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[2].GetComponent<Player>().Move(newPos);
                break;
            case 4:
                newPos = playersOnSpace[0].transform.localPosition;
                newPos.x = -0.4f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[0].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[1].transform.localPosition;
                newPos.x = -0.2f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[1].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[2].transform.localPosition;
                newPos.x = 0.2f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[2].GetComponent<Player>().Move(newPos);

                newPos = playersOnSpace[3].transform.localPosition;
                newPos.x = 0.4f;
                newPos.z = 0;
                newPos = transform.TransformPoint(newPos);
                playersOnSpace[3].GetComponent<Player>().Move(newPos);
                break;
        }
    }
}
