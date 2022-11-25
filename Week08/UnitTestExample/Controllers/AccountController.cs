using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.Controllers
{
    public class AccountController
    {        
        public IAccountManager AccountManager { get; set; }

        public AccountController()
        {
            AccountManager = new AccountManager();
        }

        public Account Register(string email, string password)
        {
            if(!ValidateEmail(email))
                throw new ValidationException(
                    "A megadott e-mail cím nem megfelelő!");
            if(!ValidateEmail(email))
                throw new ValidationException(
                    "A megadottt jelszó nem megfelelő!\n" +
                    "A jelszó legalább 8 karakter hosszú kell legyen, csak az angol ABC betűiből és számokból állhat, és tartalmaznia kell legalább egy kisbetűt, egy nagybetűt és egy számot.");

            var account = new Account()
            {
                Email = email,
                Password = password,
                ID = Guid.NewGuid()
                
            };

            var newAccount = AccountManager.CreateAccount(account);

            return newAccount;
        }

        public bool ValidateEmail(string email)
        {            
            return Regex.IsMatch(
                email, 
                @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        }

        public bool ValidatePassword(string password)
        {
            Regex length = new Regex("[0 - 9a - zA - Z]{ 8,}");

            if (password.Any(char.IsUpper))
            {
                if (password.Any(char.IsLower))
                {
                    if (password.Any(char.IsDigit))
                    {
                        return Regex.IsMatch(password, @"[0-9a-zA-Z]{8,}$");
                    }
                    else
                    {
                        //MessageBox.Show("A jelszó csak az angol ABC karaktereiből állhat és minimum 8 karakter hosszú kell legyen");
                        return false;
                    }
                }
                else
                {
                    //MessageBox.Show("A jelszónak tartalmaznia kell kisbetűt");
                    return false;
                    
                }
            }
            else
            {
                //MessageBox.Show("A jelszónak tartalmaznia kell nagybetűt");
                return false;
                
            }
            
        }
    }
}
