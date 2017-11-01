using UnityEngine;
using System.Collections;
using System;
using TetrisTools;




//procedure drawfield;
//var i, j : integer;
//begin
//    setfillstyle(solidfill, black );
//    bar( 11, 1, 160, 260 );
//    for i := 1 to 15 do
//    for j := 1 to 25 do
//    if field[i, j] then
//    begin
//        setfillstyle(solidfill, green );
//        bar( 10 * i + 1, 10 * j + 1, 10 *( i + 1 ), 10 *( j + 1 ) );
//        setfillstyle(solidfill, blue );
//        bar( 10 * i + 3, 10 * j + 3, 10 *( i + 1 ) - 2, 10 *( j + 1 ) - 2 );
//    end;
//    setfillstyle(solidfill, black );
//    bar( 250, 180, 300, 230 );
//    for i := 1 to size[next] do
//    for j := 1 to size[next] do
//    if nextblock[i, j] then
//    begin
//        setfillstyle(solidfill, green );
//        bar( 10 * i + 251, 10 * j + 181, 10 *( i + 1 ) + 250, 10 *( j + 1 ) + 180 );
//        setfillstyle(solidfill, blue );
//        bar( 10 * i + 253, 10 * j + 183, 10 *( i + 1 ) + 248, 10 *( j + 1 ) + 178 );
//    end;
//end;

//procedure delblock;
//var i, j : integer;
//begin
//    setfillstyle(solidfill, black );
//    for i := 1 to size[k] do
//    for j := 1 to size[k] do
//    if block[i, j] then bar( 10 * x + 10 * i + 1, 10 * y + 10 * j + 1,
//                               10 * x + 10 *( i + 1 ), 10 * y + 10 *( j + 1 ) );
//end;

//procedure delline(h : integer );
//var i, j, k, c : integer;
//begin
//    c := 0;
//    for j := h to h + 3 do
//    begin
//        if j > 25 then break;
//        i := 1;
//        while( field[i, j] ) and(i< 15 ) do
//            inc(i );
//        if( field[i, j] ) and(i = 15) then
//    begin
//            inc(c );
//            for i := 1 to 15 do
//            for k := j downto 2 do
//                field[i, k] := field[i, k - 1];
//        end;
//    end;
//    drawfield;
//    score := score + c* c;
//    if( score >= 50 * level ) and(level< 10 ) then inc(level );
//updatescore;
//end;



//procedure move(m : integer );
//var i, j : integer;
//begin
//    for i := 1 to size[k] do
//    for j := 1 to size[k] do
//    if( block[i, j] ) and(( i + x + m > 15 ) or(i + x + m< 1 )
//                        or(field[x + i + m, y + j] ) ) then exit;
//delblock;
//    if m = 1 then inc(x ) else dec(x );
//drawblock;
//end;

//procedure movedown;
//var i, j, i1, j1 : integer;
//begin
//    for i := 1 to size[k] do
//    for j := 1 to size[k] do
//    if( block[i, j] ) and(( field[i + x, j + 1 + y] ) or(j + y >= 25 ) ) then
//begin
//        if y = 0 then for i1 := 1 to size[k] do
//        if block[i1, 1] then
//        begin
//            k := 8;
//            exit
//        end;
//        for i1 := 1 to size[k] do
//        for j1 := 1 to size[k] do
//        if block[i1, j1] then field[i1 + x, j1 + y] := true;
//        block := nextblock;
//        k := next;
//        next := random( 7 ) + 1;
//        build(next, nextblock );
//        delline(y + 1 );
//x := 6;
//        y := 0;
//        exit;
//    end;
//    delblock;
//    inc(y );
//drawblock;
//end;



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
    //d, m, i, j, k, x, y, time, count, score, level, next : integer;
    //a : char;

    public static class GameSettings
    {
        public static int sizex = 26;
        public static int sizey = 36;
    }


    public enum CellTypes
    {
        empty = 0,
        baseType = 1,
        borderBrick = -1,
    }


    public class TetrisFigureFactory
    {
        public TetrisFigure GetNormalFigure(FigureTypes figureType, GameCellPool pool)
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
        }

        public void ClearCell()
        {
            if (CellMesh != null)
            {
                _parentPool.AddFreeObject((int)CellType, CellMesh);
                CellMesh = null;
            }
        }
    }


    public class TetrisField
    {
        public TetrisCell[,] gameField = new TetrisCell[GameSettings.sizex, GameSettings.sizey];
        public static int[] fsize = new int[] { 2, 4, 3, 3, 3, 3, 3 };
        public TetrisFigure curFigure;
        public TetrisFigure nextFigure;

        public FigureTypes curFigureType; //k = figure int type


    }

    public class TetrisFigure
    {
        public TetrisCell[,] gameFigure = new TetrisCell[3, 3];
        private GameCellPool _pool;
        public FigureTypes curFigureType;
        public int posx, posy; 

        public TetrisFigure(GameCellPool pool)
        {
            for (int i=0; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    gameFigure[i, j] = new TetrisCell(pool);
                    //gameFigure[i, j].CellType
                }
            } 
        }

        public void RotateFigure(TetrisField field)
        {
            //TetrisFigure testMask = new TetrisFigure(_pool);
            bool AllowRotate = true;
            if (curFigureType == FigureTypes.two) //    if k = 2 then
            {             //    begin
                if (this.gameFigure[1, 0].CellType == CellTypes.baseType) //        if block[2, 1] then
                {//        begin
                    for (int i = 0; i < 4; i++)//            for i := 1 to 4 do
                    {//            begin
                     //                block1[i, 2] := true;
                        if ((field.gameField[i + posx, 2 + posy].CellType != CellTypes.empty) || //if( field[i + x, 2 + y] ) 
                            (i + posx > 15) || (i + posx < 1) || //   or(i + x > 15 ) or(i + x< 1 )                      
                            (posx + 2 > 26) || (posy + 2 < 1)) //or( 2 + y > 25 ) or( 2 + y< 1 ) then
                        {//               begin
                            //code: drawblock as is//drawblock;
                            AllowRotate = false;//exit;
                        }//                end;
                    }//            end;                      //            block := block1;
                }//        end
                else  //        else
                {//        begin
                 //code: rotate block//            for i := 1 to 4 do
                 //                block1[2, i] := true;
                 //            block := block1;
                    AllowRotate = true;
                }//        end;
            }//    end
            else if (curFigureType > FigureTypes.two) //    else if k > 2 then
            {//    begin
                for (int i=0; i<4; i++)
                {//        for i := 1 to 3 do
                    for (int j=0; j<4; j++)
                    {//        for j := 1 to 3 do
                        if (true) //        if block[4 - j, i] then
                {//        begin
                 //            block1[i, j] := block[4 - j, i];
                 if (true)//            if(( field[i + x, j + y] ) or(i + x > 15 ) or(i + x< 1 ) or(j + y > 25 )
                 //                                         or(j + y< 1 ) ) and(block1[i, j] ) then
                    {//       begin
                     //code//                drawblock;
                                AllowRotate = false;//                exit
                    }//            end;
                }
                    }
                }//end;
                 //        block := block1;
            }//    end;
            //    drawblock;
        }

        public void DrawFigure()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameFigure[i, j].CellType==CellTypes.baseType)
                    {
                        gameFigure[i, j].CellMesh.transform.position = new Vector3(i+posx, j+posy, 0);
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







}

public class TetrisModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
