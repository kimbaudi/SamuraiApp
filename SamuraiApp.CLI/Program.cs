using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
using SamuraiApp.Data;
using SamuraiApp.Domain;

#nullable disable

namespace SamuraiApp.CLI;

class Program
{
    private static readonly SamuraiContext _context = new();

    //private static async Task Main(string[] args)
    private static void Main()
    {
        #region basic example

        //// Build a config object, using env vars and JSON providers.
        //IConfiguration config = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .AddEnvironmentVariables()
        //    .Build();

        //// Get values from the config given their key and their target type.
        //Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

        //// Write the values to the console.
        //Console.WriteLine($"KeyOne = {settings.KeyOne}");
        //Console.WriteLine($"KeyTwo = {settings.KeyTwo}");
        //Console.WriteLine($"KeyThree:Message = {settings.KeyThree.Message}");

        #endregion

        #region basic example with hosting

        //using IHost host = Host.CreateDefaultBuilder(args).Build();

        //// Ask the service provider for the configuration abstraction.
        //IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        //// Get values from the config given their key and their target type.
        //int keyOneValue = config.GetValue<int>("KeyOne");
        //bool keyTwoValue = config.GetValue<bool>("KeyTwo");
        //string keyThreeNestedValue = config.GetValue<string>("KeyThree:Message");

        //// Get values from the config given their key and their target type.
        //Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

        //// Write the values to the console.
        //Console.WriteLine($"KeyOne = {keyOneValue}");
        //Console.WriteLine($"KeyTwo = {keyTwoValue}");
        //Console.WriteLine($"KeyThree:Message = {keyThreeNestedValue}");

        //// Write the values to the console.
        //Console.WriteLine($"KeyOne = {settings.KeyOne}");
        //Console.WriteLine($"KeyTwo = {settings.KeyTwo}");
        //Console.WriteLine($"KeyThree:Message = {settings.KeyThree.Message}");

        //// Application code which might rely on the config could start here.

        //await host.RunAsync();

        #endregion

        _context.Database.EnsureCreated();
        GetSamurais("Before Add:");
        AddSamurai();
        GetSamurais("After Add:");
        //Console.Write("Press any key...");
        //Console.ReadKey();
        AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
        AddSamurais([new Samurai() { Name = "a" }, new Samurai() { Name = "a" }, new Samurai() { Name = "a" }, new Samurai() { Name = "a" }, new Samurai() { Name = "a" }]);
        GetSamurais();
        AddVariousTypes();
        QueryFilters();
        QueryAggregates();
        RetrieveAndUpdateSamurai();
        RetrieveAndUpdateMultipleSamurais();
        MultipleDatabaseOperations();
        RetrieveAndDeleteASamurai();
        QueryAndUpdateBattles_Disconnected();
        InsertNewSamuraiWithAQuote();
        InsertNewSamuraiWithManyQuotes();
        AddQuoteToExistingSamuraiWhileTracked();
        AddQuoteToExistingSamuraiNotTracked(2);
        Simpler_AddQuoteToExistingSamuraiNotTracked(2);
        EagerLoadSamuraiWithQuotes();
        ProjectSomeProperties();
        ProjectSamuraisWithQuotes();
        ExplicitLoadQuotes();
        LazyLoadQuotes();
        FiteringWithRelatedData();
        ModifyingRelatedDataWhenTracked();
        ModifyingRelatedDataWhenNotTracked();
        AddingNewSamuraiToAnExistingBattle();
        ReturnBattleWithSamurais();
        ReturnAllBattlesWithSamurais();
        AddAllSamuraisToAllBattles();
        RemoveSamuraiFromABattle();
        WillNotRemoveSamuraiFromABattle();
        RemoveSamuraiFromABattleExplicit();
        AddNewSamuraiWithHorse();
        AddNewHorseToSamuraiUsingId();
        AddNewHorseToSamuraiObject();
        AddNewHorseToDisconnectedSamuraiObject();
        ReplaceAHorse();
        QuerySamuraiBattleStats();
        QueryUsingRawSql();
        QueryRelatedUsingRawSql();
        QueryUsingRawSqlWithInterpolation();
        DANGERQueryUsingRawSqlWithInterpolation();
        QueryUsingFromSqlRawStoredProc();
        QueryUsingFromSqlIntStoredProc();
        ExecuteSomeRawSql();
    }

