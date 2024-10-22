namespace _4.Domain.Entities.Token
{
   public class CaptchaVerificationModel
    {
        public bool Success { get; set; }
        public string Challenge_ts { get; set; }
        public string Hostname { get; set; }
        public List<string> ErrorCodes { get; set; }
    }
}
