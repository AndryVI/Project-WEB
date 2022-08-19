using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using WebH.Models;

namespace WebH.Services
{
    public class WriterAndReadService
    {

        #region Private

        //Ожидание создание директории
        private static string CreatedDirectory(string patch)
        {
            var directory = new DirectoryInfo(patch);
            try
            {
                Directory.CreateDirectory(patch);
                Thread.Sleep(1000);

                if (!directory.Exists)
                    CreatedDirectory(patch);

                return "Ок";
            }
            catch (Exception exp)
            {

                return $"{exp.Message}";
            }
        }

        //Ожидание создание файла   
        private static string CreateFile(string getfile, string getpatch)
        {
            var directory = new DirectoryInfo(getpatch);

            try
            {

                //Поиск файла, если нету создание 
                #region Find


                FileInfo[] files = directory.GetFiles(Regex.Replace(getfile, @"^\W\w+.", "*."));

                var file = new FileInfo(getpatch + getfile);


                if (files.Length == 0)
                {
                    FileStream stream = file.Create();
                    stream.Close();
                }
                else
                {
                    bool find = false;

                    foreach (FileInfo fil in files)
                        if (fil.Name == file.Name)
                            find = true;
                    if (!find)
                    {
                        FileStream stream = file.Create();
                        stream.Close();
                    }
                }

                if (!file.Exists)
                    CreateFile(getfile, getpatch);

                #endregion

                return "Ок";
            }
            catch (Exception exp)
            {

                return $"{exp.Message}";
            }

        }

        #endregion

        #region Public

     

        /// <summary>
        /// If ok =>  return "Ок"
        /// </summary>
        /// <remarks>
        /// else =>   exp.Message
        /// </remarks>
        /// <param name="getfile"></param>
        /// <param name="getdirectory"></param>
        /// <returns>(bool Success, string Text)</returns>
        public (bool Success, string Text) FileWriter(string textforwrite, FileMode getfileMode = FileMode.Append, string getfile = @"\Text.txt", string getdirectory = @"..\\..\\..\\file")
        {
            Console.OutputEncoding = Encoding.Unicode;

            //директории 
            string patch = getdirectory;
            var directory = new DirectoryInfo(patch);

            try
            {

                #region CheckDirectoryAndFile

                if (!directory.Exists)
                    CreatedDirectory(patch);

                CreateFile(getfile, patch);

                var file = new FileInfo(patch + getfile);

                #endregion

                //Заполнение 
                #region Writer

                FileStream openfile = File.Open(file.FullName, getfileMode, FileAccess.Write, FileShare.None);
                var writer = new StreamWriter(openfile);

                writer.WriteLine(textforwrite);
                writer.Close();
                openfile.Close();

                #endregion

                return (true, "Файл успешно записан");
            }
            catch (Exception exp)
            {
                return (false, $"{exp.Message}");
            }
        }

        /// <summary>
        /// If ok => return text
        /// </summary>
        /// <remarks>
        /// else =>   exp.Message
        /// </remarks>
        /// <param name="getfile"></param>
        /// <param name="getdirectory"></param>
        /// <returns>(bool Success, string Text)</returns>
        //Чтение
        public (bool Success, string[] Text) FileRead(string getfile = @"\Text.txt", string getdirectory = @"..\\..\\..\\file")
        {
            Console.OutputEncoding = Encoding.Unicode;

            //директории 
            string patch = getdirectory; //@"..\\..\\..\\file";
            var directory = new DirectoryInfo(patch);

            try
            {
                if (!directory.Exists)
                    return (false, new string[] { $"Directory not found" });


                //Поиск файла, если нету создание 
                #region Find

                FileInfo[] files = directory.GetFiles(Regex.Replace(getfile, @"^\W\w+\W\d+\W\d+.", "*."));

                var file = new FileInfo(patch + getfile);

                if (files.Length == 0)
                    return (false, new string[] { $"Directory is empty" });
                else
                {
                    bool find = false;

                    foreach (FileInfo fil in files)
                        if (fil.Name == file.Name)
                            find = true;

                    if (!find)
                        return (false, new string[] { $"File not found" });
                }

                #endregion

                //Чтение
                #region Reader

                FileStream openfile = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
                //StreamReader reader = new StreamReader(openfile);
                //string returntext = reader.ReadToEnd();

                List<string> contentlist = new List<string>();


                using (StreamReader readerfile = new StreamReader(openfile))
                {
                    string line;

                    while ((line = readerfile.ReadLine()) != null)
                        contentlist.Add(line);
                }

                //Console.WriteLine(reader.ReadToEnd());

                string[] content = contentlist.ToArray(); ;

                openfile.Close();

                #endregion

                return (true, content);
            }
            catch (Exception exp)
            {
                return (false, new string[] { $"{exp.Message}" });
            }
        }



        public List<LogActionModel> GetLog()
        {

            string[] lines = File.ReadAllLines("App_Data/logActionList.txt");


            var listCountUsersModels = new List<LogActionModel>();

            foreach (string line in lines)
            {
                string[] items = line.Split(';');

                LogActionModel logAction = new LogActionModel();

                logAction.DisplayName = items[0].Trim();
                logAction.DateAndTime = Convert.ToDateTime(items[1].Trim());
                logAction.CookieId = items[2].Trim();
                logAction.TimeElapsed = TimeSpan.Parse(items[3].Trim());

                string[] users = File.ReadAllLines("App_Data/logUniqueUsersList.txt");

                foreach (var user in users)
                {
                    string[] datauser = user.Split(';');
                    if (logAction.CookieId == datauser[1].Trim())
                        logAction.IdUser = Convert.ToInt32(datauser[0].Trim());
                }
                   
                listCountUsersModels.Add(logAction);
            }

            return listCountUsersModels;
        }

        #endregion
    }
}
