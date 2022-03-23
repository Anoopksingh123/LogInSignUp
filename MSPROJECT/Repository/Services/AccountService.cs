using MSPROJECT.Models;
using MSPROJECT.Repository.Interface;
using MSPROJECT.Utils.Enums;
using MSPROJECT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;


namespace MSPROJECT.Repository.Services
{
    public class AccountService : IUsers

    {
        private EMSDbContext dbContext;
        public AccountService()
        {
            dbContext = new EMSDbContext();

        }
        public SignInEnum SignIn(SignInModel model)
        {

            var user = dbContext.Tbl_Users.SingleOrDefault
                (e => e.Email == model.Email && e.Password == model.Password);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    if (user.Isactive)
                    {
                        return SignInEnum.success;
                    }
                    else
                    {
                        return SignInEnum.InActive;
                    }
                }

                else
                {
                    return SignInEnum.NotVerified;
                }
            }
            else
            {
                return SignInEnum.WrongCredentials;
            }
        }

        //public SignUpEnum SignUp(SignUpModel model)
        //{
        //    throw new NotImplementedException();
        //}

        public SignUpEnum SignUp(SignUpModel model)
        {
            if (dbContext.Tbl_Users.Any(e => e.Email == model.Email))
            {
                return SignUpEnum.Emailexist;
            }
            else
            {
                var user = new Tbl_Users()
                {
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Email = model.Email,
                    Password = model.CoinfirmPassword,
                    Gender = model.Gender
                };
                dbContext.Tbl_Users.Add(user);
                string Otp = GenerateOtp();
                SendMail(model.Email, Otp);
                var VAccount = new VerifyAccount()
                {
                    Otp = Otp,
                    Userid = model.Email,
                    send_Time = DateTime.Now
                };
                dbContext.VerifyAccounts.Add(VAccount);
                dbContext.SaveChanges();
                return SignUpEnum.success;
            }
        }
        private void SendMail(string to, string Otp)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("dotnetasp791@gmail.com");
            mail.Subject = "Verify Your Account";
            string Body = $"Your Otp is <b>{Otp}</b> <br/>thanks for choosing us.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("dotnetasp791@gmail.com", "dotnetasp791@123"); //enter senders users name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        private string GenerateOtp()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, 6).Select(x => chars[random.Next(chars.Length)]);
            var r = string.Join("", list);
            return r;
        }
        public bool VerifyAccounts(string Otp)
        {
            if (dbContext.VerifyAccounts.Any(e => e.Otp == Otp))
            {
                var Acc = dbContext.VerifyAccounts.SingleOrDefault(e => e.Otp == Otp);
                var User = dbContext.Tbl_Users.SingleOrDefault(e => e.Email == Acc.Userid);
                User.IsVerified = true;
                User.Isactive = true;
                dbContext.VerifyAccounts.Remove(Acc);
                dbContext.Tbl_Users.Update(User);
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

     
    }
}
