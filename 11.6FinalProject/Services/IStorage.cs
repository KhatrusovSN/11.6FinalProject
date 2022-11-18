using _11._6FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11._6FinalProject.Services
{
    internal interface IStorage
    {
        Session GetSession(long chatId);
    }
}
