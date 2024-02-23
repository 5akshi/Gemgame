using System;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}
// IDENTIFY THE PLAYER 
class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; }


    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        int newX = Position.X;
        int newY = Position.Y;

        switch (direction)
        {
            case 'U':
                newX--;
                break;
            case 'D':
                newX++;
                break;
            case 'L':
                newY--;
                break;
            case 'R':
                newY++;
                break;
            default:
                Console.WriteLine("Invalid direction. Please enter U, D, L, or R.");
                break;
        }

        if (newX < 0) newX = 0;
        if (newX > 5) newX = 5;
        if (newY < 0) newY = 0;
        if (newY > 5) newY = 5;

        Position = new Position(newX, newY);
    }
}

class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

class Board
{
    private Cell[,] Grid;

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Initialize empty cells
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }

        // Initialize players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";
         
        // Initialize gems (random positions)
        Random random = new Random();
        for (int i = 0; i < 3; i++)
        {
            int x = random.Next(0, 6);
            int y = random.Next(0, 6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "G";
            }
        }

        // Initialize obstacles (random positions)
        for (int i = 0; i < 5; i++)
        {
            int x = random.Next(0, 6);
            int y = random.Next(0, 6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "O";
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        Position position = player.Position;
        int newX = position.X;
        int newY = position.Y;

        switch (direction)
        {
            case 'U':
                newX--;
                break;
            case 'D':
                newX++;
                break;
            case 'L':
                newY--;
                break;
            case 'R':
                newY++;
                break;
            default:
                return false;
        }

        bool isValidMove = true;

        if (newX < 0) newX = 0;
        if (newX > 5) newX = 5;
        if (newY < 0) newY = 0;
        if (newY > 5) newY = 5;

        // Check if new position is an obstacle
        if (Grid[newX, newY].Occupant == "O")
        {
            isValidMove = false;
        }

        if (isValidMove)
        {
            Grid[position.X, position.Y].Occupant = "-";
        }

        return isValidMove;
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
        }
        Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
    }
}

class Game
{
    private Board Board;
    private Player Player1;
    private Player Player2;
    private Player CurrentTurn;
    private int TotalTurns;

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        Console.WriteLine("Gem Hunters Game\n");
        Console.WriteLine("Welcome to the Game!\n");
        while (!IsGameOver())
        {
            Console.WriteLine($"Turn {TotalTurns + 1}: {CurrentTurn.Name}'s turn");
            Board.Display();

            Console.Write("Enter direction (U/D/L/R): ");
            char direction = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);
                TotalTurns++;
                SwitchTurn();
            }
            else
            {
                //Console.WriteLine("Invalid move. Please try again.");
            }
        }

        AnnounceWinner();
    }

    private void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    private bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        Console.WriteLine("\nGame Over!");
        Console.WriteLine($"Player 1 gems: {Player1.GemCount}");
        Console.WriteLine($"Player 2 gems: {Player2.GemCount}");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (Player2.GemCount > Player1.GemCount)
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}

