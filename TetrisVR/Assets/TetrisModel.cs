using UnityEngine;
using System.Collections;
using System;
using TetrisTools;


//procedure pause;
//begin
//    clearviewport;
//    moveto( 230, 60 );
//    outtext( 'The game is paused' );
//    moveto( 150, 90 );
//    outtext( 'Press F3 to continue or press Esc to exit' );
//repeat
//    a := readkey;
//    until(a = #61 ) or( a = #27 );
//    if a = #27 then halt;
//    gameinterface;
//drawfield;
//    drawblock;
//    updatescore;
//end;

//begin
//    detectgraph(d, m );
//    initgraph(d, m, '' );
//gameinterface;
//    newgame : score := 0;
//    level := 1;
//    updatescore;
//    setfillstyle(solidfill, black );
//    bar( 11, 1, 160, 260 );
//x := 6;
//    y := 0;
//    randomize;
//    k := random( 7 ) + 1;
//    build(k, block );
//    for i := 1 to 15 do
//    for j := 1 to 25 do
//        field[i, j] := false;
//    next := random( 7 ) + 1;
//    build(next, nextblock );
//drawfield;
//    repeat
//        if count = 0 then time := 40 - 3 * level;
//        for i := 1 to 15 - level do
//        begin
//            if keypressed then
//            begin
//                a := readkey;
//                if a = #0 then a := readkey;
//                case a of
//                    #75 : move( - 1 );
//                    #77 : move( 1 );
//                    #72 : rotate( k );
//                    #80 :
//                    begin
//                        count := count + 2 *( 15 - level );
//                        time := 0
//                    end;
//                    #60 : goto newgame;
//                    #61 : pause;
//                end;
//            end;
//            while keypressed do
//                readkey;
//            delay(time );
//            if count > 0 then dec(count );
//end;
//        movedown;
//        if k = 8 then goto gamover;
//    until(a = #27 );
//    exit;
//gamover : moveto( 50, 60 );
//    outtext( 'Game Over!' );
//repeat
//    a := readkey;
//    until(a = #60 ) or( a = #27 );
//    if a = #60 then goto newgame;
//end.

namespace TetrisCore
{

    public static class GameSettings
    {
        public static int sizex = 26;
        public static int sizey = 36;
    }


    public enum CellTypes
    {
        empty = 0,
        baseType = 1,
        borderBrick = 2,
    }


    public static class TetrisFigureFactory
    {
        public static TetrisFigure GetNormalFigure(FigureTypes figureType, GameCellPool pool)
        {
            TetrisFigure obj = new TetrisFigure(pool);

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
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);//b[i + 1, 2] := true;
                        obj.gameFigure[i, 1].FillCell(CellTypes.baseType);//b[i, 1] := true
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

        public static TetrisFigure RandomizeNormalFigure(GameCellPool pool)
        {
            int nF = UnityEngine.Random.Range(1, 8);
            return TetrisFigureFactory.GetNormalFigure((FigureTypes)nF, pool);
        }
    }

    public class TetrisCell 
    {
        private GameCellPool _parentPool;

        public GameObject CellMesh { get; set; }
        public CellTypes CellType;

        public TetrisCell(GameCellPool parent)
        {
            _parentPool = parent;
            CellType = CellTypes.empty;
            CellMesh = null;
        }

        public void FillCell(CellTypes cellType)
        {
            CellType = cellType;
            CellMesh = _parentPool.GetFreeMesh((int)cellType);
            CellMesh.SetActive(true);
        }

        public void ClearCell()
        {
            if (CellMesh != null)
            {
                _parentPool.AddFreeObject(CellType, CellMesh);
                CellMesh = null;
                CellType = CellTypes.empty;
            }
        }

        public bool IsCellEmpty()
        {
            return CellType != CellTypes.baseType;
        }

        public void UpdateCellPosition(int x, int y)
        {
            if (CellMesh!=null)
            {
                CellMesh.transform.position = new Vector3(x, y, 0);
            }
        }

        public void FillCellByCell(TetrisCell cell)
        {
            CellType = cell.CellType;
            CellMesh = CellMesh;
        }
    }

    public class TetrisPlayerModel
    {
        private static TetrisPlayerModel _instance;

        public int level;
        public int score;

        private TetrisPlayerModel() { }

