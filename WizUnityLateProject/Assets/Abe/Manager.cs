using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager instance;

    private void Awake()
    {
        instance = this;
        
    }

    public static Manager GetInstance() { return instance; }

    [SerializeField]
    private CellBehavior Prefub;

    [SerializeField]
    private uint CellSize;


    [SerializeField]
    private bool StartSarch;

    [SerializeField]
    private bool StartAstar;

    [SerializeField]
    private Cell StartCell, GoalCell;

    [SerializeField]
    private List<Cell> SelectCell;

    private Dictionary<Cell, CellBehavior> pairs;

    private Dictionary<Cell, CellStar> starpairs;

    // Start is called before the first frame update
    void Start()
    {
        pairs = new Dictionary<Cell, CellBehavior>();
        starpairs = new Dictionary<Cell, CellStar>();
        for (int x = 0; x < CellSize; x++)
        {
            for (int y = 0; y < CellSize; y++)
            {
                var obj = Instantiate(Prefub, new Vector3(x,-y,0),Quaternion.identity);
                obj.SetIndex(x, y);
                pairs.Add(new Cell(x, y), obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StartSarch)
        {
            MovetoBehavior();

            StartSarch = false;
        }
    }

    private void MovetoBehavior()
    {
        List<List<Cell>> MoveCells = new List<List<Cell>>();
        foreach (var select in SelectCell)
        {
            pairs[select].SetSelected(true);



            MoveCells.Add(MoveCellEnumeration(pairs[select].GetMoveCount(), select));
        }

        foreach (var celllist in MoveCells)
        {
            foreach (var cell in celllist)
            {
                pairs[cell].SetDenger();
            }
        }
    }

    private List<Cell> MoveCellEnumeration(uint MoveCount,Cell target)
    {
        List<Cell> result = new List<Cell>();

        for(int x =0;x<= MoveCount;x++)
        {
            for (int y = 0; y <= MoveCount;y++)
            {
                if (x == 0 && y == 0)
                    continue;

                if (x + y > MoveCount)
                    continue;

                int Cellx = target.X + x;
                int Celly = target.Y + y;

                int Underx = target.X - x;
                int Undery = target.Y - y;

                if(Cellx<CellSize&&Celly<CellSize)
                {
                    Cell cell = new Cell(Cellx, Celly);

                    if(!result.Contains(cell))
                        result.Add(cell);
                }

                if(Underx>=0&&Undery>=0)
                {
                    Cell Under = new Cell(Underx, Undery);
                    if (!result.Contains(Under))
                        result.Add(Under);
                }

                if(Cellx<CellSize&&Undery>=0)
                {
                    Cell Right = new Cell(Cellx, Undery);
                    if (!result.Contains(Right))
                        result.Add(Right);
                }

                if(Underx>=0&&Celly<CellSize)
                {
                    Cell Left = new Cell(Underx, Celly);
                    if (!result.Contains(Left))
                        result.Add(Left);
                }

            }
        }

        
        return result;
    }
}
