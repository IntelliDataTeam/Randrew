using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randrew
{
    class Secrets
    {
        public static string bunnyEmotion()
        {
            Random random = new Random();
            int ranGen = random.Next(0, 12);
            //int ranGen = 8;
            string[] emo = storage(ranGen);
            
            return string.Join(Environment.NewLine, emo);
        }

        public static string bunnyEmotion(int x)
        {
            string[] emo = storage(x);

            return string.Join(Environment.NewLine, emo);
        }

        public static string[] storage(int x)
        {
            string[] emo = new string[4];
            switch (x)
            {
                case 0:
                    emo[0] = "(" + '\\' + ' ' + ' ' + '/' + ')';
                    emo[1] = "( .  .)";
                    emo[2] = "C(\") (\")";
                    break;

                case 1:
                    emo[0] = "(\\_/)";
                    emo[1] = "(0_0)";
                    emo[2] = "C(\") (\")";
                    break;

                case 2:
                    emo[0] = "( )_( )";
                    emo[1] = "(='.'=)";
                    emo[2] = "(\")_(\")";
                    break;

                case 3:
                    emo[0] = "( Y)";
                    emo[1] = "(  . .)";
                    emo[2] = "o(\") (\")";
                    break;

                case 4:
                    emo[0] = "(\\-/)";
                    emo[1] = "(='.'=)";
                    emo[2] = "(\")-(\")o";
                    break;

                case 5:
                    emo[0] = " /)_/)";
                    emo[1] = "( .  .)";
                    emo[2] = "C(\") (\")";
                    break;

                case 6:
                    emo[0] = "( )   ( )";
                    emo[1] = "(>•.•<)";
                    emo[2] = "(\")    (\")";
                    break;

                case 7:
                    emo[0] = " (\\(\\";
                    emo[1] = "(=' :')";
                    emo[2] = "   (, (\") (\")";
                    break;

                case 8:
                    emo[0] = "(\\_/)";
                    emo[1] = "(^_^)";
                    emo[2] = "  (___)O";
                    break;

                case 9:
                    emo[0] = "()  ()";
                    emo[1] = "(=\"=)";
                    emo[2] = "(    .    )";
                    emo[3] = "c((\")  (\")";
                    break;

                case 10:
                    emo[0] = "(\\_/)";
                    emo[1] = "(-_-)";
                    emo[2] = "<=( 0 )=>";
                    emo[3] = "(\").|.(\")";
                    break;

                case 11:
                    emo[0] = "(\\_/)";
                    emo[1] = "(o.o)";
                    emo[2] = "/ (   ) \\";
                    emo[3] = "/_|_\\";
                    break;

                case 12:
                    emo[0] = "(" + '\\' + ' ' + ' ' + '/' + ')';
                    emo[1] = "( .  .)";
                    emo[2] = ">[Working...]<";
                    emo[3] = "C(\") (\")";
                    break;

                default:
                    break;
            }
            return emo;
        }
    }
}
