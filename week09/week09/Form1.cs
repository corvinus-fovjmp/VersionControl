using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week09.Entities;

namespace week09
{
    public partial class Form1 : Form


    {

        List<Person> Population = new List<Person>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();
            //Népteszt
            //Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            //Nép teljes
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
            //dataGridView1.DataSource = Population;

            for (int year = 2005; year <= 2024; year++)
            {
                
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year,Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                //Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
                if (year == 2024)
                {
                    MessageBox.Show(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
                }
                

            }

        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive)
            {
                return;
            }

            byte age = (byte)(year - person.BirthYear);
            double PDeath = (from d in DeathProbabilities
                             where d.Age == age && d.Gender == person.Gender
                             select d.DeathProb).FirstOrDefault();
            if (rng.NextDouble()<PDeath)
            {
                person.IsAlive = false;
            }
            if (person.IsAlive == true && person.Gender == Gender.Female)
            {
                double PBirth = (from b in BirthProbabilities
                                 where b.Age == age
                                 select b.BirthProb).FirstOrDefault();
                if (rng.NextDouble()<PBirth)
                {
                    Person newborn = new Person();
                    newborn.BirthYear = year;
                    newborn.NbrOfChildren = 0;
                    newborn.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(newborn);
                }
            }
        }

        public List<Person> GetPopulation (string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;


        }
        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthprob = new List<BirthProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthprob.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        BirthProb = double.Parse(line[2])



                    });
                }



            }

            return birthprob;


        }
        public List<DeathProbability> GetDeathProbabilities (string csvpath)
        {
            List<DeathProbability> deathprob = new List<DeathProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathprob.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        DeathProb = double.Parse(line[2])
                    });
                }
            }
            return deathprob;




        }
        
    }
}
