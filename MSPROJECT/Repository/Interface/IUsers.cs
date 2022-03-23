using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPROJECT.Repository.Interface;
using MSPROJECT.Utils.Enums;
using MSPROJECT.ViewModel;

namespace MSPROJECT.Repository.Interface
{
    public  interface IUsers
	{

        SignInEnum SignIn(SignInModel model);
        SignUpEnum SignUp(SignUpModel model);
        bool VerifyAccounts(string otp);
    }

}
