// See https://aka.ms/new-console-template for more information

/*
 
   1   2   3 
1  o | x | o
  ---+---+---
2  o | x | o 
  ---+---+---
3  x | o | x
  
  
  X = 2, O = 1
 
 */

using System.Text.RegularExpressions;


var moveCount = 0;

var win = 0;

const int fieldSize = 3;

var field = new[]
{
    new int[fieldSize],
    new int[fieldSize],
    new int[fieldSize]
};

while (win == 0)
{
    PrintField(field, fieldSize);
    var (coordX, coordY) = ReadMove(field);

    moveCount++;
    var firstPlayer = moveCount % 2 == 1;
    field[coordX][coordY] = firstPlayer ? 2 : 1;
    win = CheckWin(fieldSize, moveCount, field);
}

switch (win)
{
    case 1:
        Console.WriteLine("Выиграл первый игрок");
        break;
    case 2:
        Console.WriteLine("Выиграл второй игрок");
        break;
    default:
        Console.WriteLine("Ничья");
        break;
}


/**
 * 0 - игра продолжается
 * 1 - выиграл первый игрок
 * 2 - выиграл второй игрок
 * 3 - ничья
 */
static int CheckWin(int fieldSize, int moveCount, int[][] field)
{
    if (moveCount < fieldSize * 2 - 1)
    {
        return 0;
    }

    var win = CheckVertical(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckHorizontal(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckDiagonal(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    for (int x = 0; x < fieldSize; x++)
    {
        var filled = false;
        for (int y = 0; y < fieldSize; y++)
        {
            filled = field[x][y] != 0;
        }

        if (filled)
        {
            return 3;
        }
    }

    return 0;
}

static int CheckVertical(int fieldSize, int[][] field)
{
    for (var x = 0; x < fieldSize; x++)
    {
        var win = false;
        for (var y = 1; y < fieldSize; y++)
        {
            win = field[x][y - 1] == field[x][y];
        }

        if (win)
        {
            return field[x][0] == 2 ? 1 : 2;
        }
    }

    return 0;
}

static int CheckHorizontal(int fieldSize, int[][] field)
{
    for (var y = 0; y < fieldSize; y++)
    {
        var win = false;
        for (var x = 1; x < fieldSize; x++)
        {
            win = field[x - 1][y] == field[x][y];
        }

        if (win)
        {
            return field[0][y] == 2 ? 1 : 2;
        }
    }

    return 0;
}

static int CheckDiagonal(int fieldSize, int[][] field)
{
    var win = false;
    for (var x = 1; x < fieldSize; x++)
    {
        win = field[x][x] == field[x - 1][x - 1];
    }
    if (win)
    {
        return field[0][0] == 2 ? 1 : 2;
    }

    return 0;
}

static (int coordX, int coordY) ReadMove(int[][] field)
{
    while (true)
    {
        Console.WriteLine("Введите координаты по горизонтали и вертикали (от 1 до 3) через пробел:");

        var rawPlayerMove = Console.ReadLine();

        if (rawPlayerMove != null)
        {
            var matches = Regex.Match(rawPlayerMove, "(\\d) (\\d)");
            if (matches.Success)
            {
                var coordX = int.Parse(matches.Groups[1].Value) - 1;
                var coordY = int.Parse(matches.Groups[2].Value) - 1;

                if ((coordX is >= 0 and < fieldSize) && (coordY is >= 0 and < fieldSize))
                {
                    if (field[coordX][coordY] != 0)
                    {
                        Console.WriteLine("Клетка занята");
                    }
                    else
                    {
                        return (coordX, coordY);
                    }
                }
            }
        }

        Console.WriteLine("Попробуйте еще");
    }
}

static void PrintField(int[][] field, int fieldSize)
{
    Console.WriteLine("   1   2   3 ");
    for (var y = 0; y < fieldSize; y++)
    {
        Console.Write((y + 1) + "  ");
        for (var x = 0; x < fieldSize; x++)
        {
            var symbol = field[x][y] switch
            {
                2 => "X",
                1 => "O",
                _ => " "
            };
            Console.Write(symbol);
            if (x + 1 != fieldSize)
            {
                Console.Write(" | ");
            }
        }

        Console.WriteLine();
        if (y + 1 != fieldSize)
        {
            Console.WriteLine("  ---+---+---");
        }
    }

    // Console.WriteLine("1    |   |   ");
    // Console.WriteLine("  ---+---+---");
    // Console.WriteLine("2    |   |   ");
    // Console.WriteLine("  ---+---+---");
    // Console.WriteLine("3    |   |   ");
}