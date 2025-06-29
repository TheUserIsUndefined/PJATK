﻿using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class LoginUserRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}