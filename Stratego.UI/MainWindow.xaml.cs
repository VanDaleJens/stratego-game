using Stratego.Core;
using Stratego.Core.Enums;
using Stratego.Core.Pawns;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Stratego.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Label[,] labels;
        private GameBoard gameBoard;
        private Label selectedPiece;
        private const int BOARD_WIDTH = 10;
        private const int LABEL_SIZE = 70;
        private bool isStarted;
        private bool isError;
        public MainWindow()
        {
            InitializeComponent();
            InitBoard();
        }

        // Methods

        private void InitBoard()
        {
            cmbPlayer1Color.ItemsSource = System.Enum.GetValues(typeof(PieceColor));
            cmbPlayer2Color.ItemsSource = System.Enum.GetValues(typeof(PieceColor));

            PieceColor player1Color = (PieceColor)cmbPlayer1Color.SelectedItem;
            PieceColor player2Color = (PieceColor)cmbPlayer2Color.SelectedItem;
            if (player2Color == player1Color)
            {
                isStarted = false;
                btnQuitGame.IsEnabled = false;
                btnNewGame.IsEnabled = true;
                MessageBox.Show("Player and Npc color can't be the same.\n please choose a different color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isError = true;
                if (!gameBoard.GameEnded)
                {
                    cmbPlayer1Color.IsEnabled = true;
                    cmbPlayer2Color.IsEnabled = true;
                }
                return;
            }
            isError = false;
            gameBoard = new GameBoard(player1Color, player2Color);

            labels = new Label[BOARD_WIDTH, BOARD_WIDTH];
            Width = LABEL_SIZE * BOARD_WIDTH + 600;
            Height = LABEL_SIZE * BOARD_WIDTH + 100;

            for (int row = 0; row < BOARD_WIDTH; row++)
            {
                for (int column = 0; column < BOARD_WIDTH; column++)
                {
                    var backgroundColor = new SolidColorBrush(Colors.LightGreen);
                    if (row == 4 || row == 5)
                    {
                        if (column == 2 || column == 3 || column == 6 || column == 7) backgroundColor = new SolidColorBrush(Colors.LightSkyBlue);
                    }

                    Label lbl = new Label
                    {
                        Width = LABEL_SIZE,
                        Height = LABEL_SIZE,
                        Background = backgroundColor,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(2.0),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeights.Bold
                    };

                    lbl.MouseUp += HandlePieceClick;

                    grBoard.Children.Add(lbl);
                    Grid.SetColumn(lbl, column);
                    Grid.SetRow(lbl, row);

                    labels[row, column] = lbl;
                }
            }
            lbBattleLog.Items.Clear();
        }

        private void RefreshBoard()
        {
            var gameField = gameBoard.GetPlayingField();

            for (int row = 0; row < BOARD_WIDTH; row++)
            {
                for (int column = 0; column < BOARD_WIDTH; column++)
                {
                    PlayingPiece p = gameField[row, column];
                    if (p != null)
                    {
                        if (gameBoard.isPlayerTurn)
                        {
                            if (p.Color == (PieceColor)cmbPlayer1Color.SelectedItem)
                            {
                                labels[row, column].Content = p.Name;
                                if(p is Veteran) labels[row, column].Content = $"Vet.  {p.Rank}";
                            }
                            else labels[row, column].Content = "#2";
                        }
                        else
                        {
                            if (p.Color == (PieceColor)cmbPlayer2Color.SelectedItem)
                            {
                                labels[row, column].Content = p.Name;
                                if (p is Veteran) labels[row, column].Content = $"Vet.  {p.Rank}";
                            } 
                            else labels[row, column].Content = "#1";
                        }

                        labels[row, column].Foreground =
                            (p.Color == PieceColor.Red) ? Brushes.Red :
                            (p.Color == PieceColor.Blue) ? Brushes.Blue :
                            (p.Color == PieceColor.Yellow) ? Brushes.DarkGoldenrod :
                            (p.Color == PieceColor.Purple) ? Brushes.Purple :
                            (p.Color == PieceColor.Pink) ? Brushes.HotPink :
                            Brushes.Black;
                    }
                    else
                    {
                        labels[row, column].Content = string.Empty;
                    }
                }
            }
            if(!isStarted)
            {
                cmbPlayer1Color.IsEnabled = true;
                cmbPlayer2Color.IsEnabled = true;
            }

            gameBoard.CheckWinner();
            if (gameBoard.GameEnded == true)
            {
                if(!isError) MessageBox.Show(gameBoard.WinnerText, "End Game!!!", MessageBoxButton.OK ,MessageBoxImage.Information);
                cmbPlayer1Color.IsEnabled = true;
                cmbPlayer2Color.IsEnabled = true;
                btnNewGame.IsEnabled = true;
                btnQuitGame.IsEnabled = false;
            }

            gameBoard.BattleLog.ForEach(battleLogItem => lbBattleLog.Items.Add(battleLogItem));
        }

        private void DeselectExisting()
        {
            if (selectedPiece != null)
            {
                selectedPiece.BorderBrush = Brushes.Black;
                selectedPiece = null;
            }
        }

        // Event handlers

        private void HandlePieceClick(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            byte column = (byte)Grid.GetColumn(lbl);
            byte row = (byte)Grid.GetRow(lbl);

            if (selectedPiece == lbl || gameBoard.GameEnded || !isStarted)
            {
                DeselectExisting();
            }
            else if (gameBoard.HasMovablePieceOn(row, column))
            {
                DeselectExisting();
                selectedPiece = lbl;
                lbl.BorderBrush = Brushes.Yellow;
            }
            else if (selectedPiece != null)
            {
                byte pieceColumn = (byte)Grid.GetColumn(selectedPiece);
                byte pieceRow= (byte)Grid.GetRow(selectedPiece);
                bool canMove = gameBoard.CanMove(pieceRow, pieceColumn, row, column) ;

                if (canMove)
                {
                    gameBoard.MovePiece(pieceRow, pieceColumn, row, column);
                    DeselectExisting();
                    RefreshBoard();
                }
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnQuitGame.IsEnabled = false;
        }
        
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            cmbPlayer1Color.IsEnabled = false;
            cmbPlayer2Color.IsEnabled = false;
            isStarted = true;
            btnNewGame.IsEnabled = false;
            btnQuitGame.IsEnabled = true;
            gameBoard.BattleLog.Clear();
            InitBoard();
            RefreshBoard();
        }

        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            btnNewGame.IsEnabled = true;
            btnQuitGame.IsEnabled = false;
            cmbPlayer1Color.IsEnabled = true;
            cmbPlayer2Color.IsEnabled = true;
            isStarted = false;
        }
    }
}
