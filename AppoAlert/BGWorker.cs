﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppoAlert
{
    class BGWorker
    {
        public static List<Rule> Rules = new List<Rule>();
        public static List<Task> RuleWorkers = new List<Task>();

        public static void AddRule(string url, int refreshtime, string content = "")
        {
            Rule newRule = new Rule();

            newRule.Running = 0;
            newRule.URL = url;
            newRule.RuleID = getAvailableId();
            newRule.RefreshTime = refreshtime;
            newRule.SearchedContent = content;

            using (WebClient httpRunner = new WebClient())
            {
                newRule.SourceContent = httpRunner.DownloadString(url);
            }

            if (content == "")
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newRule.SourceContent));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    newRule.Hash = hash;
                }
            }

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
                        try
                        {
                            string webSiteContent = httpRunner.DownloadString(selectedRule.URL);
                            string hash = "";

                            using (var sha256 = SHA256.Create())
                            {
                                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(webSiteContent));
                                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                            }

                            if (selectedRule.Running == 0)
                            {
                                break;
                            }

                            if (selectedRule.SearchedContent != "")
                            {
                                if (webSiteContent.IndexOf(selectedRule.SearchedContent) != -1)
                                {
                                    Console.WriteLine("RULE WORKER >> <RULE:" + selectedRule.RuleID + "> Searched content founded");
                                }
                            }
                            else if (selectedRule.Hash != hash)
                            {
                                Console.WriteLine("RULE WORKER >> <RULE:" + selectedRule.RuleID + "> Changes are detected in content");
                                selectedRule.Hash = hash;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Rule ID:" + selectedRule.RuleID + "; Connecting problem: " + e.Message);
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
            int RunningNow = 0;

            foreach (var item in Rules)
            {
                Console.WriteLine("| RuleID: " + item.RuleID + "; URL: " + item.URL + "; RefreshTime: " + item.RefreshTime + "; isRunning: " + item.Running + " |");

                RunningNow += item.Running;
            }

            Console.WriteLine("-----------------------");
            Console.WriteLine("Total Rules: " + Rules.Count.ToString());
            Console.WriteLine("Running Now: " + RunningNow.ToString());
            Console.WriteLine("Idled: " + (Rules.Count - RunningNow).ToString());
            Console.WriteLine("-----------------------");
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
