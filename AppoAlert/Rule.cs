using System;
using System.Collections.Generic;
using System.Text;

namespace AppoAlert
{
    public class Rule
    {
        public int RuleID { get; set; }
        public string URL { get; set; }
        public string Content { get; set; }
        public int RefreshTime { get; set; }
        public int Running { get; set; }
    }
}
