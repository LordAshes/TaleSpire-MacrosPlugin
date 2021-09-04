using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordAshes
{
    class MacroBuilder
    {
        static void Main(string[] args)
        {
            BuildMacroPluginSourceCode();
            BuildMacrosPlugin();
        }

        static void BuildMacroPluginSourceCode()
        {
            string template = System.IO.File.ReadAllText("macro_template.pcs");
            string check = "";
            string execute = "";
            int count = 1;
            string[] macroFiles = System.IO.Directory.EnumerateFiles("./macros", "*.macroScript").ToArray();
            foreach (string macroFile in macroFiles)
            {
                string[] contents = System.IO.File.ReadAllLines(macroFile);
                string title = "";
                string button = "";
                int mode = 0;
                string checkCode = "";
                string executeCode = "";
                foreach (string line in contents)
                {
                    if (line.ToUpper().StartsWith("MACRO:")) { title = line.Substring(6); }
                    if (line.ToUpper().StartsWith("BUTTON:")) { button = line.Substring(7); }
                    if (line.ToUpper().StartsWith("CHECK:")) { mode = 1; }
                    else if (line.ToUpper().StartsWith("EXECUTE:")) { mode = 2; }
                    else if (mode == 1) { checkCode = checkCode + "\t\t\t" + line + "\r\n"; }
                    else if (mode == 2) { executeCode = executeCode + "\t\t\t" + line + "\r\n"; }
                }

                while (checkCode.StartsWith("\t\t\t\r\n")) { checkCode = checkCode.Substring(5); }
                while (executeCode.StartsWith("\t\t\t\r\n")) { executeCode = executeCode.Substring(5); }

                check = check + "\t\t\t\t//\r\n";
                check = check + "\t\t\t\t// Macro: " + title + "\r\n";
                check = check + "\t\t\t\t//\r\n";
                check = check + "\t\t\t\tif(Macro_" + count.ToString("d2") + "_Available())\r\n";
                check = check + "\t\t\t\t{\r\n";
                check = check + "\t\t\t\t\tif(GUI.Button(new Rect(1920-macroMenuSlide, offset, 160, 40), \"" + button + "\")) { Macro_" + count.ToString("d2") + "_Execute(); }\r\n";
                check = check + "\t\t\t\t\toffset = offset + 50;\r\n";
                check = check + "\t\t\t\t}\r\n";

                execute = execute + "\t\tpublic bool Macro_" + count.ToString("d2") + "_Available()\r\n\t\t{\r\n" + checkCode + "\r\n\t\t}\r\n\r\n";
                execute = execute + "\t\tpublic void Macro_" + count.ToString("d2") + "_Execute()\r\n\t\t{\r\n" + executeCode + "\r\n\t\t}\r\n";

                count++;
            }
            System.IO.File.WriteAllText("./MacrosPlugin/MacrosPlugin.cs", template.Replace("{Check}", check).Replace("{Execute}", execute));
        }

        public static void BuildMacrosPlugin()
        {
            string[] config = System.IO.File.ReadAllLines("MacrosBuilder.config");
            Process build = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = "./MacrosPlugin",
                    FileName = config[0],
                    Arguments = "MacrosPlugin.csproj",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            build.Start();
            StreamReader reader = build.StandardOutput;
            string output = reader.ReadToEnd();
            Console.WriteLine(output);
            build.WaitForExit();
            if(System.IO.File.Exists("./MacrosPlugin/bin/Debug/MacrosPlugin.dll"))
            {
                if (System.IO.File.Exists(config[1] + "/MacrosPlugin.dll")) { System.IO.File.Delete(config[1] + "/MacrosPlugin.dll"); }
                System.IO.File.Copy("./MacrosPlugin/bin/Debug/MacrosPlugin.dll", config[1] + "/MacrosPlugin.dll");
            }
        }
    }
}
