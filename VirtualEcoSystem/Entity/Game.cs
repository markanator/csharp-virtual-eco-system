using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualEcoSystem
{
    [Serializable]
    public abstract class Game
    {
        protected Boolean IsPlaying;

        public virtual void Setup()
        {
            // implement later
        }

        public virtual void StartGame()
        {
            this.IsPlaying = false;
        }
    }
}
