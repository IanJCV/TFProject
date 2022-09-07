using System.Collections.Generic;
using UnityEngine;

namespace TF.Input
{
    public class PlayerCommands
    {
        public float forward;
        public float right;
        public float up;

        public Vector2 viewAngles;
        public int weaponSelect;

        public bool crouch;
        public bool sprint;

        public KeyAxis fwdBind;
        public KeyAxis rgtBind;
        public KeyAxis jumpBind;
        public KeyAxis crouchBind;
    }

    public struct KeyAxis
    {
        internal KeyCode positive;
        internal KeyCode negative;
    }
}