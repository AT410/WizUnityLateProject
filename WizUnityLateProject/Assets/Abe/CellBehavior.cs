using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct Cell
{
    public int X,Y;

    public Cell(int x,int y)
    {
        X = x;
        Y = y;
    }
}

public enum ObjectType
{
    Default,
    Wall
}

public class CellBehavior : MonoBehaviour
{
    [SerializeField]
    private bool IsSelected;

    [SerializeField]
    private uint MoveCount;

    [SerializeField]
    private Cell index;

    private float WaveTime;

    private SpriteRenderer Srender;

    private Color DefaultColor;

    private bool IsDenger;

    private uint DengerCase;

    public void SetIndex(int x,int y)
    {
        index = new Cell(x, y);
    }

    public void SetSelected(bool Active)
    {
        IsSelected = Active;
    }

    public uint GetMoveCount() { return MoveCount; }

    // Start is called before the first frame update
    void Start()
    {
        Srender = GetComponent<SpriteRenderer>();

        DefaultColor = Srender.color;

        WaveTime = 0.0f;

        MoveCount = 3;
        IsDenger = false;

        DengerCase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelected)
        {
            SelectBehavior();
        }
        else
        {
            if (!IsDenger)
            {
                WaveTime = 0.0f;
                Srender.color = DefaultColor;
            }
            else
            {
                if (DengerCase == 1)
                {
                    Srender.color = Color.red;
                }
                else if(DengerCase ==2)
                {
                    Srender.color = Color.blue;
                }
                else if(DengerCase >=3)
                {
                    Srender.color = Color.black;
                }
            }
        }
    }

    private void SelectBehavior()
    {
        Srender.color = new Color(1,1,1,Mathf.Abs(Mathf.Sin(WaveTime)));
        WaveTime += Time.deltaTime*2.0f;
    }

    public void SetDenger()
    {
        IsDenger = true;
        DengerCase++;
    }
}
