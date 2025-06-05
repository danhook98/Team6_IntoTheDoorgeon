namespace DoorGame.EventSystem
{
    public class VoidEventTrigger : AbstractEventTrigger<Empty>
    {
        // Parameterless version of the Trigger method to prevent issues with 'empty' data. 
        public void Trigger() => eventToTrigger.Invoke(new Empty());
    }
}
