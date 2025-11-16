using System.Data;

namespace EvenApp.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

