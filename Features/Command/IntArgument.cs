namespace BonetIDE
{
    struct IntArgument : ICommandArgument
    {
        int argument;

        internal IntArgument(int arg)
        {
            argument = arg;
        }

        public int Value => argument;
    }
}