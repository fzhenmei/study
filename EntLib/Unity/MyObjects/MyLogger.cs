namespace MyObjects
{
  public class MyLogger : ILogger
  {
    public string WriteMessage(string message)
    {
      // write to log file here
      return "Hi, I'm the MyLogger, and this is the output I logged: '" + message + "'.";
    }
  }
}
