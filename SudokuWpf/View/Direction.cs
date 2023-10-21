using System;

namespace SudokuWpf.View;

[Flags]
public enum Direction
{
    Left = 0b00000001,
    LeftUp = 0b00000011,
    Up = 0b00000010,
    RightUp = 0b00000110,
    Right = 0b00000100,
    RightDown = 0b00001100,
    Down = 0b00001000,
    LeftDown = 0b00001001
}