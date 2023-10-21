using System;

namespace SudokuWpf.ViewModel;

public class GameBoardViewModel
{
    public GameBoardViewModel(int[] gameBoard)
    {
        GameBoard = gameBoard;
    }

    public int[] GameBoard { get; set; }
    
}