    private static void AddSamurai()
    {
        var samurai = new Samurai { Name = "Julie" };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }

    private static void GetSamurais(string text)
    {
        var samurais = _context.Samurais.ToList();
        Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
        foreach (var samurai in samurais)
        {
            Console.WriteLine(samurai.Name);
        }
    }

    private static void AddVariousTypes()
    {
        _context.AddRange(new Samurai { Name = "Shimada" },
                          new Samurai { Name = "Okamoto" },
                          new Battle { Name = "Battle of Anegawa" },
                          new Battle { Name = "Battle of Nagashino" });
        //_context.Samurais.AddRange(
        //    new Samurai { Name = "Shimada" },
        //    new Samurai { Name = "Okamoto" });
        //_context.Battles.AddRange(
        //    new Battle { Name = "Battle of Anegawa" },
        //    new Battle { Name = "Battle of Nagashino" });
        _context.SaveChanges();
    }

    private static void AddSamuraisByName(params string[] names)
    {
        foreach (string name in names)
        {
            _context.Samurais.Add(new Samurai { Name = name });
        }
        _context.SaveChanges();
    }

    private static void AddSamurais(Samurai[] samurais)
    {
        //AddRange can take an array or an IEnumerable e.g. List<Samurai>
        _context.Samurais.AddRange(samurais);
        _context.SaveChanges();
    }

    private static void GetSamurais()
    {
        var samurais = _context.Samurais
            .TagWith("ConsoleApp.Program.GetSamurais method")
            .ToList();
        Console.WriteLine($"Samurai count is {samurais.Count}");
        foreach (var samurai in samurais)
        {
            Console.WriteLine(samurai.Name);
        }
    }

    private static void QueryFilters()
    {
        //var name = "Sampson";
        //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
        var filter = "J%";
        var samurais = _context.Samurais
            .Where(s => EF.Functions.Like(s.Name, filter)).ToList();
    }

    private static void QueryAggregates()
    {
        //var name = "Sampson";
        //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
        _ = _context.Samurais.Find(2);
    }

    private static void RetrieveAndUpdateSamurai()
    {
        var samurai = _context.Samurais.FirstOrDefault();
        samurai.Name += "San";
        _context.SaveChanges();
    }

    private static void RetrieveAndUpdateMultipleSamurais()
    {
        var samurais = _context.Samurais.Skip(1).Take(4).ToList();
        samurais.ForEach(s => s.Name += "San");
        _context.SaveChanges();
    }

    private static void MultipleDatabaseOperations()
    {
        var samurai = _context.Samurais.FirstOrDefault();
        samurai.Name += "San";
        _context.Samurais.Add(new Samurai { Name = "Shino" });
        _context.SaveChanges();
    }

    private static void RetrieveAndDeleteASamurai()
    {
        var samurai = _context.Samurais.Find(18);
        _context.Samurais.Remove(samurai);
        _context.SaveChanges();
    }

    private static void QueryAndUpdateBattles_Disconnected()
    {
        List<Battle> disconnectedBattles;
        using (var context1 = new SamuraiContext())
        {
            disconnectedBattles = [.. _context.Battles];
        } //context1 is disposed
        disconnectedBattles.ForEach(b =>
        {
            b.StartDate = new DateTime(1570, 01, 01);
            b.EndDate = new DateTime(1570, 12, 1);
        });
        using var context2 = new SamuraiContext();
        context2.UpdateRange(disconnectedBattles);
        context2.SaveChanges();
    }

    private static void InsertNewSamuraiWithAQuote()
    {
        var samurai = new Samurai { Name = "Kambei Shimada", Quotes = [new Quote { Text = "I've come to save you" }] };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }

    private static void InsertNewSamuraiWithManyQuotes()
    {
        var samurai = new Samurai
        {
            Name = "Kyūzō",
            Quotes =
            [
                new() { Text = "Watch out for my sharp sword!" },
                new() { Text = "I told you to watch out for the sharp sword! Oh well!" }
            ]
        };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }

