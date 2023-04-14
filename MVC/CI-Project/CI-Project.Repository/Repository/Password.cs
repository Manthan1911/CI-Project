using CI_Project.Repository.Repository.Interface;
using System.Text;
using System.Text.Unicode;

namespace CI_Project.Repository.Repository
{
    public class Password : IPassword
    {

        public string Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = System.Text.Encoding.UTF8.GetBytes(password);
                string EncryptedPassword = Convert.ToBase64String(EncDataByte);
                return EncryptedPassword;
             }
            catch(Exception ex) 
            { 
                throw new Exception("Problem in Encrypting Message... "+ ex.Message);
            }
        }
        public string Decode(string EncryptedPassword)
        {
            try
            {
                System.Text.UTF8Encoding encoder= new System.Text.UTF8Encoding();
                System.Text.Decoder decoder = encoder.GetDecoder();
                byte[] DecodecByte = Convert.FromBase64String(EncryptedPassword);
                int CharCount = decoder.GetCharCount(DecodecByte, 0, DecodecByte.Length);
                char[] DecodedChar = new char[CharCount];
                decoder.GetChars(DecodecByte, 0, DecodecByte.Length, DecodedChar, 0);
                string DecryptedPassword = new string(DecodedChar);
                return DecryptedPassword;
            }
            catch(Exception ex )
            {
                throw new Exception("Problem in Decrypting Message ...." + ex.Message);
            }
        }
    }
}
