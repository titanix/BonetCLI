namespace BonetIDE
{
    class StringArgument : ICommandArgument
    {
        string argument;

        internal StringArgument(string arg)
        {
            argument = arg;
        }

        public string Value => argument;

        public override string ToString()
        {
            return argument;
        }
    }
}