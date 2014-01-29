using System.Linq;

namespace XNAContentCompiler.Console
{
    public class CommandInputModel
    {
        private readonly string[] _arguments;
        private readonly string _command;

        public CommandInputModel(string[] args)
        {
            _command = args[0].ToUpper();

            var arguments = args.Skip(1);
            _arguments = arguments.ToArray();
        }

        public string Command
        {
            get { return _command; }
        }

        public string[] Arguments
        {
            get { return _arguments; }
        }
    }
}