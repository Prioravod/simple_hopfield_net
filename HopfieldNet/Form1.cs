using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HopfieldNet
{
    public partial class Form1 : Form
    {
        List<int[]> numb = new List<int[]>(0);
        List<char> letter = new List<char>(0);
        public static int[,] mas;
        public static int[] falseArr;

        public Form1()
        {
            InitializeComponent();            
        }        

        private void digitButtonClick(object sender, EventArgs e)
        {
            Button x = (sender as Button);
            if (x.BackColor == Color.MediumAquamarine) x.BackColor = Color.DarkSlateGray;
            else x.BackColor = Color.MediumAquamarine;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char L = e.KeyChar;
            if (L < 'A' || L > 'Z')
            {
                e.Handled = true;
            }
        }

        private void createNew_Click(object sender, EventArgs e)
        {
            int[] arr = new int[30];
            char L = 'U';
            if (textBox1.Text != null && textBox1.Text != "")
            {
                L = textBox1.Text[0];
            }            
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    for(int i = 0; i < 30; i++)
                    {
                        if (control.Name == "button" + (i+1))
                        {
                            if (control.BackColor == Color.DarkSlateGray) arr[i] = 1; else arr[i] = -1;
                            control.BackColor = Color.MediumAquamarine;
                            break;
                        } 
                    }
                }                
            }
            string newStr = "arr" + L + " = { ";
            for (int i = 0; i < 30; i++)
            {
                newStr += arr[i] + "; ";
            }
            newStr += "}";
            numb.Add(arr);
            letter.Add(L);
            MessageBox.Show(newStr);
            newStr = "";
            textBox1.Text = "";
        }

        private void showAll_Click(object sender, EventArgs e)
        {
            int len = numb.Count;
            int[] arr;
            for (int i = 0; i < len; i++)
            {
                string newStr = letter[i] + " = { ";
                arr = numb[i];
                for (int k = 0; k < 30; k++)
                {
                    newStr += arr[k] + "; ";
                }
                newStr += "}";
                MessageBox.Show(newStr);
                newStr = "";
            }
            
        }

        private void button32_Click(object sender, EventArgs e)
        {
            int[] arr = new int[30];
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        if (control.Name == "button" + (i + 1))
                        {
                            if (control.BackColor == Color.DarkSlateGray) arr[i] = 1; else arr[i] = -1;
                            control.BackColor = Color.MediumAquamarine;
                            break;
                        }
                    }
                }
            }
            string newStr = "arr" + " = { ";
            for (int i = 0; i < 30; i++)
            {
                newStr += arr[i] + "; ";
            }
            newStr += "}";
            falseArr = arr;
            MessageBox.Show(newStr);
            newStr = "";
            Process();
        }
        public void GenMass(int n, int m)
        {
            MessageBox.Show("Generate");
            mas = new int[n, m];
            int i = 0, j = 0;
            for (i = 0; i < n; i++)
                for (j = 0; j < m; j++)
                    mas[i, j] = numb[i][j];
        }
        private void Process()
        {
            int n = numb.Count; //кол-во строк
            int m = numb[0].Length;//кол-во символов
            if (numb.Count != 0 && numb[0].Length != 0)
            {
                //
                label4.ForeColor = System.Drawing.Color.Black;
                richTextBox1.Text = "";
                //
                int i = 0, j = 0;
                string s = "";
                GenMass(n,m);
                //
                s = "Исходный массив:\n";
                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < m; j++)
                        s += String.Format("{0,3}", mas[i, j]);
                    s += "\n";
                }
                s += "\nMассив Y:\n";
                for (i = 0; i < m; i++)
                    s += String.Format("{0,3}", falseArr[i]);

                //
                int[,] w = new int[m, m];//
                int[,] tm = new int[m, m];//Временный массив
                int k = 0;

                s += "\nПоиск значения W:\n";
                s += "Транспонирование матрицы...\n";

                //Нахожу W
                for (k = 0; k < n; k++)
                {
                    //Транспонирую матрицу
                    for (i = 0; i < m; i++)
                        for (j = 0; j < m; j++)
                            tm[i, j] = mas[k, i] * mas[k, j];

                    s += "\nТ(" + (k + 1) + "):\n";
                    for (i = 0; i < m; i++)
                    {
                        for (j = 0; j < m; j++)
                            s += String.Format("{0,3}", tm[i, j]);
                        s += "\n";
                    }


                    //Нахожу сумму tm[i]
                    for (i = 0; i < m; i++)
                        for (j = 0; j < m; j++)
                            w[i, j] += tm[i, j];
                }

                s += "\nПолученная матрица W:\n";
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < m; j++)
                        s += String.Format("{0,3}", w[i, j]);
                    s += "\n";
                }


                //Обнуляю главную диагональ w
                for (i = 0; i < m; i++) w[i, i] = 0;


                s += "\nОбнуление главной диагонали w:\n";
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < m; j++)
                        s += String.Format("{0,3}", w[i, j]);
                    s += "\n";
                }
                s += "\n--- Поиск совпадений ---\n";

                int[] a = new int[m];//ф-ия активации
                bool b = false; k = 0;//Счётчик
                int l = -1;//Индекс подходящей строки
                while (!b && k < 200)
                {
                    s += "\n--" + (k + 1) + "--\n";
                    //Вычисляю y'
                    for (i = 0; i < m; i++)
                        for (j = 0; j < m; j++)
                            a[i] += w[i, j] * falseArr[j];

                    s += "\nВычисление y' \n";
                    for (i = 0; i < m; i++)
                        s += String.Format("{0,3}", a[i]);

                    //Обрабатываю y' ф-ей активации
                    for (i = 0; i < m; i++)
                        if (a[i] >= 0) a[i] = 1;
                        else a[i] = -1;

                    s += "\nОбработка y' ф-ей активации\n";
                    for (i = 0; i < m; i++)
                        s += String.Format("{0,3}", a[i]);
                    s += "\nПоиск совпадений\n";
                    //Сравниваю
                    bool st = false;
                    for (i = 0; i < n; i++)
                    {
                        st = true;
                        for (j = 0; j < m; j++)
                            if (mas[i, j] != a[j]) { st = false; break; }
                        if (st) { l = i; break; }
                    }
                    if (l != -1)
                    {
                        b = true;
                        s += "\nСовпадение найдено\n";
                    }
                    else
                    {
                        s += "\nСовпадение НЕ найдено\n";
                        k++;//Увеличиваю счетчик
                        //Обновляем у'
                        for (i = 0; i < m; i++)
                            falseArr[i] = a[i];
                        //Обнуляем a
                        for (i = 0; i < m; i++)
                            a[i] = 0;

                        s += "\nПолучение нового y'\n";
                        for (i = 0; i < m; i++)
                            s += String.Format("{0,3}", falseArr[i]);
                    }
                }
                s += "\n--- Поиск совпадений завершен ---\n";
                if (l != -1)
                {
                    string s2 = "";
                    for (j = 0; j < m; j++)
                        s2 += String.Format("{0,3}", a[j]);
                    label4.Text = "Найдено совпадение с X=" + (l + 1).ToString() + "\n" + s2 + "\nГлубина поиска:" + (k + 1).ToString();
                    s += "\nНайдено совпадение с X=" + (l + 1).ToString() + "\n" + s2 + "\nГлубина поиска:" + (k + 1).ToString();
                }
                else
                {
                    if (k == 200) { label4.ForeColor = System.Drawing.Color.Red; label4.Text = "Зацикливание"; }
                    s += "\nНет совпадений";
                }
                richTextBox1.Text = s;
            }

        }
    }   
}
