namespace ProductQL.Data
{
    public record AddUserInput(
        int? Id,
        string FullName,
        string Email,
        string Username,
        string Password
     );
}
