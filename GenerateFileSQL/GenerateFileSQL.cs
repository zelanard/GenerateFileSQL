using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenerateFileSQL
{
    internal class GenerateFileSQL
    {
        static void Main(string[] args)
        {
            //todo: Grant the user the ability to write their own file path... (This is unnecesary for the task at hand.)
            GenerateCSV(@"D:\Skole\H1\SQL\Random.csv", GenerateType.numbers);
        }

        #region ////////////////////////////////// Controller //////////////////////////////////
        /// <summary>
        /// Controlls the generation of the CSV File
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        private static void GenerateCSV(string path, GenerateType type)
        {
            while (true)
            {
                //len should be set by user input.
                int len = 1000000;
                // FileToDatabase(path, len);
                // FileTodatabase is fully functional, but shold not run simultanously with create file.
                // todo: let user select between write to database and create file.

                try
                {
                    DuplicateFilePrevention(path);

                    // create a new file
                    using (FileStream fs = File.Create(path))
                    {
                        switch (type)
                        {
                            case GenerateType.numbers:
                                {
                                    GenerateNumbers(fs, len);
                                    break;
                                }
                            case GenerateType.loremIpsum:
                                //todo: repeat the for loop above and make a function that generate lorem ipsum instead of random numbers.
                                break;
                        }
                    }
                    Success();
                }
                catch (Exception ex)
                {
                    Fail(ex.Message);
                }

                PressAnyKey();
            }
        }

        #endregion


        #region //////////////////////////////////// Model ////////////////////////////////////
        /// <summary>
        /// Add as many new lines to the file as the size of len.
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="len"></param>
        /// <param name="rnd"></param>
        private static void GenerateNumbers(FileStream fs, int len)
        {
            for (int i = 0; i < len; i++)
            {
                Byte[] line = new UTF8Encoding(true).GetBytes(GetLine(i));
                fs.Write(line, 0, line.Length);
            }
        }

        /// <summary>
        /// Get csv file line
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string GetLine(int i)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            TextInfo textInfo = culture.TextInfo;
            Random rnd = new Random();

            return i.ToString() + textInfo.ListSeparator + rnd.Next(0, 9999).ToString() + Environment.NewLine;
        }

        /// <summary>
        /// If the file exists we delete it.
        /// </summary>
        /// <param name="path"></param>
        private static void DuplicateFilePrevention(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static void FileToDatabase(string path, int len)
        {
            string conStr = @"Data Source=Balder\MSSQLSERVER01;Initial Catalog=Performance;Integrated Security=True";
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();

            for(int i = 0; i < len; i++)
            {
                string insert = $"INSERT INTO Random (Id, RandomNumbers) VALUES ({GetLine(i)})";
                using (SqlCommand cmd = new SqlCommand(insert, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Determines type of file.
        /// </summary>
        enum GenerateType
        {
            numbers,
            loremIpsum
        }

        #endregion


        #region ///////////////////////////////////// View /////////////////////////////////////
        /// <summary>
        /// Write success message to the console, or later to the logging system.
        /// </summary>
        private static void Success()
        {
            Console.WriteLine("Your file was generated!");
        }

        /// <summary>
        /// Write an error message to the console, or later to the logging system.
        /// </summary>
        /// <param name="message"></param>
        private static void Fail(string message)
        {
            Console.WriteLine($"Error Your file has not been generated: {message}");
        }

        /// <summary>
        /// Takes a user input to detemrine what should e done next.
        /// </summary>
        private static void PressAnyKey()
        {
            Console.WriteLine("If you want to write another file, press Enter.");
            Console.WriteLine("If you want to exit the application press Backspace or Escape.");
            ConsoleKey key = Console.ReadKey().Key;

            while (true)
            {
                switch (key)
                {
                    case ConsoleKey.Enter:
                        return;
                    case ConsoleKey.Backspace:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
