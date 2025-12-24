using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace AstralAutoPatch
{
  internal static class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      // .NET 6+ WinForms 초기화
      ApplicationConfiguration.Initialize();

      var form = new Form1();

      // 실행 인자 처리
      if (args.Length > 0)
      {
        // astral:// 프로토콜을 통해 실행된 경우, 첫 번째 인자로 전체 URI가 전달
        string uriString = args[0];
        if (uriString.StartsWith("astral://", StringComparison.OrdinalIgnoreCase))
        {
          form.IsProtocolLaunch = true;
          try
          {
            // URI 파싱을 통해 폴더명을 추출
            var uri = new Uri(uriString);
            if (!string.IsNullOrEmpty(uri.Host))
            {
              form.TargetGameDataFolder = uri.Host;
            }
          }
          catch
          {
            // URI 형식이 올바르지 않은 경우 기본값 사용
          }
        }
      }

      Application.Run(form);
    }

    public static bool IsAdministrator()
    {
      using (var identity = WindowsIdentity.GetCurrent())
      {
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
      }
    }
  }
}
