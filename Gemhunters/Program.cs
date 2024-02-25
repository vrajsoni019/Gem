using System;
using static System.Net.WebRequestMethods;
using System.Numerics;
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
class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }
    public char LastMove { get; set; }
    public Player(string name, Position position)
    {
        Name = name; //name of the player1 and player 2 as P1 and P2
        Position = position; //initial position parameter value
        GemCount = 0; //initial GemCount counter to set at "0"
        LastMove = '-'; //initial value of Last move to be set as "-"
    }
    public void Move(char direction)  //declaration of PUBLIC CLASS Move, with single parameter "direction"
    {
        int newX = Position.X; //creating integer variable for accepting postition values

        int newY = Position.Y; //creating integer variable for accepting postition values
        if (direction == 'U') newY--; //U is UP it decrements newY and player moves UP on the board
        else if (direction == 'D') newY++; //D is DOWN it increments newY and player moves DOWN on the board
        else if (direction == 'L') newX--; //L is LEFT it decrements newX and player moves LEFT on the board
        else if (direction == 'R') newX++; //R is RIGHT it increments newX and player moves RIGHR on the board
        Position = new Position(newX, newY);
        LastMove = direction;
    }
}
class Cell
{
    public string Occupant { get; set; }
    public Cell()
    {
        Occupant = "-";
    }
}
class Board
{
    public Cell[,] Grid { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    private Random rand = new Random();
    public Board()
    {
        Grid = new Cell[6, 6];
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                Grid[x, y] = new Cell();
            }
        }
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        InitializeBoard();
    }
    public void Display()
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                string occupant = Grid[x, y].Occupant;
                if (x == Player1.Position.X && y == Player1.Position.Y)
                {
                    occupant = Player1.Name;
                }
                else if (x == Player2.Position.X && y == Player2.Position.Y)
                {
                    occupant = Player2.Name;
                }
                Console.Write(occupant.PadRight(3)); // Add spacing
            }
            Console.WriteLine();
        }
    }
    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;
        if (direction == 'U') newY--;
        else if (direction == 'D') newY++;
        else if (direction == 'L') newX--;
        else if (direction == 'R') newX++;
        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
            return false;

        if (Grid[newX, newY].Occupant == "O")
            return false;
        return true;
    }
    public void CollectGem(Player player)
    {
        int x = player.Position.X;
        int y = player.Position.Y;
        if (Grid[x, y].Occupant == "G")
        {
            player.GemCount++;
            Grid[x, y].Occupant = "-";
        }
    }
    private void InitializeBoard()
    {
        // Initialize board with obstacles "O"
        for (int i = 0; i < 10; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(6);
                y = rand.Next(6);
            } while (Grid[x, y].Occupant != "-");
            Grid[x, y].Occupant = "O";
        }
        // Place gems "G" randomly
        for (int i = 0; i < 5; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(6);
                y = rand.Next(6);
            } while (Grid[x, y].Occupant != "-");
            Grid[x, y].Occupant = "G";
        }
    }
}
class Game
{
    private Board Board { get; }
    private Player Player1 { get; }
    private Player Player2 { get; }
    private Player CurrentTurn { get; set; }
    private int TotalTurn { get; set; }
    public Game()
    {
        Board = new Board();
        Player1 = Board.Player1;
        Player2 = Board.Player2;
        CurrentTurn = Player1;
        TotalTurn = 0;
    }
    public void Start()
    {
        Console.WriteLine("Welcome to Gem Hunters!");
        while (!IsGameOver())
        {
            Console.WriteLine($"Current Turn: {CurrentTurn.Name}");
            Board.Display();
            Console.WriteLine($"Previous Move: {CurrentTurn.LastMove}");
            Console.WriteLine($"Gems Collected by {Player1.Name}:{Player1.GemCount} ");
            Console.WriteLine($"Gems Collected by {Player2.Name}:{Player2.GemCount} ");
            Console.Write("Enter your move (U/D/L/R): ");

            char move = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
                Board.CollectGem(CurrentTurn);
                TotalTurn++;
                if (Board.Player1.GemCount + Board.Player2.GemCount == 5)
                {
                    // All gems are collected, end the game
                    break;
                }
                SwitchTurn();
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
        }
        AnnounceWinner();
    }
    private bool IsValidMove(Player player, char direction)
    {
        return Board.IsValidMove(player, direction);
    }
    private void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }
    private bool IsGameOver()
    {
        return TotalTurn >= 30;
    }
    private void AnnounceWinner()
    {
        int player1Gems = Player1.GemCount;
        int player2Gems = Player2.GemCount;
        if (player1Gems > player2Gems)
        {
            Console.WriteLine($"Player {Player1.Name} wins with {player1Gems} gems!");
        }
        else if (player2Gems > player1Gems)
        {
            Console.WriteLine($"Player {Player2.Name} wins with {player2Gems}gems!");
        }
        else
        {
            Console.WriteLine("It's tie!");
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