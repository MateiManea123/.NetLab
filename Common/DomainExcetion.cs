namespace BookManagement.Common;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}