using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BonetIDE
{
    class NormalizeDecomposedCommand : ICommand
    {
        public ICommandResult Execute(IContext context, List<ICommandArgument> arguments)
        {
            NormalizeComposedCommand c = new();

            return c.Execute(context, arguments, NormalizationForm.FormD);
        }
    }
}