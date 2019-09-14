﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.Framework
{

    public static class Constants
    {
        //DO NOT CHANGE THESE 3 STRINGS. Every class using an FSM uses these strings to find out which methods to invoke through reflection.
        // hmmmm... i shall make a better system for this if i have the time.
        public const string FSMInitPrefix = "Init";
        public const string FSMUpdatePrefix = "Update";
        public const string FSMTerminatePrefix = "Terminate";

        //dont have the time or really the necessity to create a language retrieval system.
        //thisll have to do for now
        public const string countdownGoText = "GO!";


        public const float globalAnimationSpeed = 1.0f;
    }
}
