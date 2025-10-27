using System;

namespace SAGE.Domain.ChangePlans.Exceptions
{
  public class NotFoundException : Exception
  {
    public NotFoundException() { }
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception inner) : base(message, inner) { }
  }
}