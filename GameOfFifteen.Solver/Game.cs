using System;

namespace GameOfFifteen.Solver
{
    class Game
    {
        int[,] map; //дошка
        int[] dx = new int[] { 0, -1, 0, 1 }; //зміщення 0
        int[] dy = new int[] { 1, 0, -1, 0 };
        char[] move_desc = new char[] { 'D', 'L', 'U', 'R' }; //можливі ходи
        int[] opposite_move = new int[] { 2, 3, 0, 1 }; //протилежні ходи (2, 3, 0, 1)                                                       
        const int infinity = 10000;
        int[] goalX = new int[16]; //goalX[i] - координата x i-й пятнашки, ...
        int[] goalY = new int[16];

        int minPrevIteration; //минимум стоимости среди нерассмотренных узлов

        //конструктор поля
        public Game(int[,] map)
        {
            this.map = map;         
        }   

        #region Алгоритм IDA* та пошук вглиб для знаходження найкоротшого шляху
        public string Solve()
        {
            initGoalArrays();
            string result = "";
            idaStar(out result);
            return result;
        }

        //ініціалізує цільовий стан
        private void initGoalArrays()
        {
            for (int i = 0; i < 15; i++)
            {
                goalX[i + 1] = i % 4;
                goalY[i + 1] = i / 4;
                goalX[0] = 4;
                goalY[0] = 4;
            }
        }

        //змінює місцями дві пятнашки
        private void swap(int y1, int x1, int y2, int x2)
        {
            int value1, value2;
            value1 = map[y1, x1];
            value2 = map[y2, x2];
            map[y1, x1] = value2;
            map[y2, x2] = value1;
        }

        //евристична оціночна функція Манхеттенська відстань
        private void estimate(out int manhattan)
        {
            int i, j, value, m;
            manhattan = 0;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    value = map[i, j];
                    if (value > 0)
                    {
                        m = Math.Abs(i - goalY[value]) + Math.Abs(j - goalX[value]);
                        manhattan = manhattan + m;
                    }
                }
            }
        }

        //пошук вглиб з обрізанням f=g+h < deepness
        private void recSearch(int g, int previousMove, int x0, int y0, int step, int deepness, out bool tf,
                        out string resultString)
        {
            int h;
            int manhattan;
            estimate(out manhattan);
            h = manhattan;
            tf = false;
            resultString = null;
            step = 0;
            //h = мінімум ходів до цілі
            if (h == 0)
            {
                tf = true;
                return; //якщо ціль
            }
            //якщо те, що ми пройшли (g) + те, що нам як мынымум залишилось (h) > допустимої глибини - вихід.
            int f;
            f = g + h;
            if (f > deepness)
            {
                //знаходимо мінімум вартості серед "обрізаних" вузлів
                if (minPrevIteration > f)
                    minPrevIteration = f;
                return;
            }
            int newx, newy;
            bool res;
            //робимо всі можливі ходи
            for (int i = 0; i < 4; i++)
            {
                if (opposite_move[i] != previousMove)
                {
                    //нові координати пустої клітинки
                    newx = x0 + dx[i];
                    newy = y0 + dy[i];
                    if ((newy <= 3) && (newy >= 0) && (newx <= 3) && (newx >= 0))
                    {
                        swap(y0, x0, newy, newx); //рухаємо пусту клітинку на нове місце
                        recSearch(g + 1, i, newx, newy, step, deepness, out tf, out resultString); //рекурсивний пошук з нової позиції
                        res = tf;
                        swap(y0, x0, newy, newx); //повертаємо пусту клітинку назад
                        if (res == true) //якби було знайдене рішення
                        {
                            resultString = move_desc[i] + resultString; //записуємо цей хід
                            step++;
                            tf = true;
                            return; //і виходимо
                        }
                    }
                }
            }
            return; //ціль не знайшли
        }

        //ітерація глибини та IDA*
        private bool idaStar(out string result)
        {
            bool res;
            int x0 = -1;
            int y0 = -1;
            bool tf;
            int step;
            string resultString;
            res = false;
            result = null;
            int manhattan;
            estimate(out manhattan);
            int deepness = manhattan; //починаємо з h для початкового стану
            while (deepness <= 50)
            {
                minPrevIteration = infinity; //ініціалізація для пошуку мінімуму
                                             //пошук пустої клітинки
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (map[i, j] == 0)
                        {
                            x0 = j;
                            y0 = i;
                        }
                    }
                }
                step = 0;
                recSearch(0, -1, x0, y0, step, deepness, out tf, out resultString);
                deepness = minPrevIteration;
                res = tf;
                if (res)
                {
                    result = resultString;
                    break;
                }
            }
            return res;
        }
        #endregion
    }
}