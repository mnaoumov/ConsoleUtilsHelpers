using System;
using Mono.Options;

namespace ConsoleUtilsHelpers
{
    class Program : ConsoleUtilBase
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        protected override OptionSet OptionSet
        {
            get
            {
                return new OptionSet
                {
                    { "d|doAction", "Do action", v => DoAction = v != null },
                    { "p|parameter1=", "Parameter1", v => Parameter1 = v }
                };
            }
        }

        string Parameter1 { get; set; }
        bool DoAction { get; set; }

        protected override void RunInternal()
        {
            Console.WriteLine("Parameter1: {0}", Parameter1);
            Console.WriteLine("DoAction: {0}", DoAction);
        }

        protected override string Title
        {
            get { return "Console Util Helper (c) Michael Naumov"; }
        }

        protected override bool ContinueIfNoArguments
        {
            get { return true; }
        }

        protected override bool CaseSensitiveParsing
        {
            get { return false; }
        }
    }
}
