using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace Snake
{
    class Program
    {

        static List<int[]> snake = new List<int[]>();
        static int[] apple = new int[2];

        static int score = 0;

        static Random random = new Random();
        static int direction = 0;

        static void startGame()
        {
            int[] center = { WindowWidth / 2, WindowHeight / 2 };
            snake.Add(center);

            CursorVisible = false;
            SetCursorPosition(WindowWidth / 2 - 10, WindowHeight / 2);
            Write("Press space to start");
            while (ReadKey(true).Key != ConsoleKey.Spacebar) { /*Wait until space is pressesd*/ }
            drawBorder();
            generateApple();
            gameLoop();
        }

        static void gameOver()
        {
            Clear();
            SetCursorPosition(WindowWidth / 2 - 4, WindowHeight / 2 - 1);
            Write("Game Over");
            SetCursorPosition(WindowWidth / 2 - ("Your score: ".Length + Convert.ToString(score).Length) / 2, WindowHeight / 2 + 1);
            Write("Your score: {0}", score);
            SetCursorPosition(WindowWidth / 2 - "Press R to restart".Length / 2 + 1, WindowHeight / 2 + 2);
            Write("Press R to restart");
            while (ReadKey(true).Key != ConsoleKey.R) { /*Wait until R is pressesd*/ }
            restart();
        }

        static void restart()
        {
            snake = new List<int[]>();
            score = 0;
            direction = 0;
            int[] center = { WindowWidth / 2, WindowHeight / 2 };
            snake.Add(center);
            drawBorder();
            generateApple();
            gameLoop();
        }

        static void gameLoop()
        {
            getDirection();
            updateSnake();
            Thread.Sleep(100);
            gameLoop();
            
        }

        static bool isGameOver()
        {
            if (snake[0][0] == 0 || snake[0][0] == WindowWidth - 2) //hit side
            {
                return true;
            }
            else if (snake[0][1] == 0 || snake[0][1] == WindowHeight-1) //hit top or bottom 
            {
                return true;
            }
            for (int i = 1; i < snake.Count; i++) //ran into itself
            {
                if (snake[0][0] == snake[i][0] && snake[0][1] == snake[i][1])
                {
                    return true;
                }
            }

            return false;
        }

        static void updateSnake()
        {
            int[] head = getNext(direction);
            if (head != null)
            {
                snake.Insert(0, head);
                SetCursorPosition(head[0], head[1]);
                Write("██");
                if (!checkForApple() && snake.Count > 3)
                {
                    SetCursorPosition(snake.Last<int[]>()[0], snake.Last<int[]>()[1]);

                    Write("  ");
                    if (isGameOver())
                        gameOver();
                    snake.Remove(snake.Last());
                }
            }
        }

        static void generateApple()
        {
            apple[0] = random.Next(1, (WindowWidth - 1) / 2) * 2;
            apple[1] = random.Next(1, WindowHeight - 1);
            for (int i = 0; i < snake.Count; i++)
            {
                if (apple[0] == snake[i][0] && apple[1] == snake[i][1])
                {
                    generateApple();
                }
            }
            SetCursorPosition(apple[0], apple[1]);
            Write("██", snake.Count);
        }

        static bool checkForApple()
        {
            if (apple[0] == snake[0][0] && apple[1] == snake[0][1])
            {
                generateApple();
                score++;
                return true;
            }
            else
            {
                return false;
            }
        }

        static int[] getNext(int dir)
        {
            switch (dir)
            {
                case 0:
                    int[] up = { snake[0][0], snake[0][1] - 1 };
                    return up;
                case 1:
                    int[] right = { snake[0][0] + 2, snake[0][1] };
                    return right;
                case 2:
                    int[] down = { snake[0][0], snake[0][1] + 1 };
                    return down;
                case 3:
                    int[] left = { snake[0][0] - 2, snake[0][1] };
                    return left;
                default:
                    return null;
            }
        }

        static void drawBorder()
        {
            Clear();

            for (int i = 0; i < WindowHeight; i++)
            {
                Write("██");
                SetCursorPosition(WindowWidth - 2, CursorTop);
                Write("██");
            }
            SetCursorPosition(0, 0);
            for (int i = 0; i < WindowWidth; i++)
            {
                Write("█");
            }
            SetCursorPosition(0, WindowHeight - 1);
            for (int i = 0; i < WindowWidth; i++)
            {
                Write("█");
            }
            SetCursorPosition(0, 0);
        }

        static void getDirection()
        {
            if (KeyAvailable)
            {
                ConsoleKeyInfo keyinfo = ReadKey(true);
                switch (keyinfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != 2)
                            direction = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != 3)
                            direction = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != 0)
                            direction = 2;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != 1)
                            direction = 3;
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            startGame();
        }
    }
}
