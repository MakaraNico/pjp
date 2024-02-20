namespace pjp_cv1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> lines;
            int lines_num = ParsingHandler.ParseNumLines();
            if (lines_num == 0)
            {
                return;
            }
            lines = ParsingHandler.ReadLines(lines_num);
            lines = ParsingHandler.RemoveSpaces(lines);

            for (int i = 0; i < lines.Count(); i++)
            {
                int res = ParsingHandler.SolveLine(lines[i]);
                if (res == 0)
                {
                    Console.WriteLine("ERROR");
                    continue;
                }

                Console.WriteLine(res);
            }
        }
    }
}