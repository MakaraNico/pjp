namespace pjp_cv1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> lines;
            int lines_num = MathHandler.ParseNumLines();
            if (lines_num == 0)
            {
                return;
            }
            lines = MathHandler.ReadLines(lines_num);

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = MathHandler.RemoveSpaces(lines[i]);
                if (MathHandler.CheckValidLine(lines[i]) == 0)
                {
                    Console.WriteLine("ERROR");
                    continue;
                }
                Console.WriteLine(MathHandler.DoCalculation(lines[i]));
            }
        }
    }
}