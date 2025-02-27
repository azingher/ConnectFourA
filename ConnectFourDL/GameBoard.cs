/*=============================================================================
 * 
 * 
 *  Copyright Ayal Zingher 2016
 * 
 * 
 * 
 * ============================================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourDL
{
    public class GameBoard
    {
        //DataTable theBoard;
        public static readonly List<string> theColumns = new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
        private Int32[] StartingRow =  { 0, 0, 0, 0, 0, 0 };
        public static GameStatuses myGamestatus = GameStatuses.Incomplete;
        //private playerKeys myPlayer = playerKeys.Player1;
        private static int numRows;
        private static int numcolumns;
        private Int32[,] theBoard;
        #region construction
        /// <summary>
        /// Creates the board for the columns,rows
        /// </summary>
        public GameBoard()
        {
            numRows = StartingRow.Count();
            numcolumns = theColumns.Count();
            //Create the board to play
            theBoard = new Int32[7,6];
            //Set flags
            myGamestatus = GameStatuses.Incomplete;
            //myPlayer = playerKeys.Player1;
            
        }
#endregion
        #region properties
        public GameStatuses MyGameStatus 
        { 
            get
            {
                return myGamestatus;
            }
            set
            {
                myGamestatus = value;
            }
        }

        public Int32[,] TheBoard 
        { 
            get
            {
                return theBoard;
            }
            set
            { 
                Debug.WriteLine("attempt to change board externally"); 
            }
        }
        public DataTable NiceTable //Versin 2 can have nice-looking objets.
        { 
            get
            {
                //Make the table
                DataTable ret = new DataTable();
                for (int i = 0; i < numcolumns; i++)
                {
                    ret.Columns.Add(Convert.ToChar(65 + i).ToString());

                }
                for (int i = numRows-1; i >= 0; i--)
                {
                    ret.Rows.Add();

                }
                //Populate the table down-up
                for (int i = 0; i < numcolumns; i++)
                {
                    for (int j = 0; j < numRows; j++)
                    {
                        if (theBoard[i, j] == 1)
                            ret.Rows[numRows - j - 1][i] = "0";
                        else if (theBoard[i, j] == -1)
                            ret.Rows[numRows - j - 1][i] = "X";
                        else if (theBoard[i, j] == 0)
                            ret.Rows[numRows - j - 1][i] = " ";
                        else 
                        {
                            Debug.WriteLine("got a weird value " + ret.Rows[numRows - j - 1][i]);
                        }

                    }

                }
                return ret;

                
            }
            set
            {
                Debug.WriteLine("Attempt to set niceTable externally");
            }
        }

        #endregion

        /// <summary>
        /// Add the tile to the specified column then check the status
        /// </summary>
        /// <param name="columNum"></param>
        /// <param name="message"></param>
        /// <returns>true for accepted move and to continue game </returns>
        public bool AddATile(int columNum, int playerNum, out string message)
        {
            int realColnum = columNum; // -1; //convert to sero-based index.
            bool ret = true;
            message = "Adding tile";
            if (this.MyGameStatus != GameStatuses.Incomplete)
            {
                message = "Game is locked!! Status is " + myGamestatus.ToString();
                this.MyGameStatus = GameStatuses.Invalid;//No adding after game-end!!
                return false;
            }

            //if (this.MyGameStatus == GameStatuses.Player1Win || this.MyGameStatus == GameStatuses.Player2Win)
            //{
            //    message = "No adding after game-end!!";
            //    this.MyGameStatus = GameStatuses.Invalid;//No adding after game-end!!
            //    return false;
            //}
            //Valid column?
            if (realColnum < 0 || realColnum > numcolumns - 1)
            {
                message = "Invalid column";
                //this.MyGameStatus = GameStatuses.Invalid;
                return false;
            }
            bool inserted = false;
            int newRow = 0;
            int _tryRow = 0;
            for (int i = 0; i < numRows; i++)
            {
                _tryRow = i;
                if((int)theBoard[realColnum,i] == 0)
                {
                    theBoard[realColnum,i] = playerNum;
                    newRow = i;
                    inserted = true;
                    break;
                }
            }
            if(!inserted)
            {
                //check if game-end
                bool BoardIsFull = false;
                if (_tryRow == numRows - 1)
                {
                    BoardIsFull = true;
                    for (int l = 0; l < numRows; l++)
                    {
                        if (theBoard[l, numRows - 1] == 0)
                        {
                            BoardIsFull = false;
                            break;
                        }
                    }
                }
                if(BoardIsFull)
                {
                    this.MyGameStatus = GameStatuses.Draw;
                    message = "Board is full. This is a draw";
                    return false;
                }
                message = "Illegal entry!";
                //this.MyGameStatus = GameStatuses.Invalid;
                return false;
            }

            if (!CheckGameBoardStatus(columNum, newRow, out message))
            {
                if (myGamestatus == GameStatuses.Player1Win)
                    message = "Player 1 win";
                else if (myGamestatus == GameStatuses.Player2Win)
                    message = "Player 2 win";
                return false;
            }
            return ret;
        }
        /// <summary>
        /// Check if four are connected and sets the result.
        /// </summary>
        /// <param name="columNum"></param>
        /// <param name="newRow"></param>
        /// <param name="message"></param>
        /// <returns>true if the game contiues</returns>
        private bool CheckGameBoardStatus(int columNum, int newRow, out string message)
        {
            //bool ret = true;
            message = "Checking board status";

            //*** check gameboard status
            //Check horizontal - walk to the right while same value and within grid.  then walk to the left and see if this is a win.
            int inARow = 1;
            int playervalue = (int)theBoard[columNum,newRow];

            //then update grid status.

            //check verticel
            if(newRow >= 3)
            {
                for(int i=newRow-1; i >=0; i-- )
                {
                    if (theBoard[columNum,i] == playervalue)
                        inARow++;
                    else
                        break;
                }
            }
            if(inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            //Check horizontal
            inARow = 1;
            //Go left
            if(columNum > 0)
                for (int i = columNum - 1; i >= 0; i--)
                {
                    if (theBoard[i,newRow] == playervalue)
                        inARow++;
                    else
                        break;
                }
            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            //go right
            if(columNum < numcolumns - 1)
                for (int i = columNum + 1; i < numcolumns; i++)
                {
                    if (theBoard[i,newRow] == playervalue)
                        inARow++;
                    else
                        break;
                }
            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            //Check upleft
            inARow = 1;
            if (columNum > 0 && newRow > 0)
            {
                for(int i=1; columNum-i >= 0 && newRow - i >= 0 ; i++)
                {
                    if (theBoard[columNum - i, newRow - i] == playervalue)
                        inARow++;
                    else
                        break;
                }
            }

            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }

            //Add downright
            if (columNum < numcolumns -1 && newRow < numRows - 1)
            {
                for (int i = 1; columNum + i < numcolumns && newRow + i < numRows; i++)
                {
                    if (theBoard[columNum + i, newRow + i] == playervalue)
                        inARow++;
                    else
                        break;
                }
            }
            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            inARow = 1;
            //check upright
            if (columNum < numcolumns - 1 && newRow > 0)
            {
                for (int i = 1; columNum + i < numcolumns && newRow - i >= 0; i++)
                {
                    if (theBoard[columNum + i, newRow - i] == playervalue)
                        inARow++;
                    else
                        break;
                }
            }
            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            //Add downleft
            if (columNum > 0 && newRow < numRows -1  )
            {
                for (int i = 1; newRow + i < numRows && columNum - i >= 0; i++)
                {
                    if ((int)theBoard[columNum - i, newRow + i] == playervalue)
                        inARow++;
                    else
                        break;
                }
            }
            if (inARow >= 4)
            {
                if (playervalue == (int)playerKeys.Player1)
                    this.MyGameStatus = GameStatuses.Player1Win;
                else
                    this.MyGameStatus = GameStatuses.Player2Win;
                message = "Game end";
                return false;
            }
            message = "Not a winning step yet";


            return true;
        }

    }
    public enum GameStatuses
    {
        Incomplete,
        Invalid,
        Player1Win,
        Player2Win,
        Draw
    }
    public enum playerKeys
    {
        Player1 = 1,
        Player2 = -1

    }
}
