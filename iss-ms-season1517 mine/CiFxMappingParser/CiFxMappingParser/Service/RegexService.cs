using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CiFxMappingParser.Service
{
    public class RegexService
    {
        public string[] GetValues(string source, string pattern)
        {
            //get first match only
            var resReg = Regex.Matches(source, pattern);
            if (resReg != null)
            {
                var result = new HashSet<string>();
                foreach (Match match in resReg)
                {
                    int index = 0;
                    foreach (Group group in match.Groups)
                    {
                        if (!string.IsNullOrEmpty(group.Value)
                            && ((group.Value != source || index != 0) || match.Groups.Count == 1))
                        {
                            result.Add(group.Value);
                        }
                        index++;
                    }
                }

                return result.ToArray();
            }

            return null;
        }

        public string ReplaceValues(string source, string pattern, string[] values)
        {
            var resReg = Regex.Replace(source, pattern, (match) =>
            {
                var result = match.Value;
                int processed = 0;
                int padding = 0;
                foreach (Group group in match.Groups)
                {
                    if (group.Value != source)
                    {
                        string newValue = values[processed];
                        result = result.Remove(group.Index + padding, group.Value.Length).Insert(group.Index + padding, newValue);

                        padding = newValue.Length - group.Value.Length;
                        processed++;
                    }
                }
                return result;
            });

            return resReg;
        }
    }
}