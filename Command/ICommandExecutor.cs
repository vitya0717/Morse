using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Command
{
    public interface ICommandExecutor
    {
        public bool OnCommand(ConsoleSender sender, string commandName, string[] args);
    }
}
