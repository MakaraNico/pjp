using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pjp_cv1
{
    public static class ParsingHandler
    {
        public static int ParseNumLines()
        {
            string num_string = Console.ReadLine();
            int lines_num = 0;
            try
            {
                lines_num = int.Parse(num_string);
            }
            catch (FormatException)
            {
                Console.WriteLine("Bad lines count");
                return 0;
            }
            return lines_num;
        }
        public static List<string> ReadLines(int lines_num)
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < lines_num; i++)
            {
                string line = Console.ReadLine();
                lines.Add(line);
            }

            return lines;

        }
        public static List<string> RemoveSpaces(List<string> lines)
        {
            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = lines[i].Replace(" ", "");
            }
            return lines;
        }

        public static int CountBrackets(string line)
        {
            int left_brackets_count = 0;
            int right_brackets_count = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    left_brackets_count++;
                } else if (line[i] == ')')
                {
                    right_brackets_count++;
                }
            }
            
            if (left_brackets_count != right_brackets_count)
            {
                return 0;
            }
            return 1; 
        }

        public static int CheckForCorrectLine(string line)
        {
            if (ParsingHandler.CountBrackets(line) == 0)
            {
                return 0;
            }


            line = line.Replace("(", "");
            line = line.Replace(")", "");

            string pattern = "([+\\-*/])";
            string[] res = Regex.Split(line, pattern);

            if (res.Length > 1)
            {
                if (res[0] == "" || res[res.Length - 1] == "")
                {
                    return 0;
                }
            }

            int delim_count = 0;
            int num_count = 0;

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == "+" || res[i] == "-" || res[i] == "*" || res[i] == "/")
                {
                    delim_count++;
                } else
                {
                    bool is_num = int.TryParse(res[i], out int number);
                    if (is_num)
                    {
                        num_count++;
                    }
                }
            }

            if (delim_count + 1 != num_count)
            {
                return 0;
            }

            return 1;
        }

        public static int SolveLine(string line)
        {
            if (ParsingHandler.CheckForCorrectLine(line) == 0)
            {
                return 0;
            }


            string[] res = line.Split('(', ')');

            if (res.Length <= 1)
            {
                return ParsingHandler.DoCalculation(line);

            } else
            {
                while(res.Length > 1)
                {
                    for (int i = 0; i < res.Length; i++)
                    {
                        if (res[i].Length > 2)
                        {
                            int ret = ParsingHandler.DoCalculation(res[i]);

                            string remove = "(" + res[i] + ")";
                            line = line.Replace(remove, ret.ToString());
                        }
                    }
                    res = line.Split('(', ')');
                }
                return ParsingHandler.DoCalculation(line);
            }
        }

        public static string DoDivision(string line)
        {
            string[] res = line.Split('+', '-', '*');
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].Length > 1)
                {
                    string[] subres = res[i].Split("/");
                    if (subres.Length > 1)
                    {
                        try
                        {
                            int num1 = int.Parse(subres[0]);
                            int num2 = int.Parse(subres[1]);

                            int res_num = num1 / num2;

                            line = line.Replace(res[i], res_num.ToString());
                        }
                        catch (FormatException)
                        {
                            return null;
                        }

                    }
                }
            }
            return line;
        }

        public static string DoMultiplication(string line)
        {
            string[] res = line.Split('+', '-');
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].Length > 1)
                {
                    string[] subres = res[i].Split("*");
                    if (subres.Length > 1)
                    {
                        try
                        {
                            int num1 = int.Parse(subres[0]);
                            int num2 = int.Parse(subres[1]);

                            int res_num = num1 * num2;

                            line = line.Replace(res[i], res_num.ToString());
                        }
                        catch (FormatException)
                        {
                            return null;
                        }

                    }
                }
            }
            return line;
        }

        public static string DoSubstraction(string line)
        {
            string[] res = line.Split('+');
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].Length > 1)
                {
                    string[] subres = res[i].Split("-");
                    if (subres.Length > 1)
                    {
                        try
                        {
                            int num1 = int.Parse(subres[0]);
                            int num2 = int.Parse(subres[1]);

                            int res_num = num1 - num2;

                            line = line.Replace(res[i], res_num.ToString());
                        }
                        catch (FormatException)
                        {
                            return null;
                        }

                    }
                }
            }
            return line;
        }

        public static string DoAddition(string line)
        {
            int res_num = 0;
            string[] res = line.Split('+');
            for (int i = 0; i < res.Length; i++)
            {
                try
                {
                    int num = int.Parse(res[i]);
                    res_num += num;
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            return res_num.ToString();
        }

        public static int DoCalculation(string line)
        {
            if ((line = ParsingHandler.DoDivision(line)) == null)
            {
                return 0;
            }
            if ((line = ParsingHandler.DoMultiplication(line)) == null)
            {
                return 0;
            }
            if ((line = ParsingHandler.DoSubstraction(line)) == null)
            {
                return 0;
            }
            if ((line = ParsingHandler.DoAddition(line)) == null)
            {
                return 0;
            }

            try
            {
                int res_num = int.Parse(line);
                return res_num;
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}
