using System;
using System.IO;
using Mono.Options;

namespace ConsoleUtilsHelpers
{
    public abstract class ConsoleUtilBase
    {
        protected abstract OptionSet OptionSet { get; }
        protected abstract void RunInternal();
        protected abstract string Title { get; }

        protected virtual void Run(string[] arguments)
        {
            Console.WriteLine(Title);
            Console.WriteLine();

            try
            {
                if (Parse(arguments))
                    RunInternal();
            }
            catch (Exception e)
            {
                if (Environment.ExitCode == 0)
                    Environment.ExitCode = 1;

                Console.Error.WriteLine(e);
            }
        }

        bool Parse(string[] arguments)
        {
            bool showHelp;
            var optionSet = new MyOptionSet(OptionSet, CaseSensitiveParsing);
            optionSet.Parse(arguments, out showHelp);

            if ((arguments.Length == 0 && !ContinueIfNoArguments) || showHelp)
            {
                string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                Console.WriteLine("Usage:\r\n{0} <options>", programName);
                optionSet.WriteOptionDescriptions(Console.Out);
            }

            return !showHelp;
        }

        protected virtual bool ContinueIfNoArguments
        {
            get { return false; }
        }

        protected virtual bool CaseSensitiveParsing { get { return true; } }

        class MyOptionSet : OptionSet
        {
            readonly bool _caseSensitiveParsing;

            public MyOptionSet(OptionSet optionSet, bool caseSensitiveParsing)
            {
                _caseSensitiveParsing = caseSensitiveParsing;
                foreach (var option in optionSet)
                    Add(option);
            }

            protected override string GetKeyForItem(Option item)
            {
                string key = base.GetKeyForItem(item);
                return _caseSensitiveParsing ? key : key.ToLower();
            }

            public void Parse(string[] arguments, out bool showHelp)
            {
                bool showHelp2 = false;
                Add("h|help|?", "Show Help", v => showHelp2 = v != null);
                Parse(arguments);
                showHelp = showHelp2;
            }

            protected override bool Parse(string argument, OptionContext c)
            {
                if (_caseSensitiveParsing)
                    return base.Parse(argument, c);
                
                string transformedArgument = c.Option == null ? argument.ToLower() : argument;
                return base.Parse(transformedArgument, c);
            }
        }
    }
}