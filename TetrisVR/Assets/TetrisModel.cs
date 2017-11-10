using UnityEngine;
using System.Collections;
using System;
using TetrisTools;


namespace TetrisCore
{

    public static class GameSettings
    {
        public static int sizex = 13;
        public static int sizey = 27;
    }


    public enum CellTypes
    {
        empty = 0,
        baseType = 1,
        borderBrick = 2,
    }


    public static class TetrisFigureFactory
    {
        public static TetrisFigure GetNormalFigure(FigureTypes figureType)
        {
            TetrisFigure obj = new TetrisFigure();

            //obj.gameFigure[1, 1].FillCell(CellTypes.baseType);

            switch (figureType)
            {
                case FigureTypes.one:
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            obj.gameFigure[i, j].FillCell(CellTypes.baseType); //b[i, j] := true;
                        }
                    }
                    break;
                case FigureTypes.two:
                    for (int j = 0; j < 4; j++)
                    {
                        obj.gameFigure[1, j].FillCell(CellTypes.baseType); //b[2, j] := true;
                    }
                    break;
                case FigureTypes.three:
                    obj.gameFigure[0, 0].FillCell(CellTypes.baseType); //b[1, 1] := true;
                    for (int i = 0; i < 3; i++)
                    {
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);  //b[i, 2] := true;
                    }
                    break;
                case FigureTypes.four:
                    obj.gameFigure[0, 2].FillCell(CellTypes.baseType); //b[1, 3] := true;
                    for (int i = 0; i < 3; i++)
                    {
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);//b[i, 2] := true;
                    }
                    break;
                case FigureTypes.five:
                    for (int i = 0; i < 2; i++)
                    {
                        obj.gameFigure[i + 1, 0].FillCell(CellTypes.baseType);//b[i + 1, 1] := true;
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);//b[i, 2] := true
                    }
                    break;
                case FigureTypes.six:
                    for (int i = 0; i < 2; i++)
                    {
                        obj.gameFigure[i+1, 1].FillCell(CellTypes.baseType);//b[i + 1, 2] := true;
                        obj.gameFigure[i, 0].FillCell(CellTypes.baseType);//b[i, 1] := true
                    }
                    break;
                case FigureTypes.seven:
                    for (int i = 0; i < 3; i++)
                    {
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);//b[i, 2] := true;
                    }
                    obj.gameFigure[1, 0].FillCell(CellTypes.baseType);//b[2, 1] := true;
                    break;
            }
            obj.curFigureType = figureType;
            return obj;
        }

        public static TetrisFigure RandomizeNormalFigure()
        {
            int nF = UnityEngine.Random.Range(1, 8);
            return TetrisFigureFactory.GetNormalFigure((FigureTypes)nF);
        }
    }

    public class TetrisCell 
    {
        public CellTypes CellType;

        public TetrisCell()
        {
            CellType = CellTypes.empty;
        }

        public void FillCell(CellTypes cellType)
        {
            CellType = cellType;
        }

        public void ClearCell()
        {
            CellType = CellTypes.empty;
        }

        public bool IsCellEmpty()
        {
            return CellType != CellTypes.baseType;
        }

        public void FillCellByCell(TetrisCell cell)
        {
            ClearCell();
            FillCell(cell.CellType);
        }
    }

    public class TetrisPlayerModel : MonoBehaviour
    {
        private static TetrisPlayerModel _instance;

        public int level;
        public int score;

        private TetrisPlayerModel() { }

        public static TetrisPlayerModel Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    GameObject gm = GameObject.Find("SceneRoot");
                    if (gm != null)
                    {
                        _instance = gm.AddComponent<TetrisPlayerModel>();
                        //_instance.InitPool();
                        return _instance;
                    }
                    else
                    {


                        _instance = new GameObject("SceneRoot").AddComponent<TetrisPlayerModel>();
                        //_instance.InitPool();
                        return _instance;
                    }
                }
            }
        }

    }


    public class TetrisField
    {
        public TetrisCell[,] gameField = new TetrisCell[GameSettings.sizex+5, GameSettings.sizey+5];
        public TetrisFigure curFigure;
        public TetrisFigure nextFigure;
        public TetrisPlayerModel playerModel;

        public TetrisField()
        {
            for (int i = 0; i < GameSettings.sizex+5; i++)
            {
                for (int j = 0; j < GameSettings.sizey+5; j++)
                {
                    gameField[i, j] = new TetrisCell();
                    gameField[i, j].FillCell(CellTypes.baseType);
                }
            }

            for (int i = 0; i < GameSettings.sizex; i++)
            {
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    gameField[i, j] = new TetrisCell();
                    gameField[i, j].FillCell(CellTypes.empty);
                }
            }

            curFigure = TetrisFigureFactory.RandomizeNormalFigure();
            curFigure.posx = 6;
            curFigure.posy = 0;
            nextFigure = TetrisFigureFactory.RandomizeNormalFigure();
        }

        public FigureTypes curFigureType; //k = figure int type

        public void DrawField()
        {
            GameFieldViewModel.ReDrawField(this);
            GameFieldViewModel.DrawFigure(this.curFigure, 0, 0);
            GameFieldViewModel.DrawFigure(this.nextFigure, GameSettings.sizex + 2, (GameSettings.sizey/2)+4);
        }

        public void DeleteLine(int h)
        {
            int count = 0;
            for (int j = h; j < h+4; j++)
            {
                if (j>GameSettings.sizey-1) break;
                int numBricks = 0;
                for (int i = 0; i < GameSettings.sizex; i++)
                {
                    if (!gameField[i, j].IsCellEmpty()) numBricks++;
                }
                if (numBricks==GameSettings.sizex)
                {
                    //Debug.Log("Line removed");
                    count++;
                    for (int i = 0; i < GameSettings.sizex; i++)
                    {
                        for (int k = j; k > 1; k--)
                        {
                            //gameField[i, k].ClearCell();
                            gameField[i, k].FillCellByCell(gameField[i, k-1]);
                            gameField[i, k - 1].ClearCell();
                        }
                    }
                }
                TetrisPlayerModel.Instance.score += count * count;
            }
        }

        public void TryMoveFigure(int move)
        {
            bool AllowMove = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!curFigure.gameFigure[i,j].IsCellEmpty() && 
                        ((i+curFigure.posx+move>=GameSettings.sizex) || 
                         (i + curFigure.posx + move <0) || 
                         (!gameField[curFigure.posx+i+move, curFigure.posy+j].IsCellEmpty())))
                    {
                        AllowMove = false;
                        break;
                    }
                }
                if (!AllowMove) break;
            }
            if (AllowMove)
            {
                curFigure.posx += move;
                //curFigure.posx = Math.Min(curFigure.posx, 0);
            }
        }

        public void TryMoveDown()
        {
            bool keepLoop = true;
            for (int i = 0; i < 4 && keepLoop; i++)
            {
                for (int j = 0; j < 4 && keepLoop; j++)
                {
                    bool bCellEmpty = false; 
                    if ((j + curFigure.posy + 1 < GameSettings.sizey) && (i + curFigure.posx<GameSettings.sizex) && (i + curFigure.posx >= 0))
                    {
                        bCellEmpty = gameField[i + curFigure.posx, j + curFigure.posy + 1].IsCellEmpty();
                    }
                    if (!curFigure.gameFigure[i,j].IsCellEmpty() && 
                        (!(bCellEmpty) || (j+curFigure.posy>GameSettings.sizey)))
                    {
                        if (curFigure.posy == 0)
                        {
                            for (int i1 = 0; i1 < 4; i1++)
                            {
                                if (!curFigure.gameFigure[i1, 0].IsCellEmpty())
                                {
                                    GameStateMachine.Instance.SwitchToReplay();
                                    keepLoop = false;
                                    break;
                                }
                            }
                        }
                        for (int i1 = 0; i1 < 4; i1++)
                        {
                            for (int j1 = 0; j1 < 4; j1++)
                            {
                                if (!curFigure.gameFigure[i1, j1].IsCellEmpty())
                                {
                                    gameField[i1 + curFigure.posx, j1 + curFigure.posy].FillCellByCell(curFigure.gameFigure[i1, j1]);
                                    //curFigure.gameFigure[i1, j1].ClearCell();
                                }
                            }
                        }
                        //do next block and remove curFigure
                        DeleteLine(curFigure.posy);
                        curFigure = nextFigure;
                        //    x := 6;
                        //y:= 0;
                        curFigure.posx = 6;
                        curFigure.posy = 0;
                        nextFigure = TetrisFigureFactory.RandomizeNormalFigure();
                        keepLoop = false;
                    }
                }
            }
            if (keepLoop) curFigure.posy++;
        }

        public void TryRotateFigure()
        {
            //TetrisFigure testMask = new TetrisFigure(_pool);
            bool AllowRotate = true;
            TetrisFigure tmpFig = new TetrisFigure();
            if (curFigure.curFigureType == FigureTypes.two) //    if k = 2 then
            {             //    begin
                if (!curFigure.gameFigure[1,0].IsCellEmpty()) //        if block[2, 1] then
                {//        begin
                    for (int i = 0; i < 4; i++)//            for i := 1 to 4 do
                    {//            begin
                     //                block1[i, 2] := true;
                        tmpFig.gameFigure[i, 1].FillCell(curFigure.gameFigure[1, i].CellType);
                        if ((gameField[i + curFigure.posx, 2 + curFigure.posy].CellType != CellTypes.empty) || //if( field[i + x, 2 + y] ) 
                            (i + curFigure.posx > GameSettings.sizex) || (i + curFigure.posx < 1) || //   or(i + x > 15 ) or(i + x< 1 )                      
                            (curFigure.posx + 2 > GameSettings.sizey) || (curFigure.posy + 2 < 1)) //or( 2 + y > 25 ) or( 2 + y< 1 ) then
                        {//               begin
                            //code: drawblock as is//drawblock;
                            AllowRotate = false;//exit;
                        }//                end;
                    }
                    //            end;                      //            block := block1;
                }//        end
                else  //        else
                {//        begin
                    for (int i = 0; i < 3; i++)
                    {
                        tmpFig.gameFigure[1, i].FillCell(curFigure.gameFigure[i, 1].CellType);
                    }//code: rotate block//            for i := 1 to 4 do
                    //                block1[2, i] := true;
                    //            block := block1;
                    AllowRotate = true;
                }//        end;
            }//    end
            else if (curFigure.curFigureType > FigureTypes.two) //    else if k > 2 then
            {//    begin
                DebugLogFigure(curFigure);
                for (int i = 0; i < 3; i++)
                {//        for i := 1 to 3 do
                    for (int j = 0; j < 3; j++)
                    {//        for j := 1 to 3 do
                        tmpFig.gameFigure[i, j].FillCell(curFigure.gameFigure[2 - j, i].CellType);
                        if (!curFigure.gameFigure[2-j,i].IsCellEmpty()) 
                        {

                            if (((!this.gameField[i+curFigure.posx, j+curFigure.posy].IsCellEmpty()) ||
                                (i+curFigure.posx>GameSettings.sizex) || (i+curFigure.posx<1) ||
                                (j+curFigure.posy > GameSettings.sizey) || (j + curFigure.posx < 1)) &&
                                (!curFigure.gameFigure[i,j].IsCellEmpty())
                                )
                            {
                                AllowRotate = false;
                            }
                        }
                    }
                }
            }
            if ((AllowRotate) && (curFigure.curFigureType >= FigureTypes.two))
            {
                DebugLogFigure(tmpFig);
                curFigure.ClearMeshes();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        curFigure.gameFigure[i, j].FillCellByCell(tmpFig.gameFigure[i, j]);
                    }
                }
                //curFigure.gameFigure = tmpFig.gameFigure;
                DebugLogFigure(curFigure);
            }
            //    drawblock;
        }

        public void ResetGame ()
        {
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    gameField[i, j].ClearCell();
                }
            }

            curFigure = TetrisFigureFactory.RandomizeNormalFigure();
            curFigure.posx = 6;
            curFigure.posy = 0;
            nextFigure = TetrisFigureFactory.RandomizeNormalFigure();
            TetrisPlayerModel.Instance.score = 0;
        }


        public void DoGameTickProgress()
        {
            TryMoveDown();
        }

        public void DoControllProgress(TetrisControllMove move)
        {
            switch (move)
            {
                case TetrisControllMove.left: TryMoveFigure(-1);
                    break;
                case TetrisControllMove.right: TryMoveFigure(1);
                    break;
                case TetrisControllMove.rotate: TryRotateFigure();
                    break;
            }
        }

        public void DebugLogState()
        {
            DebugLogFigure(curFigure);
            Debug.Log("Cur: field:");
            string line = "";
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                line += i.ToString() + ":";
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    line += gameField[i, j].IsCellEmpty() ? j.ToString() + "_" : j.ToString() + "*";
                }
                line += Environment.NewLine;
            }
            Debug.Log(line);
        }

        public void DebugLogFigure(TetrisFigure fig)
        {
            Debug.Log("Cur: figure:"+fig.curFigureType.ToString());
            string line="";
            for (int i = 0; i < 4; i++)
            {
                line+= i.ToString() + ":";
                for (int j = 0; j < 4; j++)
                {
                    line += fig.gameFigure[i, j].IsCellEmpty() ? j.ToString() + "_" : j.ToString() + "*";
                }
                //Debug.Log(line);
                line += Environment.NewLine;
            }
            Debug.Log(line);
        }
    }

    public class TetrisFigure
    {
        public TetrisCell[,] gameFigure = new TetrisCell[4, 4];
        public FigureTypes curFigureType;
        public int posx, posy; 

        public TetrisFigure()
        {
            for (int i=0; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    gameFigure[i, j] = new TetrisCell();
                }
            } 
        }

        public void ClearMeshes()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    gameFigure[i, j].ClearCell();
                }
            }
        }


        public void DrawFigure()
        {
            DrawFigure(0, 0);
        }

        public void DrawFigure(int offsetx, int offsety)
        {

        }

    }

    public enum FigureTypes {
       one = 1, 
       two = 2, 
       three = 3, 
       four = 4, 
       five = 5, 
       six = 6, 
       seven = 7
    }

    public enum TetrisControllMove
    {
        rotate,
        left,
        right,
        down,
        none
    }







}

public class TetrisModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
