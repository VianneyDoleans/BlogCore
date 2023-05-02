namespace BlogCoreAPI.Models.Settings;

public class EmailConfigurationSettings
{
    
    public static string Position => "EmailConfiguration";
    
    public string From { get; set; }
    
    public string NameOfFrom { get; set; }
    
    public string SmtpServer { get; set; }
    
    public int Port { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
}