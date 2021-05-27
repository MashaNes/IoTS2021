using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandMicroservice.Entities
{
    public class ActuationCommand
    {
        public string Command { get; set; }
        public List<string> AdditionalArguments { get; set; }

        public ActuationCommand()
        {
            AdditionalArguments = new List<string>();
        }
    }
}
