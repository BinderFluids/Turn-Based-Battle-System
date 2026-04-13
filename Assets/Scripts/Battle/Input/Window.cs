using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Input
{
    /// <summary>
    /// Shared data for timed action commands and responsive input windows.
    /// </summary>
    public abstract class Window
    {
        public string Id { get; protected set; }
        
        public List<InputType> ExpectedInputs { get; }

        protected Window(string id, List<InputType> expectedInputs)
        {
            Id = id;
            ExpectedInputs = expectedInputs;
        }

        public float OpenTime { get; private set; }
        public void Open()
        {
            OpenTime = Time.time;
        }
        public float Elapsed => Time.time - OpenTime;
        public abstract void HandleInput(WindowInputEvent evt);
    }
}
