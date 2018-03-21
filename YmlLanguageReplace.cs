using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YmlLanguageReplacement
{
    static class YmlLanguageReplace
    {
        public static void replaceLangCode()
        {

            Console.Write("Please type Code Base Folder to search *.yml files:");

            var path = Console.ReadLine();
            if (!Directory.Exists(path))
            {
                Console.Write("Please type Correct Code Base Folder to search *.yml files:");
                path = Console.ReadLine();
            }
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.yml", SearchOption.AllDirectories);
                var iCount = 0;
                var iCountOld =0;
                var iCountAlreadyExist = 0;
                Console.Write("Please type old language code:");
                var oldLangCode = Console.ReadLine();
                Console.Write("Please type new language code:");
                var newLangCode = Console.ReadLine();
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "log_" + oldLangCode + ".txt";
                try
                {

                    if (File.Exists(exePath))
                        File.Delete(exePath);

                    LogWrite("Total yml Files Found=" + files.Length, exePath);

                    Console.WriteLine("Total yml Files Found=" + files.Length);

                    foreach (var file in files)
                    {
                        string str = File.ReadAllText(file);
                        if (str.IndexOf("-" + newLangCode) > 0)
                        {
                            iCountOld = iCountOld + 1;
                            LogWrite("yml File Found=" + file, exePath);
                            Console.WriteLine("yml File Found=" + file);
                            str = str.Replace("-" + newLangCode, newLangCode);
                            File.WriteAllText(file, str);
                        }
                        else
                        {
                            if (str.IndexOf(newLangCode) <= 0 && str.IndexOf("- Language: " + newLangCode) <= 0)
                            {
                                if (str.IndexOf("- Language: " + oldLangCode) > 0)
                                {
                                    iCount = iCount + 1;
                                    LogWrite("yml File Found=" + file, exePath);
                                    Console.WriteLine("yml File Found=" + file);
                                    str = str.Replace("- Language: " + oldLangCode, "- Language: " + newLangCode);
                                    File.WriteAllText(file, str);
                                }
                            }
                            else
                            {
                                iCountAlreadyExist = iCountAlreadyExist + 1;
                                LogWrite("*****************************", exePath);
                                LogWrite("*****************************", exePath);
                                LogWrite("************Already using en-GB=" + file, exePath);
                                LogWrite("*****************************", exePath);
                                LogWrite("*****************************", exePath);

                                Console.WriteLine("************");
                                Console.WriteLine("************Already using en-GB=" + file);
                                Console.WriteLine("************");
                            }
                        }
                    }
                    LogWrite("Total Replaced Files are =" + iCount, exePath);
                    LogWrite("Total Replaced Files are existing =" + iCountOld, exePath);
                    LogWrite("Total Already used en-GB code files=" + iCountAlreadyExist, exePath);
                    Console.WriteLine("Total Replaced Files are =" + iCount);
                    Console.WriteLine("Total Already used en-GB code files=" + iCountAlreadyExist);
                    Console.WriteLine("Successfully Completed");
                    Console.Read();
                }
                catch (Exception ex)
                {
                    LogWrite("Error" + ex.Message, exePath);
                }
            }
            else
            {
                Console.Write("Wrong Folder: Good Bye");
            }
        }
        public static void LogWrite(string logMessage, string exePath)
        {
            try
            {
                using (StreamWriter txtWriter = File.AppendText(exePath))
                {
                    txtWriter.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error===" + ex.Message);
            }
        }
    }
}
