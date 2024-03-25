using System.Text;

namespace RegularExpressionsForUniLab;

public class MainClass
{
    public static void Main()
    {
        RegEx regEx = new RegEx();
        Console.WriteLine(regEx.GetFirst());
        Console.WriteLine(regEx.GetSecond());
        Console.WriteLine(regEx.GetThird());
    }
    public class RegEx
    {
        public string GetFirst()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('O');
            Console.Write(sb + "  ");
            var rand = new Random().Next(3);
            var str = "";
            switch (rand)
            {
                case(0):
                    str = "P";
                    break;
                case(1):
                    str = "Q";
                    break;
                case(2):
                    str = "R";
                    break;
                default:
                    throw new Exception();
            }

            rand = new Random().Next(1, 6);
            for (int i = 0; i < rand; i++)
            {
                sb.Append(str);
                Console.Write(sb + "  ");
            }

            sb.Append('2');
            Console.Write(sb + "  ");

            rand = new Random().Next(2);
            sb.Append(rand == 0 ? 3 : 4);
            Console.Write(sb + "  ");

            return sb.ToString();
        }
        public string GetSecond()
        {
            StringBuilder sb = new StringBuilder();
            var str = "";
            var rand = new Random().Next(6);
            for (int i = 0; i < rand; i++)
            {
                sb.Append('A');
                Console.Write(sb + "  ");

            }

            sb.Append('B');
            Console.Write(sb + "  ");

            rand = new Random().Next(3);
            switch (rand)
            {
                case(0):
                    str = "C";
                    break;
                case(1):
                    str = "D";
                    break;
                case(2):
                    str = "E";
                    break;
                default:
                    throw new Exception();
            }

            sb.Append(str);
            Console.Write(sb + "  ");
            
            sb.Append('F');
            Console.Write(sb + "  ");

            rand = new Random().Next(3);
            switch (rand)
            {
                case(0):
                    str = "G";
                    break;
                case(1):
                    str = "H";
                    break;
                case(2):
                    str = "I";
                    break;
                default:
                    throw new Exception();
            }
                sb.Append(str);
                Console.Write(sb + "  ");

                sb.Append(str);
                Console.Write(sb + "  ");

            return sb.ToString();
        }
        public string GetThird()
        {
            StringBuilder sb = new StringBuilder();
            var str = "";
            var rand = new Random().Next(1,6);
            for (int i = 0; i < rand; i++)
            {
                sb.Append('J');
                Console.Write(sb + "  ");
            }

            sb.Append('K');
            Console.Write(sb + "  ");

            rand = new Random().Next(3);
            switch (rand)
            {
                case(0):
                    str = "L";
                    break;
                case(1):
                    str = "M";
                    break;
                case(2):
                    str = "N";
                    break;
                default:
                    throw new Exception();
            }
            rand = new Random().Next(6);
            for (int i = 0; i < rand; i++)
            {
                sb.Append(str);
                Console.Write(sb + "  ");
            }

            rand = new Random().Next(2);
            if (rand == 1)
            {
                sb.Append('O');
                Console.Write(sb + "  ");
            }
            rand = new Random().Next(2);
            switch (rand)
            {
                case(0):
                    str = "P";
                    break;
                case(1):
                    str = "Q";
                    break;
                default:
                    throw new Exception();
            }
            sb.Append(str);
            Console.Write(sb + "  ");
            sb.Append(str);
            Console.Write(sb + "  ");
            sb.Append(str);

            return sb.ToString();
        }


    }

}
