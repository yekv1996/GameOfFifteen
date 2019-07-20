using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfFifteen.Shuffler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введiть к-ть перемiщень");
            int numberofmovements;            
            if (Int32.TryParse(Console.ReadLine(), out numberofmovements))
            {
                int[,] board = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };
                int[] currentposition = { 3, 3 };
                Random rand = new Random();
                bool ismoved;
                int currentvalue = -1; 
                for (int i = 0; i < numberofmovements; i++)
                {
                    ismoved = false;
                    while (!ismoved)
                    {
                        switch (rand.Next(4))
                        {
                            case 0:
                                {
                                    if (currentposition[1] != 0 && currentvalue != 1)
                                    {
                                        board[currentposition[0], currentposition[1]] = board[currentposition[0], --currentposition[1]];
                                        board[currentposition[0], currentposition[1]] = 0;
                                        currentvalue = 0;
                                        ismoved = true;
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    if (currentposition[1] != 3 && currentvalue != 0)
                                    {
                                        board[currentposition[0], currentposition[1]] = board[currentposition[0], ++currentposition[1]];
                                        board[currentposition[0], currentposition[1]] = 0;
                                        currentvalue = 1;
                                        ismoved = true;                                        
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    if (currentposition[0] != 0 && currentvalue != 3)
                                    {
                                        board[currentposition[0], currentposition[1]] = board[--currentposition[0], currentposition[1]];
                                        board[currentposition[0], currentposition[1]] = 0;
                                        currentvalue = 2;
                                        ismoved = true;
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    if (currentposition[0] != 3 && currentvalue != 2)
                                    {
                                        board[currentposition[0], currentposition[1]] = board[++currentposition[0], currentposition[1]];
                                        board[currentposition[0], currentposition[1]] = 0;
                                        currentvalue = 3;
                                        ismoved = true;
                                    }
                                    break;
                                }
                        }
                    }
                }
                string output = board[0, 0] + "," + board[0, 1] + "," + board[0, 2] + "," + board[0, 3] + "\n" + board[1, 0] + "," + board[1, 1] + "," + board[1, 2] + "," + board[1, 3] + "\n" + board[2, 0] + "," + board[2, 1] + "," + board[2, 2] + "," + board[2, 3] + "\n" + board[3, 0] + "," + board[3, 1] + "," + board[3, 2] + "," + board[3, 3] + "\n";
                Console.WriteLine(output);
                string path = Directory.GetCurrentDirectory() + "\\board.txt";
                Console.WriteLine(path);
                File.WriteAllText(path, output);
                Console.ReadKey();
            }
            else { }
        }
    }
}
