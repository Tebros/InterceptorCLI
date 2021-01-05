using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interceptor;

namespace InterceptorCLI
{
    class Program
    {
        
        static void Main(string[] args)
        {

            CmdHandler cmdHandler = new CmdHandler();

            string line = Console.ReadLine();
            CmdAction action;
            while (line != null)
            {
                action = cmdHandler.Handle(line);
                Console.WriteLine(action);

                switch (action)
                {
                    case CmdAction.OK:
                    case CmdAction.TRUE:
                    case CmdAction.FALSE:
                    case CmdAction.ERROR:
                        break;
                    case CmdAction.EXIT:
                        return;
                }

                line = Console.ReadLine();
            }
        }
    }
}
