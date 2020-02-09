using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolbox.Input
{
    public class DelayedInputHandler
    {
        public event EventHandler<KeyEventArgs> InputFireEvent;

        private const int DefaultFirstWaitTime = 22;
        private const int DefaultContinuousWaitTime = 2;

        private readonly int _firstWaitTime;
        private readonly int _continuousWaitTime;

        private int _timer;

        private KeyboardState _lastState;
        private Keys _lastKey = Keys.None;

        private Keys[] _currentKeys = new Keys[0];

        private bool CanInvokeEvent => _timer <= 0;

        public DelayedInputHandler(int firstWaitTime = DefaultFirstWaitTime, int continuousWaitTime = DefaultContinuousWaitTime)
        {
            _firstWaitTime = firstWaitTime;
            _continuousWaitTime = continuousWaitTime;
        }

        public void Update(KeyboardState keyboardState)
        {
            DecrementTimer();

            _currentKeys = keyboardState.GetPressedKeys();

            Keys currentKey = _currentKeys.Length > 1 ? DifferenceBetween(_lastState.GetPressedKeys(), _currentKeys) : _currentKeys.Length > 0 ? _currentKeys[0] : Keys.None;

            if (_lastKey == Keys.None && currentKey != Keys.None && CanInvokeEvent)
            {
                _timer = _firstWaitTime;
                FireEvent(currentKey);
            }
            else if (_lastKey != Keys.None && _lastKey == currentKey && CanInvokeEvent)
            {
                _timer = _continuousWaitTime;
                FireEvent(currentKey);
            }
            else if (_lastKey != Keys.None && currentKey == Keys.None)
            {
                _timer = 0;
            }
            else if (_lastKey != Keys.None && _lastKey != currentKey)
            {
                _timer = _firstWaitTime;
                FireEvent(currentKey);
            }

            _lastKey = currentKey;
            _lastState = keyboardState;
        }

        private Keys DifferenceBetween(IEnumerable<Keys> oldKeys, IEnumerable<Keys> newKeys)
        {
            Keys diff = newKeys.Except(oldKeys).FirstOrDefault();
            return diff == Keys.None ? _lastKey : diff;
        }

        private void FireEvent(Keys key) => InputFireEvent?.Invoke(this, new KeyEventArgs(key));

        private void DecrementTimer()
        {
            if (_timer > 0) _timer--;
        }
    }
}
