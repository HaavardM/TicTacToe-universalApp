using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Windows.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TicTacToe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //2D array for storing game buttons
        private Button[,] gameButtons = new Button[3, 3];

        //Variables:
        private bool playerX = true;
        private bool gameWon = false;
        private int moves = 0;

        //Statistics
        private int playerXwins = 0;
        private int playerOwins = 0;
        private int ties = 0;

        //On page create
        public MainPage()
        {
            this.InitializeComponent();
            //Set preferred window size
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(400, 310));
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Size(400, 310);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //Add buttons to 2d arrays
            addButtons();
            //Randomly select first player
            randomPlayer();
        }

        //Handler to take care of button clicked for gamebuttons
        private void btnClickHandler(object sender, RoutedEventArgs e)
        {

            Button buttonClicked = sender as Button;
            if((buttonClicked.Content as String) == "")
            {
                if(playerX)
                {
                    buttonClicked.Content = "X";
                    currentPlayerText.Text = "O";
                }
                else
                {
                    buttonClicked.Content = "O";
                    currentPlayerText.Text = "X";
                }
                playerX = !playerX;
                moves++;
                hasWon();
                if(moves >= 9 && !gameWon)
                {
                    tie();
                }
                
            }
        }

        //Select a random player - 50/50
        private void randomPlayer()
        {
            playerX = (new Random().Next(0, 100) < 50);
            if (playerX)
            {
                currentPlayerText.Text = "X";
            }
            else
            {
                currentPlayerText.Text = "O";
            }
        }

        //Check if one of the players have won.
        private void hasWon()
        {
            //Horizontal
            for(int i = 0; i < 3; i++)
            {
                int numberOfX = 0;
                int numberOfO = 0;
                for(int j = 0; j < 3; j++)
                {
                    Button btn = gameButtons[i, j]; //Get button
                    //Check if button contains X or O and increase counter
                    if((btn.Content as String) == "X")
                    {
                        numberOfX++;
                    }
                    else if((btn.Content as String) == "O")
                    {
                        numberOfO++;
                    }
                }
                //If 3 in a row - player won.
                if (numberOfO == 3)
                {
                    playerWon('O');
                    gameWon = true;
                } else if(numberOfX == 3)
                {
                    playerWon('X');
                    gameWon = true;
                }
            }
            
            //Vertical - pretty much the same logic as horizontal.
            for(int i = 0; i < 3; i++)
            {
                int numberOfX = 0;
                int numberOfO = 0;
                for(int j = 0; j < 3; j++)
                {
                    Button btn = gameButtons[j, i];
                    if((btn.Content as String) == "X")
                    {
                        numberOfX++;
                    } 
                    else if((btn.Content as String) == "O")
                    {
                        numberOfO++;
                    }
                }
                if (numberOfO == 3)
                {
                    playerWon('O');
                    gameWon = true;
                } else if(numberOfX == 3)
                {
                    playerWon('X');
                    gameWon = true;
                }
            }

            //Counters for two diagonals for each player
            int numberOfXU = 0;
            int numberOfXD = 0;
            int numberOfOU = 0;
            int numberOfOD = 0;

            for(int i = 0; i < 3; i++)
            {
                Button btn = gameButtons[i, i];
                
                if((btn.Content as String) == "X")
                {
                    numberOfXD++;
                }
                else if((btn.Content as String) == "O")
                {
                    numberOfOD++;
                }
                btn = gameButtons[2 - i, i];
                if ((btn.Content as String) == "X")
                {
                    numberOfXU++;
                }
                else if ((btn.Content as String) == "O")
                {
                    numberOfOU++;
                }
            }
            //If either of diagonals is 3 in a row.
            if(numberOfOD ==3 || numberOfOU ==3)
            {
                //O has won
                playerWon('O');
                gameWon = true;
            } else if(numberOfXD == 3 || numberOfXU == 3)
            {
                playerWon('X');
                gameWon = true;
            }
        }
        
        //Add buttons to 2d array
        private void addButtons()
        {
            gameButtons[0, 0] = btn1;
            gameButtons[0, 1] = btn2;
            gameButtons[0, 2] = btn3;
            gameButtons[1, 0] = btn4;
            gameButtons[1, 1] = btn5;
            gameButtons[1, 2] = btn6;
            gameButtons[2, 0] = btn7;
            gameButtons[2, 1] = btn8;
            gameButtons[2, 2] = btn9;
        }

        //Actions to perform when a player wins.
        private async void playerWon(char user)
        {
            await new MessageDialog(user + " has won the game!").ShowAsync();
            if(user == 'X')
            {
                playerXwins++;
            }
            else if(user == 'O')
            {
                playerOwins++;
            }
            statisticsXCount.Text = playerXwins.ToString();
            statisticsOCount.Text = playerOwins.ToString();
            gameWon = true;
            newGameButton.Visibility = Visibility.Visible;
            foreach(Button b in gameButtons)
            {
                b.IsEnabled = false;
            }
        }
      
        //Actions to perform when the game ends in a tie
        private async void tie()
        {
            statisticsTieCount.Text = (++ties).ToString();
            await new MessageDialog("Tie").ShowAsync();
            newGameButton.Visibility = Visibility.Visible;
            foreach (Button b in gameButtons)
            {
                b.IsEnabled = false;
            }

        }

        //New game button is pressed. 
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        //Create new game
        private void newGame()
        {
            gameWon = false;
            moves = 0;
            newGameButton.Visibility = Visibility.Collapsed;
            foreach (Button b in gameButtons)
            {
                b.Content = "";
                b.IsEnabled = true;
            }
            randomPlayer();
        }

        //Key pressed handler
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.R)
            {
                playerXwins = playerOwins = ties = 0;
                statisticsXCount.Text = statisticsOCount.Text = statisticsTieCount.Text = "0";
            }
            else if(e.Key == Windows.System.VirtualKey.N)
            {
                newGame();
            }
        }
    }
}
