using System;
using System.Text;

namespace HoNoSoFt.PushChartToConfluence.Sample.Configurations
{
  public class ConfluenceConfig
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public Uri BaseApiUri { get; set; }

    public string GetBase64Token()
    {
      return System.Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
    }
  }

}