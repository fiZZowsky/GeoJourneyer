using System.Text.RegularExpressions;

namespace GeoJourneyer.App.Shared.Services;

public class RegisterValidator
{
    public List<string> ValidateUsername(string username)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(username))
        {
            errors.Add("Username is required");
        }
        if (username.Trim().Length < 3)
        {
            errors.Add("Username must be at least 3 characters");
        }
        return errors;
    }

    public List<string> ValidateEmail(string email)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("Email is required");
        }
        else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errors.Add("Invalid email format");
        }
        return errors;
    }

    public List<string> ValidatePassword(string password)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password is required");
        }
        if (password.Length < 6)
        {
            errors.Add("Password must be at least 6 characters");
        }
        return errors;
    }

    public List<string> ValidateConfirmPassword(string password, string confirmPassword)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(confirmPassword))
        {
            errors.Add("Confirmation is required");
        }
        if (password != confirmPassword)
        {
            errors.Add("Passwords do not match");
        }
        return errors;
    }
}