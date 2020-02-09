using System;

namespace Toolbox.Assets
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException(string message) : base(message)
        {

        }
    }
}
