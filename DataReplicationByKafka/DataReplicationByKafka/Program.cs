using Confluent.Kafka;
using DAL;
using DataReplicationByKafka.Extensions;
using DataReplicationByKafka.HostedService;
using Microsoft.EntityFrameworkCore;
using Service.Implementation;
using Service.Interface;
using System.Security.AccessControl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IPersonDataService, PersonDataService>();
builder.Services.AddTransient<IProjectTaskService, ProjectTaskService>();

builder.Services.AddKafkaTools(builder.Configuration);
builder.CreateTopics();

var app = builder.Build();

app.MigrateDatabase();

app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
