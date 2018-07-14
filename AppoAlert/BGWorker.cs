using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppoAlert
{
    class BGWorker
    {
        public static List<Rule> Rules = new List<Rule>();
        public static List<Task> RuleWorkers = new List<Task>();

        public static void AddRule(string url, string content, int refreshtime)
        {
            Rule newRule = new Rule();
            newRule.Running = 0;
            newRule.URL = url;
            newRule.RuleID = getAvailableId();
            newRule.RefreshTime = refreshtime;
            newRule.Content = content;

            Rules.Add(newRule);
            Console.WriteLine("Success: New rule added.");
        }

        public static void RemoveRule(int rule_id)
        {
            Rules.Remove(getRuleFromList(rule_id));
            if (getRuleFromList(rule_id) == null)
            {
                Console.WriteLine("Success: Rule removed.");
            }
            else
            {
                Console.WriteLine("Fail: Not removed.");
            }
        }

        public static void StartRule(int ruleId)
        {
            Rule selectedRule = getRuleFromList(ruleId);
            if (selectedRule != null)
            {
                using (Task ruleWorker = new Task(() => WorkerStarter(selectedRule)))
                {
                    selectedRule.Running = 1;
                    RuleWorkers.Add(ruleWorker);
                    ruleWorker.Start();
                    Console.WriteLine("Success");
                }
            }
            else
            {
                Console.WriteLine("We have a problem: Selected task is not defined bro.");
            }
        }

        public static void StopRule(int ruleId)
        {
            Rule selectedRule = getRuleFromList(ruleId);
            if (selectedRule != null)
            {
                selectedRule.Running = 0;
                Console.WriteLine("Success: Selected rule is stopped.");
            }
            else
            {
                Console.WriteLine("We have a problem: Selected task is not defined bro.");
            }
        }

        public static void StartRules()
        {

        }

        public static void StopRules()
        {

        }

        static Task WorkerStarter(Rule selectedRule)
        {
            while (true)
            {
                Thread.Sleep(selectedRule.RefreshTime);
                if (selectedRule.Running == 1)
                {
                    using (WebClient httpRunner = new WebClient())
                    {
                        string webSiteContent = httpRunner.DownloadString(selectedRule.URL);
                        if (selectedRule.Running == 0)
                        {
                            break;
                        }
                        if (webSiteContent.IndexOf(selectedRule.Content) != -1)
                        {
                            Console.WriteLine("Founded string!");
                        }
                    }
                    
                }
                else
                {
                    break;
                }
            }
            return new Task(() => { });
        }

        public static void writeRulesToConsole()
        {
            foreach (var item in Rules)
            {
                Console.WriteLine("RuleID: " + item.RuleID + "; URL: " + item.URL + "; RefreshTime: " + item.RefreshTime + "; Content: " + item.Content + "; isRunning: " + item.Running);
            }
        }

        static Rule getRuleFromList(int ruleId)
        {
            return Rules.Find(x => x.RuleID == ruleId);
        }

        static int getAvailableId()
        {
            int currentReturning = 0;

            if (Rules.Count == 0)
            {
                return currentReturning;
            }

            for (int i = 0; i <= Rules.Count - 1; i++)
            {
                if (getRuleFromList(i) == null)
                {
                    currentReturning = i;
                    break;
                }

                currentReturning = i + 1;
            }

            return currentReturning;
        }

    }
}