        public TetrisPlayerModel Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new TetrisPlayerModel();
                }
                return _instance;
            }
        }
    }


    public class TetrisField
    {
        public TetrisCell[,] gameField = new TetrisCell[GameSettings.sizex, GameSettings.sizey];
        //public static int[] fsize = new int[] { 2, 4, 3, 3, 3, 3, 3 };
        public TetrisFigure curFigure;
        public TetrisFigure nextFigure;
        public TetrisPlayerModel playerModel;

        private GameCellPool _pool;
        public GameCellPool Pool
        {
            get
            {
                return _pool;
            }
        }

        public TetrisField(GameCellPool pool)
        {
            _pool = pool;
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    gameField[i, j] = new TetrisCell(_pool);
                }
            }

            curFigure = TetrisFigureFactory.RandomizeNormalFigure(Pool);
            curFigure.posx = 6;
            curFigure.posy = 0;
            nextFigure = TetrisFigureFactory.RandomizeNormalFigure(Pool);
        }

        public FigureTypes curFigureType; //k = figure int type

        public void DrawField()
        {
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    if (!gameField[i,j].IsCellEmpty())
                    {
                        gameField[i, j].UpdateCellPosition(i, j);
                    }
                }
            }
            curFigure.DrawFigure();
            nextFigure.DrawFigure(GameSettings.sizex + 10, GameSettings.sizey - 10);
        }

        public void DeleteLine(int h)
        {
            int count = 0;
            for (int j = h; j < h+4; j++)
            {
                if (j>GameSettings.sizey-2) break;
                int numBricks = 0;
                for (int i = 0; i < GameSettings.sizex; i++)
                {
                    if (!gameField[i, j].IsCellEmpty()) numBricks++;
                }
                if (numBricks==GameSettings.sizex)
                {
                    count++;
                    for (int i = 0; i < GameSettings.sizex; i++)
                    {
                        for (int k = j; k > 1; k--)
                        {
                            gameField[i, k].ClearCell();
                            gameField[i, k].FillCellByCell(gameField[i, k-1]);
                            gameField[i, k - 1].ClearCell();
                        }
                    }
                }
                playerModel.Instance.score += count * count;
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
                        ((i+curFigure.posx+move>GameSettings.sizex) || 
                        (i + curFigure.posx + move <0) || (!gameField[curFigure.posx+i+move, curFigure.posy+j].IsCellEmpty())))
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
            }
        }

        public void TryMoveDown()
        {
            bool keepLoop = true;
            for (int i = 0; i < 4 && keepLoop; i++)
            {
                for (int j = 0; j < 4 && keepLoop; j++)
                {
                    if ((!curFigure.gameFigure[i,j].IsCellEmpty()) && 
                        (!(gameField[i+curFigure.posx, j+curFigure.posy+1].IsCellEmpty() ||
                        (j+curFigure.posy>=GameSettings.sizey))))
                    {
                        if (curFigure.posy == 0)
                        {
                            for (int i1 = 0; i1 < 4; i1++)
                            {
                                if (!curFigure.gameFigure[i1, 0].IsCellEmpty())
                                {
                                    keepLoop = false;
                                    break;
                                }
                            }
                        }
                        for (int i1 = 0; i1 < 4; i1++)
                        {
                            for (int j1 = 0; j1 < 4; j1++)
                            {
                                gameField[i1 + curFigure.posx, j1 + curFigure.posy].FillCellByCell(curFigure.gameFigure[i1,j1]);
                                //curFigure.gameFigure[i1, j1].ClearCell();
                            }
                        }
                        //do next block and remove curFigure
                        curFigure = nextFigure;
                        //    x := 6;
                        //y:= 0;
                        curFigure.posx = 6;
                        curFigure.posy = 0;
                        nextFigure = TetrisFigureFactory.RandomizeNormalFigure(Pool);
                        keepLoop = false;
                    }
                }
            }
            if (keepLoop) curFigure.posy++;        }

        public void TryRotateFigure()
        {
            //TetrisFigure testMask = new TetrisFigure(_pool);
            bool AllowRotate = true;
            TetrisFigure tmpFig = new TetrisFigure(Pool);
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
            else if (curFigureType > FigureTypes.two) //    else if k > 2 then
            {//    begin
                for (int i = 0; i < 3; i++)
                {//        for i := 1 to 3 do
                    for (int j = 0; j < 3; j++)
                    {//        for j := 1 to 3 do
                        tmpFig.gameFigure[i, j].FillCell(curFigure.gameFigure[3 - j, i].CellType);
                        if (!curFigure.gameFigure[3-j,i].IsCellEmpty()) 
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
            if (AllowRotate)
            {
                curFigure.ClearMeshes();
                curFigure.gameFigure = tmpFig.gameFigure;
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

            curFigure = TetrisFigureFactory.RandomizeNormalFigure(Pool);
            curFigure.posx = 6;
            curFigure.posy = 0;
            nextFigure = TetrisFigureFactory.RandomizeNormalFigure(Pool);
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
            Debug.Log("Cur: figure:");
            for (int i = 0; i < 4; i++)
            {
                string line= "";
                for (int j = 0; j < 4; j++)
                {
                    line += curFigure.gameFigure[i, j].IsCellEmpty() ? "_" : "*";
                }
                Debug.Log(line);
            }
            Debug.Log("Cur: field:");
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                string line = "";
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    line += gameField[i, j].IsCellEmpty() ? "_" : "*";
                }
                Debug.Log(line);
            }

        }
    }

    public class TetrisFigure
    {
        public TetrisCell[,] gameFigure = new TetrisCell[4, 4];
        private GameCellPool _pool;
        public FigureTypes curFigureType;
        public int posx, posy; 

        public TetrisFigure(GameCellPool pool)
        {
            _pool = pool;
            for (int i=0; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    gameFigure[i, j] = new TetrisCell(_pool);
                    gameFigure[i, j].CellType = CellTypes.empty;
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

        public void RotateFigure(TetrisField field)
        {
        }
        public void DrawFigure()
        {
            DrawFigure(0, 0);
        }

        public void DrawFigure(int offsetx, int offsety)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameFigure[i, j].CellType==CellTypes.baseType)
                    {
                        gameFigure[i, j].CellMesh.transform.position = new Vector3(i+posx+ offsetx, j+posy+offsety, 0);
                    }
                }
            }
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
        down
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
