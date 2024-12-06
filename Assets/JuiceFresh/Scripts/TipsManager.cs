using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsManager : MonoBehaviour
{
    public static TipsManager THIS;
    public bool gotTip;
    public bool allowShowTip;
    int tipID;
    public int corCount;
    private List<Item> nextMoveItems;


    // Use this for initialization
    void Start()
    {
        THIS = this;
        //    StartCoroutine(CheckPossibleCombines());
        //    StartCoroutine(reset());
    }

    IEnumerator reset()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            LevelManager.THIS.ReGenLevel();
        }
    }

    Square GetSquare(int row, int col)
    {
        return LevelManager.THIS.GetSquare(col, row);
    }

    void CheckSquare(Square square, int COLOR, bool moveThis = false)
    {
        if (!square)
            return;
        if (square.item != null)
        {
            if (square.item.color == COLOR)
            {
                if (moveThis && square.type != SquareTypes.WIREBLOCK)
                {
                    nextMoveItems.Add(square.item);
                }
                else if (!moveThis)
                    nextMoveItems.Add(square.item);
            }
        }

    }

    public List<Item> GetCombine()
    {
        return nextMoveItems;
    }

    bool checkCombinesStarted;
    public IEnumerator CheckPossibleCombines()  //1.3
    {
        // if (checkCombinesStarted) yield break;
        checkCombinesStarted = true;
        // print("check tip");

        yield return new WaitForSeconds(1);

        allowShowTip = true;
        int maxRow = LevelManager.THIS.maxRows;
        int maxCol = LevelManager.THIS.maxCols;
        //while (true)
        //{
        while (LevelManager.THIS == null)
        {
            yield return new WaitForEndOfFrame();
        }
        while (LevelManager.THIS.gameStatus != GameState.Playing)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!LevelManager.THIS.DragBlocked && LevelManager.THIS.gameStatus == GameState.Playing)
        {
            nextMoveItems = new List<Item>();

            if (LevelManager.THIS.gameStatus != GameState.Playing) yield break;
            gotTip = false;
            int countColors = 0;

            Item it = GameObject.FindGameObjectWithTag("Item").GetComponent<Item>();
            int iterations = 0;//1.4.5
            for (int COLOR = 0; COLOR < it.items.Length; COLOR++)
            {
                for (int col = 0; col < LevelManager.THIS.maxCols; col++)
                {
                    for (int row = 0; row < LevelManager.THIS.maxRows; row++)
                    {
                        countColors = 0;
                        countColors = GetColors(COLOR, col, row, countColors);
                        iterations++;//1.4.7
                        Square square = LevelManager.THIS.GetSquare(col, row);
                        if (square.type == SquareTypes.WIREBLOCK || square.item == null)
                            continue;
                        //current square called x
                        //o-o-x
                        //	  o
                        if (col > 1 && row < maxRow - 1)
                        {
                            CheckSquare(GetSquare(row + 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col - 1), COLOR);
                            CheckSquare(GetSquare(row, col - 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //    o
                        //o-o x
                        if (col > 1 && row > 0)
                        {
                            CheckSquare(GetSquare(row - 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col - 1), COLOR);
                            CheckSquare(GetSquare(row, col - 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.down));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //x o o
                        //o
                        if (col < maxCol - 2 && row < maxRow - 1)
                        {
                            CheckSquare(GetSquare(row + 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col + 1), COLOR);
                            CheckSquare(GetSquare(row, col + 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        //x o o
                        if (col < maxCol - 2 && row > 0)
                        {
                            CheckSquare(GetSquare(row - 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col + 1), COLOR);
                            CheckSquare(GetSquare(row, col + 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.down));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        //o
                        //x o
                        if (col < maxCol - 1 && row > 1)
                        {
                            CheckSquare(GetSquare(row, col + 1), COLOR, true);
                            CheckSquare(GetSquare(row - 1, col), COLOR);
                            CheckSquare(GetSquare(row - 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.left));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //x o
                        //o
                        //o
                        if (col < maxCol - 1 && row < maxRow - 2)
                        {
                            CheckSquare(GetSquare(row, col + 1), COLOR, true);
                            CheckSquare(GetSquare(row + 1, col), COLOR);
                            CheckSquare(GetSquare(row + 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.left));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //	o
                        //  o
                        //o x
                        if (col > 0 && row > 1)
                        {
                            CheckSquare(GetSquare(row, col - 1), COLOR, true);
                            CheckSquare(GetSquare(row - 1, col), COLOR);
                            CheckSquare(GetSquare(row - 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.right));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o x
                        //  o
                        //  o
                        if (col > 0 && row < maxRow - 2)
                        {
                            CheckSquare(GetSquare(row, col - 1), COLOR, true);
                            CheckSquare(GetSquare(row + 1, col), COLOR);
                            CheckSquare(GetSquare(row + 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.right));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o-o-o
                        if (col <= maxCol - 2 && col >= 0)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row, col + 1), COLOR);
                            CheckSquare(GetSquare(row, col + 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //   StartCoroutine(showTip(nextMoveItems[0], Vector3.right));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();
                        //o-o-o
                        if (col <= maxCol - 1 && col >= 2)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row, col - 1), COLOR);
                            CheckSquare(GetSquare(row, col - 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //   StartCoroutine(showTip(nextMoveItems[0], Vector3.left));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();
                        //o
                        //o
                        //o
                        if (row <= maxRow - 2 && row >= 0)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row + 1, col), COLOR);
                            CheckSquare(GetSquare(row + 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.down));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        //o
                        //o
                        if (row <= maxRow - 1 && row >= 2)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row - 1, col), COLOR);
                            CheckSquare(GetSquare(row - 2, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //   StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //current square called x
                        //o-o
                        //	o
                        if (col > 0 && row < maxRow - 1)
                        {
                            CheckSquare(GetSquare(row + 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row, col - 1), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //  o
                        //o-o
                        if (col > 1 && row > 0)
                        {
                            CheckSquare(GetSquare(row - 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row, col - 1), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.down));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o o
                        //o
                        if (col < maxCol - 2 && row < maxRow - 1)
                        {
                            CheckSquare(GetSquare(row + 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row, col + 1), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        //o o
                        if (col < maxCol - 2 && row > 0)
                        {
                            CheckSquare(GetSquare(row - 1, col), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row, col + 1), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.down));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        //o o
                        //
                        if (col < maxCol - 1 && row > 1)
                        {
                            CheckSquare(GetSquare(row, col + 1), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row - 1, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            // StartCoroutine(showTip(nextMoveItems[0], Vector3.left));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //
                        //o o
                        //o
                        if (col < maxCol - 1 && row < maxRow - 2)
                        {
                            CheckSquare(GetSquare(row, col + 1), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row + 1, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.left));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //	o
                        //o o
                        //
                        if (col > 0 && row > 1)
                        {
                            CheckSquare(GetSquare(row, col - 1), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row - 1, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.right));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //
                        //o o
                        //  o
                        if (col > 0 && row < maxRow - 2)
                        {
                            CheckSquare(GetSquare(row, col - 1), COLOR, true);
                            CheckSquare(GetSquare(row, col), COLOR);
                            CheckSquare(GetSquare(row + 1, col), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //  StartCoroutine(showTip(nextMoveItems[0], Vector3.right));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();


                        // o
                        //  o
                        //   o
                        if (col < maxCol - 2 && row < maxRow - 2)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row + 1, col + 1), COLOR);
                            CheckSquare(GetSquare(row + 2, col + 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //   StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //       o
                        //      o
                        //     o
                        if (row >= 2 && col < maxCol - 2)
                        {
                            CheckSquare(GetSquare(row, col), COLOR, true);
                            CheckSquare(GetSquare(row - 1, col + 1), COLOR);
                            CheckSquare(GetSquare(row - 2, col + 2), COLOR);
                        }
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            //   StartCoroutine(showTip(nextMoveItems[0], Vector3.up));
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();



                        //1.4.7
                        //o x o
                        //x o x
                        //o x o

                        //o o
                        // o
                        CheckSquare(GetSquare(row, col), COLOR, true);
                        CheckSquare(GetSquare(row - 1, col - 1), COLOR);
                        CheckSquare(GetSquare(row - 1, col + 1), COLOR);
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();
                        // o
                        //o o
                        CheckSquare(GetSquare(row, col), COLOR, true);
                        CheckSquare(GetSquare(row + 1, col - 1), COLOR);
                        CheckSquare(GetSquare(row + 1, col + 1), COLOR);
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        // o
                        //o 
                        // o
                        CheckSquare(GetSquare(row, col), COLOR, true);
                        CheckSquare(GetSquare(row - 1, col + 1), COLOR);
                        CheckSquare(GetSquare(row + 1, col + 1), COLOR);
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();

                        //o
                        // o 
                        //o
                        CheckSquare(GetSquare(row, col), COLOR, true);
                        CheckSquare(GetSquare(row - 1, col - 1), COLOR);
                        CheckSquare(GetSquare(row + 1, col - 1), COLOR);
                        if (nextMoveItems.Count == 3 && GetSquare(row, col).CanGoInto())
                        {
                            showTip(nextMoveItems);
                            yield break;
                        }
                        else
                            nextMoveItems.Clear();
                    }
                }


            }
            if (!LevelManager.THIS.DragBlocked && LevelManager.THIS.gameStatus == GameState.Playing && iterations > 0 && countColors < 3)
            {

                if (!gotTip)
                {
                    Debug.Log("no matches");
                    checkCombinesStarted = false;
                    LevelManager.THIS.NoMatches();
                    StopCoroutine(CheckPossibleCombines());
                }
            }

        }
        yield return new WaitForEndOfFrame();
        if (!LevelManager.THIS.DragBlocked)
            StartCoroutine(CheckPossibleCombines());

        // }
    }

    private int GetColors(int COLOR, int col, int row, int countColors)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var sq = GetSquare(col + i, row + j);
                if (sq != null)
                {
                    if (sq.item != null)
                    {
                        if (sq.item.color == COLOR)
                        {
                            if (sq.type != SquareTypes.WIREBLOCK)
                            {
                                countColors++;
                            }
                        }
                    }
                }
            }
        }

        return countColors;
    }

    void showTip(List<Item> nextMoveItems)
    {
        // print("show tip");
        StopCoroutine(showTipCor(nextMoveItems));
        StartCoroutine(showTipCor(nextMoveItems));
    }

    IEnumerator showTipCor(List<Item> nextMoveItems)
    {
        yield return new WaitForSeconds(1);

        gotTip = true;
        corCount++;
        if (corCount > 1)
        {
            corCount--;
            yield break;
        }
        if (LevelManager.THIS.DragBlocked && !allowShowTip)
        {
            corCount--;
            yield break;
        }
        tipID = LevelManager.THIS.moveID;
        //while (!LevelManager.THIS.DragBlocked && allowShowTip)
        //{
        yield return new WaitForSeconds(1);
        if (LevelManager.THIS.DragBlocked && !allowShowTip && tipID != LevelManager.THIS.moveID)
        {
            corCount--;
            yield break;
        }
        foreach (Item item in nextMoveItems)
        {
            if (item == null)
            {
                corCount--;
                yield break;
            }

        }
        foreach (Item item in nextMoveItems)
        {
            if (item != null)
                item.anim.SetTrigger("tip");
        }
        yield return new WaitForSeconds(0);
        StartCoroutine(CheckPossibleCombines());
        corCount--;
        checkCombinesStarted = false;

        // }
    }


}
