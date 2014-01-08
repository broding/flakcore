using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Flakcore
{
    public class Input
    {
        private GamePadState[] PreviousGamepadStates = new GamePadState[4];
		private KeyboardState PreviousKeyboardState;

        private Dictionary<PlayerIndex, TimeSpan> vibratingTime = new Dictionary<PlayerIndex, TimeSpan>()
        {
            { PlayerIndex.One, TimeSpan.Zero },
            { PlayerIndex.Two, TimeSpan.Zero },
            { PlayerIndex.Three, TimeSpan.Zero },
            { PlayerIndex.Four, TimeSpan.Zero }
        };

        private Dictionary<PlayerIndex, bool> Vibrating = new Dictionary<PlayerIndex, bool>()
        {
            { PlayerIndex.One, false },
            { PlayerIndex.Two, false },
            { PlayerIndex.Three, false },
            { PlayerIndex.Four, false }
        };

        public void Update(GameTime gameTime)
        {
			this.PreviousKeyboardState = Keyboard.GetState();

            for (int i = 0; i < Vibrating.Count; i++)
            {
                if (Vibrating[(PlayerIndex)i])
                {
                    vibratingTime[(PlayerIndex)i] -= gameTime.ElapsedGameTime;
                    if (vibratingTime[(PlayerIndex)i].TotalMilliseconds <= 0)
                    {
                        GamePad.SetVibration((PlayerIndex)i, 0, 0);
                        Vibrating[(PlayerIndex)i] = false;
                    }
                }
            }

        }

        public void SetVibrationWithTimer(PlayerIndex player, TimeSpan time, float vibrationStrength = 1f)
        {
            vibratingTime[player] = time;
            this.Vibrating[player] = true;
            GamePad.SetVibration(player, vibrationStrength, vibrationStrength);
        }

        public bool JustPressed(Keys key)
        {
			KeyboardState currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(key) && this.PreviousKeyboardState.IsKeyUp(key))
                return true;
            else
                return false;
        }
    }
}
