using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.Classes
{
    public static class LogHelper
    {
        private static Object LOCK = new Object();

        public static void AddLog(string Message)
        {
            lock (LOCK)
            {
                try
                {
                    if (Environment.UserInteractive)
                    {
                        //Console.WriteLine(string.Format("{0} - {1}", DateTime.Now, Message));
                    }
                    else
                    {


                        //var path = "c:\\temp\\coopmil_log.txt";

                        //StreamWriter vWriter = new StreamWriter(path, true);
                        //vWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now, Message));
                        //vWriter.Flush();
                        //vWriter.Close();
                    }
                }
                catch (Exception erro)
                {
                    if (Environment.UserInteractive)
                    {
                        Console.WriteLine(string.Format("ERRO - {0} - {1}", DateTime.Now, Message));
                    }
                    else
                    {
                        //StreamWriter vWriter = new StreamWriter("c:\\temp\\coopmil_log.txt", true);
                        //vWriter.WriteLine(erro.Message);
                        //vWriter.Flush();
                        //vWriter.Close();
                    }

                }
            }
        }

        
    }
}
