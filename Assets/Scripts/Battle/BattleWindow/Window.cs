using System;
using System.Collections.Generic;
using Battle.BattleEntity;
using Battle.BattleWindow.Enums;
using UnityEngine;

namespace Battle.BattleWindow
{
    /// <summary>
    /// Shared data for timed action commands and responsive input windows.
    /// </summary>
    public abstract class Window
    {
        public string Id { get; protected set; }
        
        public List<PlayerId> ExpectedInputs { get; }

        protected Window(string id, List<PlayerId> expectedInputs)
        {
            Id = id;
            ExpectedInputs = expectedInputs;
        }

        public int StartFrame { get; private set; }
        public void Open()
        {
            StartFrame = Time.frameCount;
        }

        public void Close()
        {
            
        }
        
        public float Elapsed => Time.frameCount - StartFrame;
        public abstract void HandleInput(PlayerId player, bool isPressed);
    }
}