    private static void AddQuoteToExistingSamuraiWhileTracked()
    {
        var samurai = _context.Samurais.FirstOrDefault();
        samurai.Quotes.Add(new Quote
        {
            Text = "I bet you're happy that I've saved you!"
        });
        _context.SaveChanges();
    }

    private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
    {
        var samurai = _context.Samurais.Find(samuraiId);
        samurai.Quotes.Add(new Quote { Text = "Now that I saved you, will you feed me dinner?" });
        using var newContext = new SamuraiContext();
        newContext.Samurais.Attach(samurai);
        newContext.SaveChanges();
    }

    private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
    {
        var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
        using var newContext = new SamuraiContext();
        newContext.Quotes.Add(quote);
        newContext.SaveChanges();
    }

    private static void EagerLoadSamuraiWithQuotes()
    {
        //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
        //var filteredInclude = _context.Samurais.Include(s => s.Quotes.Where(q=>q.Text.Contains("Thanks"))).ToList();
        var filterPrimaryEntityWithInclude = _context.Samurais
            .Where(s => s.Name.Contains("Sampson"))
            .Include(s => s.Quotes)
            .FirstOrDefault();
    }

    private static void ProjectSomeProperties()
    {
        var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
        var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
    }

    public struct IdAndName(int id, string name)
    {
        public int Id = id;
        public string Name = name;
    }

