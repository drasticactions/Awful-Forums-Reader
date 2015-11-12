using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Forums
{
    public class ClearJumpListCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            await JumpListCreator.CreateDefaultList(true);
        }
    }
}
