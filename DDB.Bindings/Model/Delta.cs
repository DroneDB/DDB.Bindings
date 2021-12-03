namespace DDB.Bindings.Model
{
    public class Delta
    {
        public AddAction[] Adds { get; set; }
        public RemoveAction[] Removes { get; set; }
    }
}