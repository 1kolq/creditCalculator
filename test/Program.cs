using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;


namespace creditCalc

{
    class Program
    {
         static double credit, stavka;
        static double srok;
        static void Main(string[] args)
        {
            string connection = "Server=127.0.0.1;user=root;database=mydb;password=123qwert;";
            MySqlConnection conn = new MySqlConnection(connection);
            conn.Open();
            Console.WriteLine("Введите id");
            string userid = Console.ReadLine();
            string sql = "SELECT  sum,bank_rate  from  Parametrs_setting where id =" + userid + ";" +
              "SELECT credit_term from Additional_services where Parametrs_setting_id =" + userid + "; ";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Сумма кредита = " + reader[0]);
                credit = Convert.ToDouble(reader[0]);
                Console.WriteLine("Ставка =  " + reader[1]);
                stavka = Convert.ToDouble(reader[1]) / 12;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    { 
                        Console.WriteLine("Срок кредита " + reader[0]);
                        srok = Convert.ToDouble(reader[0]);
                    }
                }
            }
            Console.WriteLine("Месяц || Ежемесячный платеж || Основной долг || Долг по процентам || Остаток основоного долга ");
            Raschet();
            Console.ReadKey();
            conn.Close();
        }
        public static void Raschet()
        {
            int month = 1;
            double platesh1month = 0;
            double creditostals9 = credit;
            for (int i = 0; i < srok; i++)

            {
                platesh1month =Math.Round(credit * ((stavka*Math.Pow(1+stavka,srok)/(Math.Pow(1+stavka,srok)-1))),2);
                double dolgpoprocentam = Math.Round(creditostals9 * stavka ,2);
                double dolgosnovnoi =Math.Round(platesh1month - dolgpoprocentam,2);
                creditostals9 = Math.Round(creditostals9 - dolgosnovnoi,2);
                Console.Write("  " + month +"\t");
                Console.Write("\t" + platesh1month + "\t");
                Console.Write("\t" + dolgosnovnoi + "\t");
                Console.Write("   \t" + dolgpoprocentam + "\t");
                Console.WriteLine("\t" + "\t" + creditostals9);
                month++;
            }
        }
    }
}
