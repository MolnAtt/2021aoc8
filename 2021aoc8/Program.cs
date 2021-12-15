using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2021aoc8
{
    class Program
    {
        class Dekóder
        {
            class Jel
            {
                public HashSet<char> szegmensei;
                public Jel(string s) { szegmensei = s.ToHashSet(); }
                public static bool operator <(Jel j, string s) => j < s.ToHashSet();
                public static bool operator <(string s, Jel j) => j > s.ToHashSet();
                public static bool operator >(string s, Jel j) => j < s.ToHashSet();
                public static bool operator <(Jel j, HashSet<char> h) => j.szegmensei.IsProperSubsetOf(h);
                public static bool operator >(Jel j1, string s) => j1 > s.ToHashSet();
                public static bool operator >(Jel j, HashSet<char> h) => j.szegmensei.IsProperSupersetOf(h);
                public static bool operator ==(Jel j1, Jel j2) => j1 == j2.szegmensei;
                public static bool operator !=(Jel j1, Jel j2) => !(j1 == j2);
                public static bool operator ==(Jel j, HashSet<char> h) => Egyenlő(j.szegmensei,h);
                public static bool operator !=(Jel j, HashSet<char> h) => !Egyenlő(j.szegmensei,h);
                public static bool operator ==(HashSet<char> h, Jel j) => Egyenlő(h,j.szegmensei);
                public static bool operator !=(HashSet<char> h, Jel j) => !Egyenlő(h,j.szegmensei);
                public static bool Egyenlő(HashSet<char> h1, HashSet<char> h2) => h1.Count == h2.Count && h1.IsSubsetOf(h2);
                public static HashSet<char> operator +(Jel j1, string s) => j1 + s.ToHashSet();
                public static HashSet<char> operator +(Jel j, HashSet<char> h) => Unió(j.szegmensei, h);
                private static HashSet<char> Unió(HashSet<char> h1, HashSet<char> h2) { HashSet<char> u = new HashSet<char>(h1); u.UnionWith(h2); return u; }
            }

            string[] a_tíz_rossz_jel;
            Jel[] jel; 
            Dictionary<Jel, int> szótár;
            
            public Dekóder(string sor)
            {
                a_tíz_rossz_jel = sor.Trim().Split(' ');
                jel = new Jel[10];
                szótár = new Dictionary<Jel, int>();
            }

            public void Kalibrál()
            {
                jel[1] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 2));
                jel[4] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 4));
                jel[7] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 3));
                jel[8] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 7));
                jel[9] = new Jel(a_tíz_rossz_jel.First(x => jel[4] < x && x < jel[8])); // 9 az, ami a 4-es és 8-as között van.
                jel[3] = new Jel(a_tíz_rossz_jel.First(x => jel[7] < x && x < jel[9])); // 3 az, ami a 7 és a 9 között van.
                jel[6] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 6 && !(jel[1] < x))); // hatos az a 6-hosszú, aminek NEM része az 1-es jel.
                jel[5] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 5 && x < jel[6])); //5-ös az az 5-hosszú, ami része a 6-osnak
                jel[0] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 6 && !(jel[5] < x))); //0 az a 6-hosszú, aminek nem része az 5-ös jel
                jel[2] = new Jel(a_tíz_rossz_jel.First(x => x.Length == 5 && jel[4] + x == jel[8])); //2-es az az 5-hosszú, amit uniozva 4-gyel kijön a 8-as jel

                for (int i = 0; i < 10; i++)
                    szótár[jel[i]] = i;
            }

            private int érték(string s) => szótár[jel.First(x => x == new Jel(s))];
            public int Dekódol(string négyjel) => Dekódol(négyjel.Split(' '));
            private int Dekódol(string[] t) => 1000 * érték(t[0]) + 100 * érték(t[1]) + 10 * érték(t[2]) + érték(t[3]);

            public void Diagnosztika(string négyjel)
            {
                foreach (string s in a_tíz_rossz_jel)
                    Console.Write(s+" ");
                Console.WriteLine($": {Dekódol(négyjel)}");
            }
        }
        static void Main(string[] args)
        {
            int[] egyértelműek = new int[] { 2, 4, 3, 7 };
            /* első rész * /
            Console.WriteLine(File.ReadAllLines("input.txt").Select(sor => sor.Split('|')[1].Trim().Split(' ').Count(jel => egyértelműek.Contains(jel.Length))).Sum());
            /**/
            /**/
            int sum = 0;
            foreach (string sor in File.ReadAllLines("input.txt"))
            {
                string[] tíz_és_négy = sor.Split('|');
                (string tízjel, string négyjel ) = (tíz_és_négy[0].Trim(), tíz_és_négy[1].Trim());
                Dekóder d = new Dekóder(tízjel);
                d.Kalibrál();
                d.Diagnosztika(négyjel);
                sum += d.Dekódol(négyjel);
            }
            Console.WriteLine(sum);
            /**/
            Console.ReadKey();
        }
    }
}
