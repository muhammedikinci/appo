using System;
using System.Collections.Generic;

namespace AppoAlert
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Write(">>");
                try
                {
                    string args = Console.ReadLine();
                    if (args.Length > 0)
                    {
                        string[] arr = args.Split(' ');

                        if (arr[0] == "appo")
                        {
                            if (arr[1] == "help")
                            {
                                WriteHelp();
                            }
                            else if (arr[1] == "add-rule")
                            {
                                if (arr[2] == "sc")
                                {
                                    BGWorker.AddRule(arr[3], int.Parse(arr[5]), arr[5]);
                                }
                                else if (arr[2] == "cc")
                                {
                                    BGWorker.AddRule(arr[3], int.Parse(arr[4]));
                                }
                                else
                                {
                                    ErrorCaseBlock("help");
                                }
                            }
                            else if (arr[1] == "load-rule")
                            {

                            }
                            else if (arr[1] == "hide")
                            {

                            }
                            else if (arr[1] == "stop")
                            {
                                if (arr.Length > 1)
                                {
                                    if (arr[2] != "")
                                    {
                                        BGWorker.StopRule(int.Parse(arr[2]));
                                    }
                                    else
                                    {
                                        ErrorCaseBlock("empty_parameter");
                                    }
                                }
                                else
                                {
                                    ErrorCaseBlock("help");
                                }
                            }
                            else if (arr[1] == "start")
                            {
                                if (arr.Length > 1)
                                {
                                    if (arr[2] != "")
                                    {
                                        BGWorker.StartRule(int.Parse(arr[2]));
                                    }
                                    else
                                    {
                                        ErrorCaseBlock("empty_parameter");
                                    }
                                }
                                else
                                {
                                    ErrorCaseBlock("help");
                                }
                            }
                            else if (arr[1] == "rules")
                            {
                                BGWorker.writeRulesToConsole();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void WriteHelp()
        {
            Console.WriteLine(" - Help;");
            Console.WriteLine(" - Add rule;");
            Console.WriteLine("   - appo add-rule <<url>> <<content>> <<refresh_time>>");
            Console.WriteLine("");
            Console.WriteLine(" - Remove rule;");
            Console.WriteLine("   - appo remove-rule <<rule_id>>");
            Console.WriteLine("");
            Console.WriteLine(" - Load rules from file;");
            Console.WriteLine("   - appo load-rule <<file>>");
            Console.WriteLine("");
            Console.WriteLine(" - Hide to system tray;");
            Console.WriteLine("   - appo hide");
            Console.WriteLine("");
            Console.WriteLine(" - Stop all rules;");
            Console.WriteLine("   - appo stop");
            Console.WriteLine("");
            Console.WriteLine(" - Stop rule with id;");
            Console.WriteLine("   - appo stop <<rule_id>>");
            Console.WriteLine("");
            Console.WriteLine(" - Start all rules;");
            Console.WriteLine("   - appo start");
            Console.WriteLine("");
            Console.WriteLine(" - Start rule with id;");
            Console.WriteLine("   - appo start <<rule_id>>");
        }
        static void ErrorCaseBlock(string str)
        {
            if (str == "help")
            {
                Console.WriteLine("You can use \"appo help\" command for example usage");
            }
            else if (str == "empty_parameter")
            {
                Console.WriteLine("Parameters must be not empty!");
            }
        }
    }
}
