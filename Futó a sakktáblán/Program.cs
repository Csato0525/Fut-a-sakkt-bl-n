using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futó_a_sakktáblán
{
    class Program
    {
        #region Kiiras
        static void Kiiras(int matrix_merete, char[,] matrix, int honnan_sor, int honnan_oszlop, int hova_sor, int hova_oszlop)
        {
            Console.WriteLine();
            for (int i = 0; i < matrix_merete; i++)
            {
                for (int j = 0; j < matrix_merete; j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("{0} {1} {2} {3}", honnan_sor, honnan_oszlop, hova_sor, hova_oszlop);
            Console.WriteLine();
        }
        #endregion
        #region Irany
        static List<int> Irany(int sor, int oszlop, int matrix_merete, char[,] matrix)
        {
            sor = sor - 1;
            oszlop = oszlop - 1;
            matrix_merete = matrix_merete - 1;
            List<int> irany = new List<int>();
            if (sor != 0 && oszlop != 0)
            {
                if (matrix[sor - 1, oszlop - 1] != '+')
                {
                    irany.Add(2);
                }
            }
            if (sor != 0 && oszlop != matrix_merete)
            {
                if (matrix[sor - 1, oszlop + 1] != '+')
                {
                    irany.Add(1);
                }
            }
            if (sor != matrix_merete && oszlop != 0)
            {
                if (matrix[sor + 1, oszlop - 1] != '+')
                {
                    irany.Add(3);
                }
            }
            if (sor != matrix_merete && oszlop != matrix_merete)
            {
                if (matrix[sor + 1, oszlop + 1] != '+')
                {
                    irany.Add(4);
                }
            }
            return irany;
        }
        #endregion
        #region Futolepes
        static HashSet<(string, string)>Futolepes(int sor, int oszlop, char[,] matrix, int matrix_merete)
        {
            HashSet<(string, string)> futolepesek = new HashSet<(string, string)>();
            List<int> iranyok = Irany(sor, oszlop, matrix_merete, matrix);
            string honnan = $"{sor}{oszlop}";
            foreach (var elem in iranyok)
            {
                int sor_hely = sor - 1;
                int oszlop_hely = oszlop - 1;
                if (elem == 2)
                {
                    while (sor_hely >= 0 && oszlop_hely >= 0 && matrix[sor_hely, oszlop_hely] != '+')
                    {
                        futolepesek.Add((honnan, $"{sor_hely + 1}{oszlop_hely + 1}"));
                        sor_hely--;
                        oszlop_hely--;
                    }
                }
                else if (elem == 1)
                {
                    while (sor_hely >= 0 && oszlop_hely < matrix_merete && matrix[sor_hely, oszlop_hely] != '+')
                    {
                        futolepesek.Add((honnan, $"{sor_hely + 1}{oszlop_hely + 1}"));
                        sor_hely--;
                        oszlop_hely++;
                    }
                }
                else if (elem == 3)
                {
                    while (sor_hely < matrix_merete && oszlop_hely >= 0 && matrix[sor_hely, oszlop_hely] != '+')
                    {
                        futolepesek.Add((honnan, $"{sor_hely + 1}{oszlop_hely + 1}"));
                        sor_hely++;
                        oszlop_hely--;
                    }
                }
                else if (elem == 4)
                {
                    while (sor_hely < matrix_merete && oszlop_hely < matrix_merete && matrix[sor_hely, oszlop_hely] != '+')
                    {
                        futolepesek.Add((honnan, $"{sor_hely + 1}{oszlop_hely + 1}"));
                        sor_hely++;
                        oszlop_hely++;
                    }
                }
            }
            return futolepesek;
        }
        #endregion
        #region LegrovidebbUt
        static List<(int, int)> LegrovidebbUt(int honnan_sor, int honnan_oszlop, int hova_sor, int hova_oszlop, HashSet<(string, string)> futolepesek)
        {
            Queue<(int, int, List<(int, int)>)> sor = new Queue<(int, int, List<(int, int)>)>();
            HashSet<(int, int)> megvisgalt = new HashSet<(int, int)>();
            sor.Enqueue((honnan_sor, honnan_oszlop, new List<(int, int)> { (honnan_sor, honnan_oszlop) }));
            megvisgalt.Add((honnan_sor, honnan_oszlop));

            while (sor.Count > 0)
            {
                var (jelenlegi_sor, jelenlegi_oszlop, ut) = sor.Dequeue();

                if (jelenlegi_sor == hova_sor && jelenlegi_oszlop == hova_oszlop)
                {
                    return ut;
                }

                foreach (var lepes in futolepesek)
                {
                    int kovetkezo_sor = int.Parse(lepes.Item2[0].ToString());
                    int kovetkezo_oszlop = int.Parse(lepes.Item2[1].ToString());

                    if (lepes.Item1 == $"{jelenlegi_sor}{jelenlegi_oszlop}" && !megvisgalt.Contains((kovetkezo_sor, kovetkezo_oszlop)))
                    {
                        megvisgalt.Add((kovetkezo_sor, kovetkezo_oszlop));
                        var uj_ut = new List<(int, int)>(ut);
                        uj_ut.Add((kovetkezo_sor, kovetkezo_oszlop));
                        sor.Enqueue((kovetkezo_sor, kovetkezo_oszlop, uj_ut));
                    }
                }
            }

            return null;
        }
        #endregion
        static void Main(string[] args)
        {
            int matrix_merete = int.Parse(Console.ReadLine());
            char[,] matrix = new char[matrix_merete, matrix_merete];
            string sor;
            for (int i = 0; i < matrix_merete; i++)
            {
                sor = Console.ReadLine();
                for (int j = 0; j < matrix_merete; j++)
                {
                    matrix[i, j] = sor[j];
                }
            }
            string honnan_hova = Console.ReadLine();
            string[] honnan_hova_tomb = honnan_hova.Split(' ');
            int honnan_sor = int.Parse(honnan_hova_tomb[0]);
            int honnan_oszlop = int.Parse(honnan_hova_tomb[1]);
            int hova_sor = int.Parse(honnan_hova_tomb[2]);
            int hova_oszlop = int.Parse(honnan_hova_tomb[3]);
            //Kiiras(matrix_merete,matrix,honnan_sor,honnan_oszlop,hova_sor,hova_oszlop);
            HashSet<(string, string)> futolepesek = Futolepes(honnan_sor, honnan_oszlop, matrix, matrix_merete);
            Queue<string> vizsgalando = new Queue<string>();
            foreach (var elem in futolepesek)
            {
                vizsgalando.Enqueue(elem.Item2);
            }
            HashSet<string> marVizsgaltak = new HashSet<string>();
            while (vizsgalando.Count() > 0)
            {
                string hely = vizsgalando.Dequeue();
                if (!marVizsgaltak.Contains(hely))
                {
                    marVizsgaltak.Add(hely);
                    foreach (var item in Futolepes(int.Parse(hely[0].ToString()), int.Parse(hely[1].ToString()), matrix, matrix_merete))
                    {
                        futolepesek.Add((item.Item1, item.Item2));
                        vizsgalando.Enqueue(item.Item2);
                    }
                }
            }
            var legrovidebb_ut = LegrovidebbUt(honnan_sor, honnan_oszlop, hova_sor, hova_oszlop, futolepesek);
            if (legrovidebb_ut != null)
            {
                Console.WriteLine(legrovidebb_ut.Count() - 1);
            }
            else
            {
                Console.WriteLine(-1);
            }
            /*
            foreach (var item in futolepesek)
            {
                Console.WriteLine("({0} - {1})",item.Item1,item.Item2);
            }
            if (legrovidebb_ut != null)
            {
                Console.WriteLine("A legrövidebb út:");
                foreach (var step in legrovidebb_ut)
                {
                    Console.WriteLine($"({step.Item1}, {step.Item2})");
                }
            }
            else
            {
                Console.WriteLine("Nincs lehetséges út a célig.");
            }
            */
        }
    }
}
/*
6
------
-++--+
--+---
+---+-
+---+-
+++--+
6 4 1 3


6
------
-++--+
--+---
+---+-
+---+-
+++--+
2 5 5 2
*/
