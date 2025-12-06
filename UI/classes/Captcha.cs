

using System;

namespace UI.classes
{
    internal class Captcha
    {
        private string _captchaString;
        private string _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIKLMOPQRSTUVWXYZ0123456789";

        public string Generate(int length)
        {
            _captchaString = string.Empty;
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int letterInd = rnd.Next(_alphabet.Length);
                _captchaString += _alphabet[letterInd];
            }

            return _captchaString;
        }

        public bool CheckCaptcha(string input)
            => input == _captchaString ? true : false;
    }
}
