using System.Collections.Generic;

namespace BonetIDE
{
    class MergeSpaceCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            ICommand cmd = new MergeCommand(" ");

            return cmd.Execute(context, arguments);
        }
    }
}