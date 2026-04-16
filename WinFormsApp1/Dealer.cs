using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackOOP
{
    public class Dealer : Player
    {
        public Dealer() : base("Dealer (U zelf)")
        {
            
        }

        public override string GetMoveOpinion()
        {
            int score = GetCurrentHandValue();
            if (score < 17)
            {
                return "Hit";
            }
            else
            {
                return "Stand";
            }
        }
    }
}
