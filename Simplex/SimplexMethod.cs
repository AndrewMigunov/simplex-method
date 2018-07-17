using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    class SimplexMethod
    {
        double[,] table;
        int n, m;
        List<int> basis;
        //double[] result { get; }
        public SimplexMethod(double[,] matrix, double[] freeColumn, double[] intFuncfion,bool[] isLess)
        {
            m = matrix.GetLength(0) + 1;
            n = matrix.GetLength(1) + 1;
            table = new double[m, n + m - 1];
            basis = new List<int>();
            for (int i = 0; i < m - 1; i++)
            {
                if (isLess[i])
                {
                    table[i, 0] = freeColumn[i];
                }
                else
                {
                    table[i, 0] = -freeColumn[i];
                }
            }
            table[m - 1, 0] = 0;
            for (int i = 1; i < n; i++)
            {
                table[m - 1, i] = -intFuncfion[i - 1];
            }
            for (int i = 0; i < m - 1; i++)
            {
                if (isLess[i])
                {
                    for (int j = 1; j < n; j++)
                    {
                        table[i, j] = matrix[i, j - 1];
                    }
                }
                else
                {
                    for (int j = 1; j < n; j++)
                    {
                        table[i, j] = -matrix[i, j - 1];
                    }
                }
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = n; j < table.GetLength(1); j++)
                {
                    table[i, j] = 0;
                }
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }
            n = table.GetLength(1);
        }
        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = -1;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }
            
            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }
        public double[,] Calculate(double[] result)
        {
            while (isLessNull())
            {
                createTable();
            }
            int mainCol, mainRow; 

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                if (mainRow == -1)
                {
                    throw new Exception("Решение не ограничено!");
                }
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }
        private void createTable()
        {
            int mainRow=-1;
            for(int i =0; i < m - 1; i++)
            {
                if (table[i, 0] < 0)
                {
                    mainRow = i;
                    break;
                }
            }
            for (int i = mainRow+1; i < m - 1; i++)
            {
                if(table[i,0]<0 && table[i, 0] < table[mainRow, 0])
                {
                    mainRow = i;
                }
            }
            int mainCol = -1;
            for(int i = 1; i < table.GetLength(1); i++)
            {
                if(table[mainRow,i]<0)
                {
                    mainCol = i;
                }
            }
            if (mainCol == -1)
            {
                throw new Exception("Система не совместна");
            }
            for (int i = mainCol + 1; i < table.GetLength(1); i++)
            {
                if (table[mainRow, i] < 0 && table[mainRow,i]<table[mainRow,mainCol])
                {
                    mainCol = i;
                }
            }
            
            basis[mainRow] = mainCol;

            double[,] new_table = new double[m, n];

            for (int j = 0; j < n; j++)
                new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

            for (int i = 0; i < m; i++)
            {
                if (i == mainRow)
                    continue;

                for (int j = 0; j < n; j++)
                    new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
            }
            table = new_table;
        }

        private bool isLessNull()
        {
            for (int i = 0; i < m - 1; i++)
            {
                if (table[i, 0] < 0)
                {

                    return true;
                }
            }
            return false;
        }

    }
}
