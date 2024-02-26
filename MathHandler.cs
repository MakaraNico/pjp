using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pjp_cv1
{
    public static class MathHandler
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

        public static string RemoveSpaces(string line)
        {
            return line.Replace(" ", "");
        }

        /*
        public static int CheckValid(string line)
        {
            int left_brackets_count = 0;
            int right_brackets_count = 0;
            int sign_count = 0;
            int num_count = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    left_brackets_count++;
                } else if (line[i] == ')')
                {
                    right_brackets_count++;
                } else if (line[i] == '+' || line[i] == '-' || line[i] == '*' || line[i] == '/')
                {
                    sign_count++;
                }
            }

            string tmp_line = line.Replace("(", "");
            tmp_line = tmp_line.Replace(")", "");
            string[] numbers = tmp_line.Split('+', '-', '*', '/');
            for (int i = 0; i < numbers.Length; i++)
            {
                try
                {
                    Int32.Parse(numbers[i]);
                    num_count++;
                }
                catch (FormatException)
                {
                    return 0;
                }
            }

            if (left_brackets_count != right_brackets_count)
            {
                return 0;
            } 
            else if (num_count - 1 != sign_count)
            {
                return 0;
            }

            return 1;
        }

        public static int GetFinalResult(string line)
        {
            //
            List<string> result = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    string tmp = line[i + 1].ToString() + line[i + 2].ToString();
                    result.Add(tmp);
                    i = i + 3;
                } else
                {
                    result.Add(line[i].ToString());
                }
            }


            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(result[i]);
            }
            //
            
            
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    var sb = new StringBuilder(line);
                    sb[i + 1] = '~';
                    //line[i + 1] = '~';
                    line = sb.ToString();
                }
            }

            string[] line_split = line.Split('+', '-', '*');


            for (int i = 0; i < line_split.Length; i++)
            {
                if (line_split[i].Length > 1)
                {                    
                    if (line_split[i][1] != '(')
                    {
                        //string[] tmp_split = line_split[i].Split('/');

                    }
                }
            }



            return 1;
        }
        */

        public static int CheckValidLine(string line)
        {
            int num_left_brackets = 0;
            int num_right_brackets = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    num_left_brackets++;
                }
                else if (line[i] == ')')
                {
                    num_right_brackets++;
                }

                if (num_left_brackets < num_right_brackets)
                {
                    return 0;
                }
            }

            if (num_left_brackets != num_right_brackets)
            {
                return 0;
            }

            line = line.Replace("(", "");
            line = line.Replace(")", "");

            int num_signs = 0;
            int num_numbers = 0;
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] == '+' || line[i] == '-' || line[i] == '*' || line[i] == '/')
                {
                    num_signs++;
                }
                else
                {
                    try
                    {
                        int.Parse(line[i].ToString());
                        num_numbers++;
                    }
                    catch (FormatException)
                    {
                        return 0;
                    }
                }
            }

            if (num_signs + 1 != num_numbers)
            {
                return 0;
            }

            return 1;
        }

        public static string DoCalculation(string line)
        {
            int num_left_brackets = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    num_left_brackets++;
                }
            }

            int actual_left_bracket = 0;
            int tmp_num_left_brackets = num_left_brackets;
            for (int i = 0; i < num_left_brackets; i++)
            {
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j] == '(')
                    {
                        actual_left_bracket++;
                        if (actual_left_bracket == tmp_num_left_brackets)
                        {
                            string substring = null;

                            int iterator = j;
                            while (line[iterator] != ')')
                            {
                                substring += line[iterator].ToString();
                                iterator++;
                            }
                            substring += line[iterator].ToString();
                            tmp_num_left_brackets--;
                            actual_left_bracket = 0;

                            //Console.WriteLine(substring);

                            line = line.Replace(substring, MathHandler.CalculateMathExpression(substring));
                            if (line == "ERROR")
                            {
                                return "ERROR";
                            }

                            break;

                        }
                    }
                }
            }

            line = MathHandler.CalculateMathExpression(line);

            if (line.Length > 1)
            {
                try
                {
                    int res = int.Parse(line[1].ToString());
                    res *= -1;
                    return res.ToString();

                }
                catch (FormatException)
                {

                    return "ERROR";
                }
            }
            return line;
        }

        private static string CalculateMathExpression(string line)
        {
            line = line.Replace("(", "");
            line = line.Replace(")", "");

            line = DoDivision(line);
            if (line == "ERROR")
            {
                return "ERROR";
            }
            line = DoMultiplication(line);
            if (line == "ERROR")
            {
                return "ERROR";
            }
            line = DoSubstraction(line);
            if (line == "ERROR")
            {
                return "ERROR";
            }
            line = DoAddition(line);
            if (line == "ERROR")
            {
                return "ERROR";
            }

            return line;
        }

        private static string DoDivision(string line)
        {
            string[] splits = line.Split('+', '-', '*');

            int res = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                if (splits[i].Length > 2)
                {
                    string[] sub_splits = splits[i].Split('/');
                    for (int j = 0; j < sub_splits.Length; j++)
                    {
                        if (j == 0)
                        {
                            try
                            {
                                if (sub_splits[j][0] == '~')
                                {
                                    res = int.Parse(sub_splits[j][1].ToString());
                                    res = res * -1;
                                } else
                                {
                                    res = int.Parse(sub_splits[j]);
                                }
                                continue;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        if (sub_splits[j][0] == '~')
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j][1].ToString());
                                num *= -1;
                                res = res / num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        else
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j]);
                                res = res / num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                    }
                    if (res < 0)
                    {
                        res *= -1;
                        string tmp_res = "~" + res.ToString();
                        line = line.Replace(splits[i], tmp_res);
                    }
                    else
                    {
                        line = line.Replace(splits[i], res.ToString());
                    }
                }
            }
            return line;
        }

        private static string DoMultiplication(string line)
        {
            string[] splits = line.Split('+', '-');

            int res = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                if (splits[i].Length > 2)
                {
                    string[] sub_splits = splits[i].Split('*');
                    for (int j = 0; j < sub_splits.Length; j++)
                    {
                        if (j == 0)
                        {
                            try
                            {
                                if (sub_splits[j][0] == '~')
                                {
                                    res = int.Parse(sub_splits[j][1].ToString());
                                    res = res * -1;
                                }
                                else
                                {
                                    res = int.Parse(sub_splits[j]);
                                }
                                continue;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        if (sub_splits[j][0] == '~')
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j][1].ToString());
                                num *= -1;
                                res = res * num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        else
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j]);
                                res = res * num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                    }
                    if (res < 0)
                    {
                        res *= -1;
                        string tmp_res = "~" + res.ToString();
                        line = line.Replace(splits[i], tmp_res);
                    }
                    else
                    {
                        line = line.Replace(splits[i], res.ToString());
                    }
                }
            }
            return line;
        }

        private static string DoSubstraction(string line)
        {
            string[] splits = line.Split('+');

            int res = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                if (splits[i].Length > 2)
                {
                    string[] sub_splits = splits[i].Split('-');
                    for (int j = 0; j < sub_splits.Length; j++)
                    {
                        if (j == 0)
                        {
                            try
                            {
                                if (sub_splits[j][0] == '~')
                                {
                                    res = int.Parse(sub_splits[j][1].ToString());
                                    res = res * -1;
                                }
                                else
                                {
                                    res = int.Parse(sub_splits[j]);
                                }
                                continue;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        if (sub_splits[j][0] == '~')
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j][1].ToString());
                                num *= -1;
                                res = res - num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                        else
                        {
                            try
                            {
                                int num = int.Parse(sub_splits[j]);
                                res = res - num;
                            }
                            catch (FormatException)
                            {
                                return "ERROR";
                            }
                        }
                    }
                    if (res < 0)
                    {
                        res *= -1;
                        string tmp_res = "~" + res.ToString();
                        line = line.Replace(splits[i], tmp_res);
                    }
                    else
                    {
                        line = line.Replace(splits[i], res.ToString());
                    }
                }
            }
            return line;
        }

        private static string DoAddition(string line)
        {
            string[] splits = line.Split('+');

            int res = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                if (splits[i][0] == '~')
                {
                    try
                    {
                        int tmp_res = int.Parse(splits[i][1].ToString());
                        tmp_res = tmp_res * -1;
                        res += tmp_res;
                        continue;
                    }
                    catch (FormatException)
                    {
                        return "ERROR";
                    }
                } else
                {
                    int tmp_res = int.Parse(splits[i]);
                    res += tmp_res;
                }
            }
            if (res < 0)
            {
                res *= -1;
                return "~" + res.ToString();
            } else
            {
                return res.ToString();
            }
        }
    }
}