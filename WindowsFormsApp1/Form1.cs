using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// З16-ИВТ, Мамаков А.В.
// Вариант №9
// 46 символов (А…Я, пробел, «,», «.», цифры 0…9 ), методы 3 и 9
// M=
//

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Аффинная система подстановок Цезаря (3):
        int A, K, M; // А, К - ключи, М - длина словаря
        int[] SubstTable; // Таблица  кодов для аффинных подстановок I(J)
        string Alphabet; // Алфавит
        Dictionary<char, char> EnCodingTable; // Словарь символов для прямой подстановки - кодирования (Исх. символ, Код. символ)
        Dictionary<char, char> DeCodingTable; // Словарь символов для обратной подстановки - декодировния (Код. символ, Исх. символ)

        // Метод шифрующих таблиц с одиночной перестановкой по ключу (9)


        public Form1()
        {
            InitializeComponent();
        }

        // Функция вычисляющяя НОД двух чисел
        public int GetNOD(int val1, int val2)
        {
            if (val2 == 0)
                return val1;
            else
                return GetNOD(val2, val1 % val2);
        }

        private bool ValidateASSCKeys() // Проверка:  0 ≤ (A, J)≤ (M-1),  0 ≤ K ≤ (M-1),  НОД (A, M)=1
        { //A, K, M
            Hint.Hide(edA);
            Hint.Hide(edK);
            if (A >= M)
            {
                Hint.Show("A должно быть меньше длины алфавита!", edA, 0, 0);
                //PointToClient(new Point(edA.Left, edA.Top))
                return false;
            }
            if (K >= M)
            {
                Hint.Show("K должно быть меньше длины алфавита!", edK, 0, 0);
                return false;
            }
            if (GetNOD(A, M) != 1)
            {
                Hint.Show("A и М должны иметь наименьший общий делитель = 1!", edA, 0, 0);
                return false;
            }
            return true;
        }

        // Инициализируем A, K, M
        public void Init()
        {
            A = Decimal.ToInt32(edA.Value);
            K = Decimal.ToInt32(edK.Value);
            M = Decimal.ToInt32(edAlphabet.Text.Length);
            lblM.Text = "M = " + M.ToString();
            if (!ValidateASSCKeys()) // Проверка:  0 ≤ (A, J)≤ (M-1),  0 ≤ K ≤ (M-1),  НОД (A, M)=1
                return;
            // Алфавит:
            Alphabet = edAlphabet.Text;
            // Инициализация таблицу кодов для аффинных подстановок при A, K, M 
            SubstTable = new int[M];
            for (int j = 0; j < M; j++) // I = (А∙J+K) mod M
                SubstTable[j] = (A * j + K) % M;
            // Отобразим таблицу в логе:
            textLog.Text += "таблица кодов для аффинных подстановок при A, K, M" + Environment.NewLine;
            for (int j = 0; j < M; j++)
                textLog.Text += string.Format("{0,2}", j) + "|";
            textLog.Text += Environment.NewLine;
            for (int j = 0; j < M; j++)
                textLog.Text += string.Format("{0,2}", SubstTable[j]) + "|";
            //
            // Инициализируем словарь символов для афинных подстановок
            EnCodingTable = new Dictionary<char, char>();
            DeCodingTable = new Dictionary<char, char>();
            for (int j = 0; j < M; j++)
            {
                EnCodingTable.Add(Alphabet[j], Alphabet[SubstTable[j]]);
                DeCodingTable.Add(Alphabet[SubstTable[j]], Alphabet[j]);
            }
            // Log
            textLog.Text += Environment.NewLine + "словарь символов для афинных подстановок" + Environment.NewLine;
            foreach (var kvp in EnCodingTable)
                textLog.Text += string.Format("('{0}'-'{1}') ", kvp.Key, kvp.Value);
        }

        private string EncodeASSC(string str)
        {
            string res = "";
            foreach (char c in str)
            {
                // Проверить, есть симвоол в алфавите!
                if (!EnCodingTable.ContainsKey(c))
                    return "";
                // Перекодировать символ и добавить в результат!
                res += EnCodingTable[c];
            }
            return res;
        }

        private string DecodeASSC(string str)
        {
            string res = "";
            foreach (char c in str)
            {
                // Проверить, есть симвоол в алфавите!
                if (!DeCodingTable.ContainsKey(c))
                    return "";
                // Перекодировать символ и добавить в результат!
                 res += DeCodingTable[c];
            }
            return res;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            textBox2.Text = EncodeASSC(textBox1.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            textBox2.Text = DecodeASSC(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}
