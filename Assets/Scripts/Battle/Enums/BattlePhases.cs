namespace Battle.Enums
{
    public enum BattlePhases
    {
        /// <summary>
        /// The first phase of the battle.
        /// </summary>
        Intro,
        
        /// <summary>
        /// Phase before control is given to an entity
        /// </summary>
        StartTurn,
        
        /// <summary>
        /// Phase where an entity can select an action and target.
        /// </summary>
        SelectingAction,
        
        /// <summary>
        /// Phase where an entity can select a target.
        /// </summary>
        SelectingTarget,
        
        /// <summary>
        /// Phase where an entity is performing their action; 
        /// </summary>
        PerformingAction,
        
        /// <summary>
        /// Phase after an entity is done with their control
        /// </summary>
        EndTurn,
    }
}