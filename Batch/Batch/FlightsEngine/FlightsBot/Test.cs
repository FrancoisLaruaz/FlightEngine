using IronPython.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;

namespace FlightsEngine.FlighsBot
{
    public static class Test
    {
        public static void run()
        {
            try
            {
                string mainfile = @"D:\DEV\Batch1\WebScraper\Main.py";
                // https://www.youtube.com/watch?v=PYnWKSXXyIk
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Test ***");
               
                var engine = Python.CreateEngine();
                //Set up the folder with my code and the folder with struct.py.
                // https://stackoverflow.com/questions/30234756/execute-python-3-code-from-c-sharp-forms-application

                var searchPaths = engine.GetSearchPaths();
                searchPaths.Add(@"C:\Users\franc\AppData\Local\Programs\Python\Python37-32\Lib");
                engine.SetSearchPaths(searchPaths);

                //A couple of files.
                

                var scope = engine.CreateScope();

                /*
                 *                 var scope = engine.CreateScope();
                ScriptSource source = engine.CreateScriptSourceFromFile(mainfile);

                source.Execute(scope); 
                 * 
                 */


                var scriptSource = engine.CreateScriptSourceFromFile(mainfile);
                var compiledScript = scriptSource.Compile();
                compiledScript.Execute(scope);

                scriptSource = engine.CreateScriptSourceFromString("my_variable");
                scriptSource.Compile();
                var theValue = compiledScript.Execute(scope);
                /*

               Process p = new Process(); // create process to run the python program
               p.StartInfo.FileName = @"C:\Users\franc\AppData\Local\Programs\Python\Python37-32\python.exe"; //Python.exe location
               p.StartInfo.RedirectStandardOutput = true;
               p.StartInfo.UseShellExecute = false; // ensures you can read stdout
               p.StartInfo.Arguments =  mainfile + " '213.157.62.137' '1' 'Edreams' 'RNS' 'TYO' 'false' '10/10/2018' '18/10/2018'";
               p.Start(); // start the process (the python program)
               StreamReader s = p.StandardOutput;
               String output = s.ReadToEnd();
               Console.WriteLine(output);
               FlightsEngine.Utils.Logger.GenerateInfo(output);
               p.WaitForExit();
               */
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Test ***");
            }
            catch(Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
        }


    }
}