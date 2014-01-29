using System;
using System.Collections.Generic;
using System.Linq;

namespace XNAContentCompiler.Console
{
    public class ConsoleExecutor
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, Command> _commands;

        public ConsoleExecutor(ILogger logger)
        {
            _commands = new Dictionary<string, Command>();
            _logger = logger;
        }

        public bool Execute(CommandInputModel inputModel)
        {
            var command = inputModel.Command;
            var success = false;

            if (_commands.ContainsKey(command))
            {
                var commandDefinition = _commands[command];
                var arguments = inputModel.Arguments;

                try
                {
                    commandDefinition.TryExecute(_logger, arguments);
                    success = true;
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
            }
            else
                _logger.Error("{0} is not a valid command.", command);
            return success;
        }

        public void AddCommand(string commandName, Action<ILogger, string[]> executionFunc, Func<string[], bool> validateArgs = null, string description = null)
        {
            if(string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("commandName");

            if(executionFunc == null)
                throw new ArgumentException("executionFunc");

            var finalCommandString = commandName.ToUpper();

            _commands.Add(finalCommandString, new Command(description, executionFunc, validateArgs));
        }

        public IEnumerable<string> CommandDescriptions
        {
            get { return _commands.Select(x => x.Value.Description).Where(x => !String.IsNullOrWhiteSpace(x)); }
        }

        struct Command
        {
            private readonly Action<ILogger, string[]> _execFunc;
            private readonly Func<string[], bool> _validator;
            private readonly string _description;

            public Command(string description, Action<ILogger, string[]> executor, Func<string[], bool> validator )
            {
                _description = description;
                _execFunc = executor;
                _validator = validator;
            }

            public void TryExecute(ILogger logger, string[] arguments)
            {
                if (_validator == null || _validator(arguments))
                    _execFunc(logger, arguments);
            }

            public string Description
            {
                get { return _description; }
            }
        }
    }
}
