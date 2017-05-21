using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoginErrors
{
    ServerFull = 1,
    GameAlreadyStarted = 2,
    LoginAlreadyUsed = 3,
    InvalidKey = 4,
}

public class LoginErrorMessage : Message {

    public LoginErrors ErrorCode { get; set; }

    public LoginErrorMessage()
    {
        Id = 201;
    }

    public LoginErrorMessage(LoginErrors error)
        :this()
    {
        ErrorCode = error;
    }

    public override string Serialize()
    {
        return ((int)ErrorCode).ToString();
    }
}
