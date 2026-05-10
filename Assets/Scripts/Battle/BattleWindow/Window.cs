using System.Collections.Generic;
using Core.Enums;
using UnityEngine;
using EventBus;
using Battle.Events.Windows;

namespace Battle.Window
{
    /// <summary>
    /// Shared data for timed action commands and responsive input windows.
    /// </summary>
    public abstract class Window
    {
        public string Id { get; protected set; }
        public int StartFrame { get; private set; }
        
        
        HashSet<PlayerId> expectedInputs = new HashSet<PlayerId>();
        public IReadOnlyCollection<PlayerId> ExpectedInputs => expectedInputs; 

        protected Window(string id, IEnumerable<PlayerId> expectedInputs)
        {
            Id = id;
            foreach (var playerId in expectedInputs)
                this.expectedInputs.Add(playerId);
        }

        public void Open()
        {
            Debug.Log($"Opening {Id}");
            StartFrame = Time.frameCount;
        }
        protected virtual void OnOpen() {}

        public void Close()
        {
            
        }
        protected virtual void OnClose() {}
        
        public int Elapsed => Time.frameCount - StartFrame;
        public abstract void HandleInput(PlayerId player, bool isPressed);
    }
}
