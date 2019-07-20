using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace GameOfFifteen.Solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int moves, move, interval;//к-ть ходів, поточний хід, інтервал(для програвача)        
        int[,] board;//дошка
        int[][,] playeroutput;// послідовність станів дошки для програвача
        System.Windows.Threading.DispatcherTimer dispatcherTimer; 
        public MainWindow()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (move < moves)
            {                
                label1.Content = playeroutput[move][0, 0];
                label2.Content = playeroutput[move][0, 1];
                label3.Content = playeroutput[move][0, 2];
                label4.Content = playeroutput[move][0, 3];
                label5.Content = playeroutput[move][1, 0];
                label6.Content = playeroutput[move][1, 1];
                label7.Content = playeroutput[move][1, 2];
                label8.Content = playeroutput[move][1, 3];
                label9.Content = playeroutput[move][2, 0];
                label10.Content = playeroutput[move][2, 1];
                label11.Content = playeroutput[move][2, 2];
                label12.Content = playeroutput[move][2, 3];
                label13.Content = playeroutput[move][3, 0];
                label14.Content = playeroutput[move][3, 1];
                label15.Content = playeroutput[move][3, 2];
                label16.Content = playeroutput[move][3, 3];
                move++;
            }
            else
            {
                dispatcherTimer.Stop();
            }
        }

        private void ButtonOpenFile_click(object sender, RoutedEventArgs e)
        {
            string input = "";
            move = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                input = File.ReadAllText(openFileDialog.FileName);
            board = new int[4, 4];
            string[] stringboard = input.Split('\n', ',');//текст з файлу в масив

            label1.Content = board[0, 0] = Convert.ToInt32(stringboard[0]);
            label2.Content = board[0, 1] = Convert.ToInt32(stringboard[1]);
            label3.Content = board[0, 2] = Convert.ToInt32(stringboard[2]);
            label4.Content = board[0, 3] = Convert.ToInt32(stringboard[3]);
            label5.Content = board[1, 0] = Convert.ToInt32(stringboard[4]);
            label6.Content = board[1, 1] = Convert.ToInt32(stringboard[5]);
            label7.Content = board[1, 2] = Convert.ToInt32(stringboard[6]);
            label8.Content = board[1, 3] = Convert.ToInt32(stringboard[7]);
            label9.Content = board[2, 0] = Convert.ToInt32(stringboard[8]);
            label10.Content = board[2, 1] = Convert.ToInt32(stringboard[9]);
            label11.Content = board[2, 2] = Convert.ToInt32(stringboard[10]);
            label12.Content = board[2, 3] = Convert.ToInt32(stringboard[11]);
            label13.Content = board[3, 0] = Convert.ToInt32(stringboard[12]);
            label14.Content = board[3, 1] = Convert.ToInt32(stringboard[13]);
            label15.Content = board[3, 2] = Convert.ToInt32(stringboard[14]);
            label16.Content = board[3, 3] = Convert.ToInt32(stringboard[15]);
                       
            Game game = null;
            game = new Game(board);
            string result = game.Solve();//результат у вигляды напрямкыв руху 0 LRUD
            int x = 0, y = 0;//координати нуля
            moves = result.Length;

            for (int i = 0; i <= 3; i++)//шукаэмо координати 0
                for (int j = 0; j <= 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        x = i;
                        y = j;
                    }
                }
            string output = "";
            playeroutput = new int[moves][,];
            int[,] tempboard = new int[4, 4];
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    tempboard[i, j] = board[i, j];
            int currentnumber;
            int m = 0;
            foreach (char c in result)//результат у масив для програвача та у стрынг для виведення у текстовий файл 
            {
                switch (c)
                {
                    case 'R':                        
                        tempboard[x, y] = currentnumber = tempboard[x, ++y];
                        tempboard[x, y] = 0;
                        output += currentnumber + ",→\n";//для текстового файлу
                        playeroutput[m] = new int[4,4];
                        for (int i = 0; i <= 3; i++)
                            for (int j = 0; j <= 3; j++)
                                playeroutput[m][i, j] = tempboard[i, j];//для програвача
                        
                        break;
                    case 'L':                        
                        tempboard[x, y] = currentnumber = tempboard[x, --y];
                        tempboard[x, y] = 0;
                        output += currentnumber + ",←\n";
                        playeroutput[m] = new int[4, 4];
                        for (int i = 0; i <= 3; i++)
                            for (int j = 0; j <= 3; j++)
                                playeroutput[m][i, j] = tempboard[i, j];
                        break;
                    case 'U':
                        tempboard[x, y] = currentnumber = tempboard[--x, y];
                        tempboard[x, y] = 0;
                        output += currentnumber + ",↑\n";
                        playeroutput[m] = new int[4, 4];
                        for (int i = 0; i <= 3; i++)
                            for (int j = 0; j <= 3; j++)
                                playeroutput[m][i, j] = tempboard[i, j];
                        break;
                    case 'D':                        
                        tempboard[x, y] = currentnumber = tempboard[++x, y];
                        tempboard[x, y] = 0;
                        output += currentnumber + ",↓\n";
                        playeroutput[m] = new int[4, 4];
                        for (int i = 0; i <= 3; i++)
                            for (int j = 0; j <= 3; j++)
                                playeroutput[m][i, j] = tempboard[i, j];
                        break;
                }
                m++;
            }        
            File.WriteAllText(Directory.GetCurrentDirectory() + @"/output.txt", output);
        }

        private void ButtonPlayer_click(object sender, RoutedEventArgs e)
        {
            interval = Convert.ToInt32(textBox_interval.Text);//інтервал у секундах
            dispatcherTimer.Interval = new TimeSpan(0, 0, interval);
            dispatcherTimer.Start();
        }
    }
}

