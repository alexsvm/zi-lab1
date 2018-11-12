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
#if DEBUG // Отобразим таблицу в логе:
            textLog.Text += "таблица кодов для аффинных подстановок при A, K, M" + Environment.NewLine;
            for (int j = 0; j < M; j++)
                textLog.Text += string.Format("{0,2}", j) + "|";
            textLog.Text += Environment.NewLine;
            for (int j = 0; j < M; j++)
                textLog.Text += string.Format("{0,2}", SubstTable[j]) + "|";
#endif
            // Инициализируем словарь символов для афинных подстановок
            EnCodingTable = new Dictionary<char, char>();
            DeCodingTable = new Dictionary<char, char>();
            for (int j = 0; j < M; j++)
            {
                EnCodingTable.Add(Alphabet[j], Alphabet[SubstTable[j]]);
                DeCodingTable.Add(Alphabet[SubstTable[j]], Alphabet[j]);
            }
#if DEBUG // Log
            textLog.Text += Environment.NewLine + "словарь символов для афинных подстановок" + Environment.NewLine;
            foreach (var kvp in EnCodingTable)
                textLog.Text += string.Format("('{0}'-'{1}') ", kvp.Key, kvp.Value);
#endif
        }

        private string EncodeASSC(string str) // Кодирование
        {
            string res = "";
            foreach (char c in str)
            {
                if (!EnCodingTable.ContainsKey(c)) // Проверить, есть симвоол в алфавите!
                    return "";
                res += EnCodingTable[c]; // Перекодировать символ и добавить в результат!
            }
            return res;
        }

        private string DecodeASSC(string str) // Декодирование
        {
            string res = "";
            foreach (char c in str)
            {
                if (!DeCodingTable.ContainsKey(c)) // Проверить, есть символ в алфавите!
                    return "";
                res += DeCodingTable[c]; // Перекодировать символ и добавить в результат!
            }
            return res;
        }

        // Кодирование по методу шифрующих таблиц с одиночной перестановкой по ключу
        private string EncodeCTSCh(string str, string pass) 
        {
            int pass_len = pass.Length; // Длина кодовой строки
            int R = str.Length / pass_len + 1;  // Кол-во строк таблицы
            int S = pass_len;                   // Кол-во столбцов таблицы
            char[,] ctable = new char[R, S]; // Таблица для перекодировки с исходной строкой
            Dictionary<char, int> key = new Dictionary<char, int>();
            // Инициализируем словарь перестановки по ключу:
            for (int i = 0; i < pass_len; i++)
                key[pass[i]] = i;
            var key_char_list = key.Keys.ToList(); // Получаем список символов в ключевой строке
            key_char_list.Sort(); // и сортируем его
#if DEBUG // Log--->
            foreach (var ch in key_char_list)
                textLog.Text += ch + "(" + key[ch] + ") ";
            textLog.Text += Environment.NewLine;
#endif// <---
            int idx;
            // Заполняем таблицу с исходным текстом поколоночно
            for (int j = 0; j < S; j ++)
                for(int i = 0; i < R; i++)
                {
                    idx = j * R + i;
                    if (idx < str.Length)
                        ctable[i, j] = str[idx];
                    else
                        //ctable[i, j] = ' ';
                        continue;
                }
#if DEBUG// Log--->
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < S; j++)
                    textLog.Text += ctable[i, j] + "|";
                textLog.Text += Environment.NewLine;
            }
#endif // <--
            string res = "";
            // Теперь считывем закодированное сообщение построчно в порядке колонок отсортированного ключа:
            for (int i = 0; i < R; i++)
                foreach (var ch in key_char_list)
                    res += ctable[i, key[ch]];
            return res;
        }

        // Декодирование по методу шифрующих таблиц с одиночной перестановкой по ключу
        private string DecodeCTSCh(string str, string pass)
        {
            int pass_len = pass.Length; // Длина кодовой строки
            int R = str.Length / pass_len + 1;  // Кол-во строк таблицы
            int S = pass_len;                   // Кол-во столбцов таблицы
            char[,] ctable = new char[R, S]; // Таблица для перекодировки с закодированной строкой
            Dictionary<char, int> key = new Dictionary<char, int>();
            // Инициализируем словарь перестановки по ключу:
            for (int i = 0; i < pass_len; i++)
                key[pass[i]] = i;
            var key_char_list = key.Keys.ToList(); // Получаем список символов в ключевой строке
            key_char_list.Sort(); // и сортируем его
#if DEBUG // Log--->
            foreach (var ch in key_char_list)
                textLog.Text += ch + "(" + key[ch] + ") ";
            textLog.Text += Environment.NewLine;
#endif// <---
            int idx;
            // Заполняем таблицу с закодированным текстом построчно
            for (int i = 0; i < R; i++)
                for (int j = 0; j < S; j++)
                {
                    idx = i * S + j;
                    if (idx < str.Length)
                        ctable[i, j] = str[idx];
                    else
                        //ctable[i, j] = ' ';
                        continue;
                }
#if DEBUG// Log--->
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < S; j++)
                    textLog.Text += ctable[i, j] + "|";
                textLog.Text += Environment.NewLine;
            }
#endif // <--
            string res = "";
            // Теперь считывем расшифрованное сообщение в порядке колонок отсортированного ключа:
            foreach (var ch in key_char_list)
                for (int i = 0; i < R; i++)
                    res += ctable[i, key[ch]];
            return res;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            //textBox2.Text = EncodeASSC(textBox1.Text);
            //textBox2.Text = EncodeCTSCh(textBox1.Text, "ПАР");
            textBox2.Text = EncodeCTSCh(EncodeASSC(textBox1.Text), edASSCkey.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            //textBox2.Text = DecodeASSC(textBox1.Text);
            //textBox2.Text = DecodeCTSCh(textBox1.Text, "ПАР");
            textBox2.Text = DecodeASSC(DecodeCTSCh(textBox1.Text, edASSCkey.Text));
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}
