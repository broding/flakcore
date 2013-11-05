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
        private KeyboardState[] PreviousKeyboardStates = new KeyboardState[4];

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
            this.PreviousGamepadStates[(int)PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            this.PreviousGamepadStates[(int)PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            this.PreviousGamepadStates[(int)PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            this.PreviousGamepadStates[(int)PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            this.PreviousKeyboardStates[(int)PlayerIndex.One] = Keyboard.GetState(PlayerIndex.One);
            this.PreviousKeyboardStates[(int)PlayerIndex.Two] = Keyboard.GetState(PlayerIndex.Two);
            this.PreviousKeyboardStates[(int)PlayerIndex.Three] = Keyboard.GetState(PlayerIndex.Three);
            this.PreviousKeyboardStates[(int)PlayerIndex.Four] = Keyboard.GetState(PlayerIndex.Four);

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

        public List<GamePadState> getPadStateList
        {
            get
            {
                List<GamePadState> tempList = new List<GamePadState>();
                for (int i = 0; i < 4; i++)
                {
                    tempList.Add(GamePad.GetState((PlayerIndex)i));
                }
                return tempList;
            }
        }


        public GamePadState GetPadState(PlayerIndex player)
        {
            return GamePad.GetState(player);
        }

        public void SetVibrationWithTimer(PlayerIndex player, TimeSpan time, float vibrationStrength = 1f)
        {
            vibratingTime[player] = time;
            this.Vibrating[player] = true;
            GamePad.SetVibration(player, vibrationStrength, vibrationStrength);
        }

        public bool JustPressed(PlayerIndex player, Buttons button)
        {
            GamePadState currentState = GamePad.GetState(player);

            if(currentState.IsButtonDown(button) && this.PreviousGamepadStates[(int)player].IsButtonUp(button))
                return true;
            else
                return false;
        }

        public bool JustPressed(PlayerIndex player, Keys key)
        {
            KeyboardState currentState = Keyboard.GetState(player);

            if (currentState.IsKeyDown(key) && this.PreviousKeyboardStates[(int)player].IsKeyUp(key))
                return true;
            else
                return false;
        }

        public InputState GetInputState(PlayerIndex player)
        {
            InputState state = new InputState();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                state.X = -1;
            else if(keyboardState.IsKeyDown(Keys.Right))
                state.X = 1;

            if (keyboardState.IsKeyDown(Keys.Up))
                state.Y = -1;
            else if (keyboardState.IsKeyDown(Keys.Down))
                state.Y = 1;

            if (keyboardState.IsKeyDown(Keys.Space))
                state.Jump = true;

            if (keyboardState.IsKeyDown(Keys.X))
                state.Fire = true;

            return state;
        }
    }

    public struct InputState
    {
        public float X;
        public float Y;
        public bool Jump;
        public bool Fire;
    }
}
