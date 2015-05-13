using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War
{
    class Program
    {
        static void Main(string[] args)
        {
            int N;

            StreamReader file = new StreamReader("input.txt");
            StreamWriter file2 = new StreamWriter("output.txt");

            Int32.TryParse(file.ReadLine(), out N);

            for (int Case = 1; Case <= N; Case++)
            {
                int blocks;
                Int32.TryParse(file.ReadLine(), out blocks);
                string[] line1 = file.ReadLine().Split(' ');
                string[] line2 = file.ReadLine().Split(' ');

                List<Double> ken = new List<double>();
                List<Double> naomi = new List<double>();

                List<Double> ken2 = new List<double>();
                List<Double> naomi2 = new List<double>();

                for(int i=0;i<blocks;i++)
                {
                    double rez;
                    Double.TryParse(line2[i], out rez);
                    ken.Add(rez);
                    Double.TryParse(line1[i], out rez);
                    naomi.Add(rez);
                }

                ken.Sort();
                naomi.Sort();

                ken2.AddRange(ken);
                naomi2.AddRange(naomi);

                double ck, cn, fcn;
                int fairkenp = 0;
                int fairnaomip = 0;

                for (int i = 0; i < blocks; i++)
                {
                    cn = naomi2.Min();

                    var intermediate = from block in ken2
                                       where block > cn
                                       orderby block
                                       select block;

                    var result = intermediate.FirstOrDefault();

                    ck = result;

                    if(ck != 0)
                    {
                        fairkenp++;
                        naomi2.Remove(cn);
                        ken2.Remove(ck);
                    }
                    else
                    {
                        fairnaomip++;
                    }
                }

                int kenp = 0;
                int naomip = 0;

                
                for(int i=0;i<blocks;i++)
                {
                    if(naomi.Min() < ken.Min())
                    {
                        fcn = ken.Max() - 0.0001f;
                        ck = ken.Max();
                        cn = naomi.Min();
                        kenp++;
                    }
                    else
                    {
                        fcn = ken.Max() + 0.0001f;
                        ck = ken.Min();
                        cn = naomi.Min();
                        naomip++;
                    }
                    naomi.Remove(cn);
                    ken.Remove(ck);
                }

                file2.WriteLine("Case #" + Case + ": " + naomip + " " + fairnaomip);
            }
            file.Close();

            file2.Close();
        }
    }
}
