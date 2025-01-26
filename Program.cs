using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//ref: https://en.wikipedia.org/wiki/INI_file

namespace iniedit
{

    class IniEdit
    {
        private const string ADD = "/a";
        private const string DEL = "/d";
        private const string YES = "/y";
        private static char[] CLN = { ':' };
        private static char[] EQU = { '=' };
        private static List<ActionParam> opts;
        private static string iniFile = null;
        private static bool bCreate = false;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        //params: 1=inifilePath 2=action 3=params: section propname propval
        //sample c:\temp\test.ini /add section:name=value 

        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Print("Invalid parameter count! Please refer to the usage info below.");
                Usage();
                return -1;
            }
            iniFile = args[0];
            ProcOpts(args);
            if(!File.Exists(iniFile) && !bCreate)
            {
                Print("File does not exist. Specify /y to create file if non-existent");
                return -1;
            }
            return ProcEdit();
        }
        static int ProcEdit()
        {
            foreach(ActionParam p in opts)
            {
                switch (p.Action)
                {
                    case ADD:
                        Write(p.Data, p.Value, p.Section);
                        break;
                    case DEL:
                        Delete(p.Data, p.Section);
                        break;
                }
            }
            return 0;
        }
        static void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section, Key, Value, iniFile);
        }

        static void Delete(string Key, string Section)
        {
            Write(Key, null, Section);
        }

        static int ProcOpts(string[] args)
        {           
            opts = new List<ActionParam>();
            string curAction = null;
            for (int i = 1; i < args.Length; i++)
            {
                string s = args[i];               
                if (s.StartsWith("/"))
                {
                    curAction = s.ToLower();
                    switch (curAction) {
                        case ADD:
                            break;
                        case DEL:
                            break;
                        case YES:
                            bCreate = true;
                            break;
                        default:
                            Print("Unsupported action verb: " + curAction);
                            return -1;
                    }
                }
                else
                {
                    if (curAction == null)
                    {
                        Print("Parameter must be preceeded by an action verb.");
                        return -1;
                    }
                        else
                    {
                        string[] p = s.Split(CLN,2);
                        string sec = p[0]; //section name is always first
                        string nam = null;
                        string val = null;
                        if (p.Length==2)  //param is specified
                        {
                            string[] q = p[1].Split(EQU,2);
                            nam = q[0];  //propname
                            val = q.Length == 2 ? q[1] : String.Empty;
                        }
                        ActionParam n = new ActionParam(curAction, sec, nam, val);
                        opts.Add(n);
                     }
                }
            }
            return 0;
        }
        static void Usage()
        {
            Print("Usage: ");
            Print(" iniedit iniFilePath [actionVerb] [actionVerbParameters...] [behaviorFlag]");
            Print("Where:");
            Print(" iniFilePath - fullPath of the ini file");
            Print(" actionVerb - could be one of the following:");
            Print("    Add/Overwrite a prop Name/Value into a specific section.");
            Print("     /a [sectionName]:[propName]=[propValue]");
            Print("    Add a section.");
            Print("     /a [sectionName]");
            Print("    Delete a prop under a specific section.");
            Print("     /d [sectionName]:[propName]");
            Print("    Delete a whole section.");
            Print("     /d [sectionName]");
            Print(" behaviorFlag - either of the following:");
            Print("    /y - If file does not exist, /add will create new ini file; /del is ignored.");
            Print("         if not specified, return an error code -1 if file does not exist.");

        }
        static void Print(string txt)
        {
            Console.Out.WriteLine(txt);
        }


        internal class ActionParam
        {
            public string Action { get; }
            public string Section { get; }
            public string Data { get; }
            public string Value { get; }
            public ActionParam(string action, string section, string data = null, string value = null)
            {
                this.Action = action;
                this.Section = section;
                this.Data = data;
                this.Value = value;
            }
        }
    }
}
