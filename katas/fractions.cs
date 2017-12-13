public interface IDoSomeShit
{
    public void DoSomeStuff();    
}

public class ShitDoer : IDoSomeShit
{
    public void DoSomeStuff()
    {
        System.Threading.Thread.Sleep(2000);
    }
}

public class LoggingShitDoer : IDoSomeShit 
{
    private IDoSomeShit _thingToLog;

    public LoggingShitDoer(IDoSomeShit thingToLog)
    {
        _thingToLog = thingToLog;
    }


    public void DoSomeStuff()
    {
        Log.WriteThings("Things to write.")
        _thingToLog.DoSomeStuff();
        Log.WriteMoreThings("Things")
    }
}


public class Program
{

    
}


public class Decorations 
{
    public PrincipalSoftwareEngineer GetNameAndTitle(int level, string name)
    {

    }

}