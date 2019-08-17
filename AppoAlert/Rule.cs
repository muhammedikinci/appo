using System;
using System.Collections.Generic;
using System.Text;

namespace AppoAlert
{
    public class Rule
    {
        public int RuleID { get; set; }
        public string URL { get; set; }
        public string SourceContent { get; set; }
        public string SearchedContent { get; set; }
        public string Hash { get; set; }
        public int RefreshTime { get; set; }
        public int Running { get; set; }
        public string Type { get; set; }
    }
}
