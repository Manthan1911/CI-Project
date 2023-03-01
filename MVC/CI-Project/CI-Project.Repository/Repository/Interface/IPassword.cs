using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IPassword
    {
        public string Encode(string password);

        public string Decode(string EncryptedPassword);
    }
}
