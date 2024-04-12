using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Input
{
    public partial class @GameControl
    {
        public static GameControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameControl();
                    _instance.Enable();
                }
                return _instance;
            }
        }

        private static @GameControl _instance;
    }
}