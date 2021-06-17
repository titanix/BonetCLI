namespace BonetIDE
{
    class CommandResult : ICommandResult
    {
        bool success;

        public CommandResult(bool success)
        {
            this.success = success;
        }
    }
}