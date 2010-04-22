namespace MyObjects
{
  public class MyFastLogger :ILogger
  {
    public string WriteMessage(string message)
    {
      // write to log file here
      return "Hi, I'm the MyFastLogger, and this is the output I logged: '" + message + "', and I did it really quickly!";
    }
  }
}
