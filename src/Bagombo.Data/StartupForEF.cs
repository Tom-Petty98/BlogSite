﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Bagombo;
using Microsoft.EntityFrameworkCore.Design;

namespace Bagombo.Data
{
  /// <summary>
  /// The whole point of this class is to allow for EF Core code to exist in a seperate project from the web application
  /// and be able to do migrations and so forth
  /// dotnet ef migrations add test1 --startup-project ..\blog
  /// or
  /// https://github.com/aspnet/EntityFramework/issues/7889   -- using this fix
  /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
  /// </summary>
  /// 
  class StartupForEF : IDesignTimeDbContextFactory<BlogDbContext>
  {
    public BlogDbContext CreateDbContext(string[] args)
    {
      var confBuilder = new ConfigurationBuilder()
       .AddEnvironmentVariables()
       .AddUserSecrets<StartupForEF>();

      IConfiguration Configuration = confBuilder.Build();

      var ConnectionStringConfigName = "ConnectionString";
      var ConnectionString = Configuration[$"{ConnectionStringConfigName}"];

      var builder = new DbContextOptionsBuilder<BlogDbContext>();

      builder.UseSqlServer(ConnectionString);

      return new BlogDbContext(builder.Options);
    }
  }
}
