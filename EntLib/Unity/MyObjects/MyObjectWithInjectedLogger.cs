namespace MyObjects
{
  public class MyObjectWithInjectedLogger : IMyInterface
  {
    private ILogger theLogger;

    public MyObjectWithInjectedLogger(ILogger log)
    {
      theLogger = log;
    }

    public string GetObjectStringResult()
    {
      string output = theLogger.WriteMessage("SOME REALLY IMPORTANT MESSAGE");
      return "Hi, I'm the 'MyObjectWithInjectedLogger' object, and I've got a reference to some type of Logger!<p />" + output;
    }

  }
}
