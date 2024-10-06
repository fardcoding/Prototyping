using Hashing_and_Salting;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

Prompt();


void Prompt()
{
    Clear();
    WriteLine("[R] Register [L] Login");

    while (true)
    {
        var input = ReadLine().ToUpper()[0];
        switch (input)
        {
            case 'R': Register(); break;
            case 'L': Login(); break;
            default:
                break;
        }

    }

}


void Login()
{
    Clear();
    WriteLine("============Login========");
    Write("User Name ");
    var name = ReadLine();
    Write("Password ");
    var password = ReadLine();

    using AppDataContext context = new AppDataContext();
    var userfound = context.Users.Any(u => u.Name == name);

    if (userfound)
    {
        var loginuser = context.Users.FirstOrDefault(u => u.Name == name);


        if (HashPassword($"{password}{loginuser.Salt}") == loginuser.Password)
        {

            Clear();
            ForegroundColor = ConsoleColor.Green;
            WriteLine("Login Successful");
            ReadLine();

        }
        else
        {

            Clear();
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Login Failed");
            ReadLine();
        }
    }



}

void Register()
{
    Clear();
    WriteLine("============Register========");
    Write("User Name ");
    var name = ReadLine();
    Write("Password ");
    var password = ReadLine();

    using AppDataContext context = new AppDataContext();

    var salt = DateTime.Now.ToString();

    var HashedPW = HashPassword($"{password}{salt}");

    context.Users.Add(new User() { Name = name, Password = HashedPW, Salt = salt });
    context.SaveChanges();

    while (true)
    {
        Clear();
        WriteLine("Registration Complete");
        WriteLine("[B] Back");
        if (ReadKey().Key == ConsoleKey.B)
        {
            Prompt();
        }

    }


}


string HashPassword(string password)
{
    SHA256 hash = SHA256.Create();

    var passwordBytes = Encoding.Default.GetBytes(password);

    var hashedpassword = hash.ComputeHash(passwordBytes);

    return Convert.ToHexString(hashedpassword);

}