    private static void ProjectSamuraisWithQuotes()
    {
        // var somePropsWithQuotes = _context.Samurais
        //    .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count })
        //    .ToList();
        // var somePropsWithQuotes = _context.Samurais
        //     .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) })
        //     .ToList();
        var samuraisAndQuotes = _context.Samurais
            .Select(s => new { Samurai = s, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) })
            .ToList();
    }

    private static void ExplicitLoadQuotes()
    {
        //make sure there's a horse in the DB, then clear the context's change tracker
        _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        //-------------------
        var samurai = _context.Samurais.Find(1);
        _context.Entry(samurai).Collection(s => s.Quotes).Load();
        _context.Entry(samurai).Reference(s => s.Horse).Load();
    }

    private static void LazyLoadQuotes()
    {
        var samurai = _context.Samurais.Find(2);
        _ = samurai.Quotes.Count; //won't work without LL setup
    }

    private static void FiteringWithRelatedData()
    {
        var samurais = _context.Samurais
            .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
            .ToList();
    }

    private static void ModifyingRelatedDataWhenTracked()
    {
        var samurai = _context.Samurais
            .Include(s => s.Quotes)
            .FirstOrDefault(s => s.Id == 2);
        samurai.Quotes[0].Text = "Did you hear that?";
        _context.Quotes.Remove(samurai.Quotes[2]);
        _context.SaveChanges();
    }

    private static void ModifyingRelatedDataWhenNotTracked()
    {
        var samurai = _context.Samurais
            .Include(s => s.Quotes)
            .FirstOrDefault(s => s.Id == 2);
        var quote = samurai.Quotes[0];
        quote.Text += "Did you hear that again?";

        using var newContext = new SamuraiContext();
        //newContext.Quotes.Update(quote);
        newContext.Entry(quote).State = EntityState.Modified;
        newContext.SaveChanges();
    }

    private static void AddingNewSamuraiToAnExistingBattle()
    {
        var battle = _context.Battles.FirstOrDefault();
        battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
        _context.SaveChanges();
    }

    private static void ReturnBattleWithSamurais()
    {
        var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
    }

    private static void ReturnAllBattlesWithSamurais()
    {
        var battles = _context.Battles.Include(b => b.Samurais).ToList();
    }
    private static void AddAllSamuraisToAllBattles()
    {
        var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
        var allSamurais = _context.Samurais.ToList();
        foreach (var battle in allbattles)
        {
            battle.Samurais.AddRange(allSamurais);
        }
        _context.SaveChanges();
    }

    private static void RemoveSamuraiFromABattle()
    {
        var battleWithSamurai = _context.Battles
            .Include(b => b.Samurais.Where(s => s.Id == 12))
            .Single(s => s.BattleId == 1);
        var samurai = battleWithSamurai.Samurais[0];
        battleWithSamurai.Samurais.Remove(samurai);
        _context.SaveChanges();
    }

    private static void WillNotRemoveSamuraiFromABattle()
    {
        var battle = _context.Battles.Find(1);
        var samurai = _context.Samurais.Find(12);
        battle.Samurais.Remove(samurai);
        _context.SaveChanges(); //the relationship is not being tracked
    }

    private static void RemoveSamuraiFromABattleExplicit()
    {
        var b_s = _context.Set<BattleSamurai>()
            .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
        if (b_s != null)
        {
            _context.Remove(b_s); //_context.Set<BattleSamurai>().Remove works, too
            _context.SaveChanges();
        }
    }

    private static void AddNewSamuraiWithHorse()
    {
        var samurai = new Samurai
        {
            Name = "Jina Ujichika",
            Horse = new Horse { Name = "Silver" }
        };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }

    private static void AddNewHorseToSamuraiUsingId()
    {
        var horse = new Horse { Name = "Scout", SamuraiId = 2 };
        _context.Add(horse);
        _context.SaveChanges();
    }

    private static void AddNewHorseToSamuraiObject()
    {
        var samurai = _context.Samurais.Find(12);
        samurai.Horse = new Horse { Name = "Black Beauty" };
        _context.SaveChanges();
    }

    private static void AddNewHorseToDisconnectedSamuraiObject()
    {
        var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
        samurai.Horse = new Horse { Name = "Mr. Ed" };

        using var newContext = new SamuraiContext();
        newContext.Samurais.Attach(samurai);
        newContext.SaveChanges();
    }

    private static void ReplaceAHorse()
    {
        // var samurai = _context.Samurais.Include(s => s.Horse)
        //     .FirstOrDefault(s => s.Id == 5);
        // samurai.Horse = new Horse { Name = "Trigger" };
        var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Mr. Ed");
        horse.SamuraiId = 5; //owns Trigger! savechanges will fail
        _context.SaveChanges();
    }

    private static void QuerySamuraiBattleStats()
    {
        // var stats = _context.SamuraiBattleStats.ToList();
        //var firststat = _context.SamuraiBattleStats.FirstOrDefault();
        //var sampsonState = _context.SamuraiBattleStats
        //   .FirstOrDefault(b => b.Name == "SampsonSan");
        _ = _context.SamuraiBattleStats.Find(2);
    }

    private static void QueryUsingRawSql()
    {
        _ = _context.Samurais.FromSqlRaw("Select * from samurais").ToList();
        //var samurais = _context.Samurais.FromSqlRaw(
        //    "Select Id, Name, Quotes, Battles, Horse from Samurais").ToList();
    }

    private static void QueryRelatedUsingRawSql()
    {
        var samurais = _context.Samurais.FromSqlRaw(
            "Select Id, Name from Samurais").Include(s => s.Quotes).ToList();
    }

    private static void QueryUsingRawSqlWithInterpolation()
    {
        string name = "Kikuchyo";
        _ = _context.Samurais
            .FromSqlInterpolated($"Select * from Samurais Where Name= {name}")
            .ToList();
    }

    private static void DANGERQueryUsingRawSqlWithInterpolation()
    {
        string name = "Kikuchyo";
        _ = _context.Samurais
            .FromSql($"Select * from Samurais Where Name= '{name}'")
            .ToList();
    }

    private static void QueryUsingFromSqlRawStoredProc()
    {
        var text = "Happy";
        _ = _context.Samurais.FromSqlRaw(
         "EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();
    }

    private static void QueryUsingFromSqlIntStoredProc()
    {
        var text = "Happy";
        _ = _context.Samurais.FromSqlInterpolated(
         $"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
    }

    private static void ExecuteSomeRawSql()
    {
        //var samuraiId = 2;
        //var affected= _context.Database
        //    .ExecuteSqlRaw("EXEC DeleteQuotesForSamurai {0}", samuraiId) ;
        var samuraiId = 2;
        _ = _context.Database
            .ExecuteSqlInterpolated($"EXEC DeleteQuotesForSamurai {samuraiId}");
    }
}
