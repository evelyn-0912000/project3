using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meteen_Rotterdam
{
    class Filter
    {
        public static List<List<string>> executeQuery(string query, string connectionString, int columnAmount, bool returnColumns=false)
        {
            try
            {
                using (MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand select = new MySqlCommand(query, conn);
                    MySqlDataReader reader = select.ExecuteReader();

                    List<List<string>> results = new List<List<string>>();

                    if (returnColumns)
                    {
                        List<string> columns = new List<string>();
                        for (int i = 0; i < columnAmount; i++)
                        {
                            columns.Add(reader.GetName(i));
                        }

                        results.Add(columns);
                    }
                    
                    while (reader.Read())
                    {
                        List<string> row = new List<string>();
                        for (int i = 0; i < columnAmount; i++)
                        {
                            row.Add(reader.GetString(i));
                        }

                        results.Add(row);
                    }

                    return results;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                List<List<string>> messageListList = new List<List<string>>();
                List<string> messageList = new List<string>();
                messageList.Add(ex.Message);
                messageListList.Add(messageList);

                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();

                return messageListList;
            }
        }

        List<List<string>> filterNodes(string connectionString, params Tuple<string, string>[] pairs)
        {
            string query =  @"SELECT a.x, a.y
                            FROM attractions AS a
                            INNER JOIN occasions AS o
                            ON(o.occasion_name = a.occasion)";

            bool firstPair = true;
            bool returnColumns = false;

            foreach (Tuple<string, string> pair in pairs)
            {
                string component;
                string filtration = "";

                switch(pair.Item1)
                {
                    case "mood":
                        filtration = String.Format("o.mood = '{0}'", pair.Item2);
                        break;

                    case "indoors":
                        try
                        {
                            if (Convert.ToBoolean(pair.Item2) == true || Convert.ToBoolean(pair.Item2) == false)
                            {
                                filtration = String.Format("o.indoors = '{0})'", pair.Item2);
                            }
                        } catch
                        {
                            throw new System.ArgumentException("Error, neither 'true' nor 'false' supplied.", "original");
                        }
                        break;

                    case "returnColumns":
                        if (Convert.ToBoolean(pair.Item2))
                        {
                            returnColumns = true;
                        }
                        break;
                    
                    case "amount_min":
                    case "amount_max":
                    case "age_min":
                    case "age_max":
                        filtration = String.Format("o.{0} = {1}", pair.Item1, pair.Item2);
                        break;
                }

                if (firstPair)
                {
                    component = '\n' + "WHERE ";
                    firstPair = false;
                }
                else
                {
                    component = '\n' + "AND ";
                }

                if (filtration != "")
                {
                    query = query + component + filtration;
                }
            }

            List<List<string>> results = executeQuery(query, connectionString, pairs.Length, returnColumns);

            return results;
        }

        public static List<List<string>> initialMap(string connectionString)
        {
            string query = @"SELECT a.x, a.y
                            FROM attractions AS a;";

            List<List<string>> results = executeQuery(query, connectionString, 2);

            return results;
        }

    List<string> identifyNode(string connectionString, double x, double y, bool returnColumns=false)
        {
            string query =  @"SELECT a.name, a.postal
                            FROM attractions as a
                            WHERE a.x = {0}
                            AND a.y = {1}";

            query = String.Format(query, x, y);

            List<List<string>> results = executeQuery(query, connectionString, 2, returnColumns);
            List<string> node = results[0];

            return node;
        }
    }
}
