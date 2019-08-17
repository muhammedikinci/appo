using System;
using CommandLine;
using System.Collections.Generic;

namespace AppoAlert
{
    class Program
    {
        [Verb("add-rule", HelpText = "Adds new rules.")]
        public class AddRuleOptions {
            [Value(0, Required = true, HelpText = "Task type: sc(Search in content) cc(Changes Content)")]
	        public string Type { get; set; }

            [Value(1, Required = true, HelpText = "URL")]
	        public string URL { get; set; }

            [Value(2, Required = true, HelpText = "Refresh time")]
	        public string Time { get; set; }

            [Value(2, Required = false, HelpText = "Search text")]
	        public string Content { get; set; }
        }

        [Verb("load", HelpText = "Loads rules from file.")]
        public class LoadFileOptions {
            [Value(0, Required = false, HelpText = "File path; Default : rules.json")]
	        public string LoadFilePath { get; set; }
        }

        [Verb("save", HelpText = "Saves rules from file.")]
        public class SaveFileOptions {
            [Value(0, Required = false, HelpText = "File path; Default : rules.json")]
	        public string SaveFilePath { get; set; }
        }

        [Verb("stop", HelpText = "Stops the rule.")]
        public class StopRuleOptions {
            [Value(0, Required = true, HelpText = "Rule ID")]
	        public string StopRuleID { get; set; }
        }

        [Verb("start", HelpText = "Starts the rule.")]
        public class StartRuleOptions {
            [Value(0, Required = true, HelpText = "Rule ID")]
	        public string StartRuleID { get; set; }
        }

        [Verb("remove", HelpText = "Removes the rule.")]
        public class RemoveRuleOptions {
            [Value(0, Required = true, HelpText = "Rule ID")]
	        public string RemoveRuleID { get; set; }
        }

        [Verb("remove-all", HelpText = "Removes all rules.")]
        public class RemoveAllRulesOptions {
        }

        [Verb("rules", HelpText = "Gets all rules.")]
        public class GetAllRulesOptions {
        }

        [Verb("start-all", HelpText = "Starts all rules.")]
        public class StartAllRulesOptions {
        }

        [Verb("stop-all", HelpText = "Starts all rules.")]
        public class StopAllRulesOptions {
        }

        static void Main()
        {
            bool exit = false;

            while (!exit) {
                Console.Write(">>");
                string data = Console.ReadLine();
                string[] args = data.Split(" ");

                if (args[0] == "exit")
                {
                    exit = true; 
                    break;
                }

                Parser.Default.ParseArguments<AddRuleOptions, LoadFileOptions, SaveFileOptions, StopRuleOptions, StartRuleOptions, RemoveRuleOptions, RemoveAllRulesOptions, GetAllRulesOptions, StartAllRulesOptions, StopAllRulesOptions>(args)
                    .WithParsed<AddRuleOptions>(options => { AddRule(options); })
                    .WithParsed<LoadFileOptions>(options => { LoadFile(options); })
                    .WithParsed<SaveFileOptions>(options => { SaveFile(options); })
                    .WithParsed<StopRuleOptions>(options => { StopRule(options); })
                    .WithParsed<StartRuleOptions>(options => { StartRule(options); })
                    .WithParsed<RemoveRuleOptions>(options => { RemoveRule(options); })
                    .WithParsed<RemoveAllRulesOptions>(options => { RemoveAllRules(options); })
                    .WithParsed<GetAllRulesOptions>(options => { GetAllRules(options); })
                    .WithParsed<StartAllRulesOptions>(options => { StartAllRules(options); })
                    .WithParsed<StopAllRulesOptions>(options => { StopAllRules(options); });
            }
        }

        static void AddRule(AddRuleOptions options) {
            BGWorker.AddRule(options.Type, options.URL, int.Parse(options.Time), options.Content);
        }

        static void LoadFile(LoadFileOptions options) {
            BGWorker.RulesFileName = options.LoadFilePath;
            BGWorker.LoadRules();
        }

        static void SaveFile(SaveFileOptions options) {
            BGWorker.RulesFileName = options.SaveFilePath;
            BGWorker.SaveRules();
        }

        static void StopRule(StopRuleOptions options) {
            BGWorker.StopRule(int.Parse(options.StopRuleID));
        }

        static void StartRule(StartRuleOptions options) {
            BGWorker.StartRule(int.Parse(options.StartRuleID));
        }

        static void RemoveRule(RemoveRuleOptions options) {
            BGWorker.StopRule(int.Parse(options.RemoveRuleID));
            BGWorker.RemoveRule(int.Parse(options.RemoveRuleID));
        }

        static void RemoveAllRules(RemoveAllRulesOptions options) {
            foreach (var item in BGWorker.Rules)
            {
                BGWorker.StopRule(item.RuleID);
            }

            BGWorker.Rules.Clear();
            Console.WriteLine("Rules removed");
        }

        static void GetAllRules(GetAllRulesOptions options) {
            BGWorker.writeRulesToConsole();
        }

        static void StartAllRules(StartAllRulesOptions options) {
            foreach (var item in BGWorker.Rules)
            {
                BGWorker.StartRule(item.RuleID);
            }

            Console.WriteLine("All rules started!");
        }

        static void StopAllRules(StopAllRulesOptions options) {
            foreach (var item in BGWorker.Rules)
            {
                BGWorker.StopRule(item.RuleID);
            }

            Console.WriteLine("All rules stopped!");
        }
    }
}