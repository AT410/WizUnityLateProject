using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStar : MonoBehaviour
{
    [SerializeField]
    private Cell index;

    // -- A-Star --
    private ObjectType type;

    public enum Status
    {
        None,
        Open,
        Colsed
    }

    public Status m_status;

    public int MCost = 0, HCost = 0;

    public int Score = 0;

    public CellBehavior parent;

    private SpriteRenderer Srender;

    private Color DefaultColor;

    public void SetType(ObjectType settype)
    {
        type = settype;
    }

    // Start is called before the first frame update
    void Start()
    {
        Srender = GetComponent<SpriteRenderer>();

        DefaultColor = Srender.color;

        type = ObjectType.Default;

        m_status = Status.None;

    }

    // Update is called once per frame
    void Update()
    {
        if (type == ObjectType.Wall)
        {
            Srender.color = Color.gray;
            return;
        }

    }
}
