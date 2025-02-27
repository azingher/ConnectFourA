/*=============================================================================
 * 
 * 
 *  Copyright Ayal Zingher 2016
 * 
 * 
 * 
 * ============================================================================*/
using ConnectFourDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourUI
{
    /// <summary>
    /// View model for interaction
    /// </summary>
    public class ConnectFourVM : INotifyPropertyChanged
    {
        #region implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (string.IsNullOrEmpty(name))
            {
                Debug.WriteLine("Binding error: OnPropertyChanged in " + (this.GetType()).ToString() + " got an invalid name " + name);
                
            }
            if (handler != null)
            {
                try
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
                catch (Exception)
                {
                    Debug.WriteLine("OnPropertyChanged in ObservableObject threw an exception in a Binding update");
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    
                }
            }
        }
        #endregion

        private GameBoard myBoard;
        private bool isFirstPlayer = false;

        #region construction
        public ConnectFourVM()
        {
            myBoard = new GameBoard();
        }

        #endregion

        #region properties
        //DataTable PrettyTable;
        public DataTable MyBoard 
        { 
            get
            {
                return myBoard.NiceTable;
            }
            set
            {
                OnPropertyChanged("MyBoard");
                OnPropertyChanged("TokenBoard");
            }
        }
        public DataTable  TokenBoard 
        { 
            get
            {
                DataTable ret = new DataTable();
                for(int j=0; j < MyBoard.Columns.Count; j++)
                {
                    ret.Columns.Add(MyBoard.Columns[j].ColumnName);
                }
                for(int i = 0; i < MyBoard.Rows.Count; i++)
                {
                    ret.Rows.Add();
                }
                for(int i = 0; i>MyBoard.Rows.Count; i++)
                    for(int j=0; j < MyBoard.Columns.Count; j++)
                    {
                        if (MyBoard.Rows[i][j].ToString() == "X")
                            ret.Rows[i][j] = new RedToken();
                        else if (MyBoard.Rows[i][j].ToString() == "0")
                            ret.Rows[i][j] = new BlackToken();
                        else
                        {
                            ret.Rows[i][j] = new BlueToken();
                        }
                    }
                return ret;
            }
            set
            {

            }
        }
        private string gameHeader = "CONNECT FOUR";
        public string GameHeader 
        { 
            get
            {
                return gameHeader;
            }
            set
            {
                if (value == gameHeader)
                    return;
                gameHeader = value;
                OnPropertyChanged("GameHeader");
            }
        }
        public bool IsFirstPlayer 
        { 
            get
            {
                return isFirstPlayer;
            }
            set
            {
                if (value == isFirstPlayer)
                    return;
                isFirstPlayer = value;
                OnPropertyChanged("IsFirstPlayer");

            }
        }

        public System.Windows.UIElement NextTile 
        { 
            get
            {
                if(IsFirstPlayer)
                    return new BlackToken();
                else
                    return new RedToken();
            }
            set
            {

            }
        }
        
        #endregion

        #region methods
        public void UpdateView()
        {
            this.UpdateUI(this);
        }

        private void UpdateUI(Object Sender)
        {
            Type t = Sender.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                string name = pi.Name;
                if (string.IsNullOrEmpty(name))
                    continue;
                OnPropertyChanged(name);
            }
        }

        public bool AddTile(int p, out string message)
        {
            IsFirstPlayer = !IsFirstPlayer;
            int playernum = isFirstPlayer? 1 : -1;
            UpdateUI(this);
            if (!myBoard.AddATile(p-1,playernum, out message))
            {
                //UpdateUI(this);
                IsFirstPlayer = !IsFirstPlayer;
                return false;
            }
            return true;
        }



        public bool ResetGame(out string message)
        {
            message = "Initializing";
            try
            {
                myBoard = new GameBoard();
                UpdateUI(this);
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

            return true;
        }
        #endregion
    }
}
