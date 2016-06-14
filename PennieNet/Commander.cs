using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PennieNet
{
    internal class Commander
    {
        public Commander()
        {
            // Connect to bluetooth
        } 
        public void IssueCmd(string input)
        {
            if (input.Equals("left"))
            {

            }
            else if (input.Equals("right"))
            {

            }
            else if (input.Equals("fwd"))
            {

            }
            else if (input.Equals("rev"))
            {

            }
            else
            {
                //STOP!!
            }
        }
    }
}
