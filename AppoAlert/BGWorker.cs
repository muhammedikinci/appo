using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace AppoAlert
{
    class BGWorker
    {
        public static string RulesFileName = "rules.json";
        static byte[] JsonBuffer = new byte[60000];

        public static List<Rule> Rules = new List<Rule>();
        public static List<Task> RuleWorkers = new List<Task>();

        public static void AddRule(string type, string url, int refreshtime, string content = "")
        {
            Rule newRule = new Rule();

            newRule.Running = 0;
            newRule.URL = url;
            newRule.RuleID = getAvailableId();
            newRule.RefreshTime = refreshtime;
            newRule.SearchedContent = content;
            newRule.Type = type;

            using (WebClient httpRunner = new WebClient())
            {
                newRule.SourceContent = httpRunner.DownloadString(url);
            }

            if (type == "cc" && !string.IsNullOrEmpty(content))
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newRule.SourceContent));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    newRule.Hash = hash;
                }
            }
            else if (type == "cc" && !string.IsNullOrEmpty(content) && newRule.SourceContent.IndexOf(content) != -1) {
                Console.WriteLine("The specified content was found. You will be notified when the change is detected.");
            }
            else if (type == "sc" && !string.IsNullOrEmpty(content) && newRule.SourceContent.IndexOf(content) == -1)
            {
                Console.WriteLine("The specified content does not exist on the site. You will be notified when content is added.");
            }

            Rules.Add(newRule);

            Console.WriteLine("Success: New rule added. Rule Id {0}", newRule.RuleID);
        }

        public static void RemoveRule(int rule_id)
        {
            Rules.Remove(getRuleFromList(rule_id));
            if (getRuleFromList(rule_id) == null)
            {
                Console.WriteLine("Success: {0} removed.", rule_id);
            }
            else
            {
                Console.WriteLine("Fail: Not removed.");
            }
        }

        public static void StartRule(int ruleId)
        {
            Rule selectedRule = getRuleFromList(ruleId);
            if (selectedRule != null && selectedRule.Running == 0)
            {
                Task RuleWorker = new Task(() => { WorkerStarter(selectedRule); });
                RuleWorker.Start();

                selectedRule.Running = 1;
                RuleWorkers.Add(RuleWorker);
                Console.WriteLine("{0} Started!", ruleId);
            }
            else
            {
                Console.WriteLine("{0} task is not defined or started.", ruleId);
            }
        }

        public static void StopRule(int ruleId)
        {
            Rule selectedRule = getRuleFromList(ruleId);
            if (selectedRule != null && selectedRule.Running == 1)
            {
                selectedRule.Running = 0;
                Console.WriteLine("Success: {0} rule is stopped.", ruleId);
            }
            else
            {
                Console.WriteLine("Selected task is not defined or stopped.");
            }
        }

        static void WorkerStarter(Rule selectedRule)
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

                            if (selectedRule.SearchedContent != "" && selectedRule.Type == "sc")
                            {
                                if (webSiteContent.IndexOf(selectedRule.SearchedContent) != -1)
                                {
                                    string Message = "RULE WORKER >> <RULE:" + selectedRule.RuleID + "> Searched content founded";
                                    Console.WriteLine(Message);
                                    MessageBox.Show(Message);

                                    selectedRule.Running = 0;
                                    break;
                                }
                            }

                            if (selectedRule.Hash != hash && selectedRule.Type == "cc" && selectedRule.SearchedContent == "")
                            {
                                string Message = "RULE WORKER >> <RULE:" + selectedRule.RuleID + "> Changes are detected in content";
                                Console.WriteLine(Message);
                                MessageBox.Show(Message);
                                selectedRule.Hash = hash;

                                selectedRule.Running = 0;
                                break;
                            }
                            else if (selectedRule.Type == "cc" && selectedRule.SearchedContent != "" && webSiteContent.IndexOf(selectedRule.SearchedContent) != -1)
                            {
                                string Message = "RULE WORKER >> <RULE:" + selectedRule.RuleID + "> The specified content was changed or removed.";
                                Console.WriteLine(Message);
                                MessageBox.Show(Message);

                                selectedRule.Running = 0;
                                break;
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
        }

        public static void writeRulesToConsole()
        {
            string Content = "";
            string NewLine = "\n";
            string Seperate = "│";
            string WhiteSpace = " ";
            int RunningNow = 0;
            int[] TopColumnsLength = new int[4] { 0, 0, 0, 0 };
            List<string[]> Rows = new List<string[]>();

            foreach (var item in Rules)
            {
                var RuleIDColumn = "RuleID: " + item.RuleID;
                var URLColumn = "URL: " + item.URL;
                var RefreshTimeColumn = "RefreshTime: " + item.RefreshTime;
                var RunningColumn = "isRunning: " + item.Running;
                var CurrentColumnsLength = new int[4] { RuleIDColumn.Length, URLColumn.Length, RefreshTimeColumn.Length, RunningColumn.Length };

                for (int i = 0; i < 4; i++)
                {
                    if (TopColumnsLength[i] < CurrentColumnsLength[i])
                    {
                        TopColumnsLength[i] = CurrentColumnsLength[i];
                    }
                }

                Rows.Add(new string[4] { RuleIDColumn, URLColumn, RefreshTimeColumn, RunningColumn });
                RunningNow += item.Running;
            }

            // Fill all columns and write
            foreach (var item in Rows)
            {
                var RowText = "";

                for (int i = 0; i < 4; i++)
                {
                    var FillValue = (TopColumnsLength[i] - item[i].Length) + 3;

                    for (int j = 0; j < FillValue; j++)
                    {
                        item[i] += WhiteSpace;
                    }

                    RowText += Seperate + WhiteSpace + item[i];
                }
                
                Content += NewLine + RowText + Seperate + NewLine;
            }

            Console.WriteLine(Content);

            Console.WriteLine("──────────────────────────────────────");
            Console.WriteLine("Total Rules: " + Rules.Count.ToString());
            Console.WriteLine("Running Now: " + RunningNow.ToString());
            Console.WriteLine("Idled: " + (Rules.Count - RunningNow).ToString());
            Console.WriteLine("──────────────────────────────────────");
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

        public static void SaveRules()
        {
            var RulesFile = File.OpenWrite(RulesFileName);
            var databyte = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Rules));

            RulesFile.BeginWrite(databyte, 0, databyte.Length, new AsyncCallback(AfterWriteCallback), RulesFile);
        }

        static void AfterWriteCallback(IAsyncResult R)
        {
            FileStream state = (FileStream)R.AsyncState;

            state.EndWrite(R);
            state.Dispose();

            Console.WriteLine("Rules saved successfully");
        }

        public static void LoadRules()
        {
            var RulesFile = File.OpenRead(RulesFileName);

            RulesFile.BeginRead(JsonBuffer, 0, JsonBuffer.Length, new AsyncCallback(AfterReadCallback), RulesFile);
        }

        static void AfterReadCallback(IAsyncResult R)
        {
            FileStream state = (FileStream)R.AsyncState;

            int readedcontentlength = state.EndRead(R);

            if (R.IsCompleted)
            {
                byte[] ResultArr = new byte[readedcontentlength];

                Array.Copy(JsonBuffer, ResultArr, readedcontentlength);

                Rules = JsonConvert.DeserializeObject<List<Rule>>(Encoding.ASCII.GetString(ResultArr));
                state.Dispose();

                Console.WriteLine("{0} Rules loaded successfully", Rules.Count);
            }
        }
    }
}
