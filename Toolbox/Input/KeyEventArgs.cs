using Microsoft.Xna.Framework.Input;
using System;

namespace Toolbox.Input
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; set; }

        public KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
