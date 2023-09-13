using AmeisenBotX.Common;
using AmeisenBotX.Common.Math;
using AmeisenBotX.Common.Utils;
using AmeisenBotX.Core.Engines.Movement.Enums;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Core.Engines.Quest.Objects.Quests;
using AmeisenBotX.Core.Managers.Character.Inventory;
using AmeisenBotX.Core.Managers.Character.Inventory.Objects;
using AmeisenBotX.Plugins.Questing.Database.Models;
using AmeisenBotX.Plugins.Questing.Database.Repository;
using AmeisenBotX.Wow.Objects;
using AmeisenBotX.Wow.Objects.Enums;
using AmeisenBotX.Wow.Objects.Raw.SubStructs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AmeisenBotX.Plugins.Questing.Database
{
    public class QuestingDatabaseEngine : IQuestEngine
    {
        public QuestingDatabaseEngine(AmeisenBotInterfaces bot)
        {
            Bot = bot;

            Services = CreateServiceProvider(bot);

            Profiles.Clear();
            IEnumerable<IQuestProfile> profiles = Services.GetServices<IQuestProfile>();
            foreach (IQuestProfile profile in profiles)
            {
                Profiles.Add(profile);
            }

            SelectedProfile = Profiles.FirstOrDefault();

            QueryEvent = new TimegatedEvent(TimeSpan.FromSeconds(30), PopulateProfile);
            UpdateDatabase = new TimegatedEvent(TimeSpan.FromMinutes(2), PopulateDatabase);
            RightClickEvent = new TimegatedEvent(TimeSpan.FromMilliseconds(2500));

            RegisterEvent("QUEST_PROGRESS", OnQuestProgress);
        }


        public void Dispose()
        {
            UnregisterEvent("QUEST_PROGRESS", OnQuestProgress);
        }

        ~QuestingDatabaseEngine()
        {
            Dispose();
        }

        private void OnQuestProgress(long arg1, List<string> list)
        {

        }

        private void CombatLog_OnPartyKill(ulong sourceGuid, ulong npcGuid)
        {
            IWowUnit wowUnit = Bot.GetWowObjectByGuid<IWowUnit>(npcGuid);

            if (wowUnit != null && (Bot.Player.Guid == sourceGuid || Bot.Objects.PartymemberGuids.Contains(sourceGuid)))
            {
                IEnumerable<BotQuestObjective> killBasedObjectives = SelectedProfile.Quests
                    .SelectMany(t => t.SelectMany(x => x.Objectives.OfType<BotQuestObjective>().Where(r => r.DbQuestObjective?.Type == 1)))
                    .Where(t => t.DbQuestObjective?.ObjectId == wowUnit.EntryId);
                foreach (BotQuestObjective? objective in killBasedObjectives)
                {
                    double tick = 100.0 / Math.Max(objective.DbQuestObjective?.Amount ?? 1, 1);
                    objective.Progress += tick;
                }
            }
        }

        internal IServiceProvider Services { get; init; }

        private WorldContext World => Services.GetService<WorldContext>();

        private IServiceProvider CreateServiceProvider(AmeisenBotInterfaces bot)
        {
            IServiceCollection services = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(typeof(QuestingDatabaseEngine).Assembly.Location).FullName)
            .AddJsonFile("local.settings.json", true)
            .Build();

            services.AddSingleton<IConfigurationRoot>(configuration);
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton(bot);

            services.AddSingleton<IQuestProfile, DBProfile>();
            services.AddSingleton<IQuestEngine>(this);

            AddDbContext(services, configuration);

            return services.BuildServiceProvider();
        }

        private void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection mariaConfig = configuration.GetSection("MariaDb");
            string? host = mariaConfig.GetValue<string>("Host", "localhost");
            int port = mariaConfig.GetValue<int>("Port", 3363);
            string? username = mariaConfig.GetValue<string>("Username", "root");
            string? password = mariaConfig.GetValue<string>("Password", "toor");
            string? database = mariaConfig.GetValue<string>("Database", "world");
            // Replace with your connection string.
            string connectionString = $"server={host};port={port};user={username};password={password};database={database}";

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            MariaDbServerVersion serverVersion = new("10.11.4");

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<WorldContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion, t =>
                    {
                        t.EnableRetryOnFailure();
                    })
                    .EnableServiceProviderCaching()
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            , ServiceLifetime.Transient);
        }

        public bool ActionToggle { get; set; }

        public IConfiguration Configuration { get; set; }

        public AmeisenBotInterfaces Bot { get; set; }

        public List<int> CompletedQuests { get; } = new List<int>();

        public IQuestProfile SelectedProfile { get; set; }
        public bool UpdatedCompletedQuests { get; set; }

        public ICollection<IQuestProfile> Profiles { get; init; } = new List<IQuestProfile>();
        public DateTime LastAbandonQuestTime { get; private set; }

        private TimegatedEvent QueryEvent { get; init; }
        public TimegatedEvent UpdateDatabase { get; }
        public TimegatedEvent RightClickEvent { get; }

        private readonly MemoryCache cache = new(new MemoryCacheOptions());

        public void Enter()
        {
            IEnumerable<QuestlogEntry>? incompleteQuests = Bot.Player?.QuestlogEntries?.Where(t => t.Finished != 1);

            SelectedProfile = Services.GetService<IQuestProfile>();

            World.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Execute()
        {
            if (SelectedProfile == null)
            {
                return;
            }

            QueryEvent.Run();
            UpdateDatabase.Run();

            if (SelectedProfile.Quests.Count > 0)
            {
                IEnumerable<IBotQuest> selectedQuests = SelectedProfile.Quests.Peek().Where(e => !e.Returned);

                // drop all quest that are not selected
                //if (Bot.Player.QuestlogEntries?.Count() == 25 && DateTime.UtcNow.Subtract(LastAbandonQuestTime).TotalSeconds > 30)
                //{
                //    Bot.Wow.AbandonQuestsNotIn(selectedQuests.Select(q => q.Name));
                //    LastAbandonQuestTime = DateTime.UtcNow;
                //}

                if (selectedQuests.Any())
                {
                    IBotQuest notAcceptedQuest = selectedQuests.FirstOrDefault(e => !e.Accepted);

                    // make sure we got all quests
                    if (notAcceptedQuest != null)
                    {
                        if (!notAcceptedQuest.Accepted)
                        {
                            notAcceptedQuest.AcceptQuest();
                            return;
                        }
                    }
                    else
                    {
                        // do the quests if not all of them are finished
                        if (selectedQuests.Any(e => !e.Finished))
                        {
                            IBotQuest activeQuest = selectedQuests.FirstOrDefault(e => !e.Finished);
                            activeQuest?.Execute();
                        }
                        else
                        {
                            // make sure we return all quests
                            IBotQuest notReturnedQuest = selectedQuests.FirstOrDefault(e => !e.Returned);

                            if (notReturnedQuest != null)
                            {
                                if (CompleteQuest(notReturnedQuest))
                                {
                                    CompletedQuests.Add(notReturnedQuest.Id);
                                }
                                return;
                            }
                        }
                    }
                }
                else
                {
                    //CompletedQuests.AddRange(SelectedProfile.Quests.Dequeue().Select(e => e.Id));
                    return;
                }
            }
        }

        private async void PopulateDatabase()
        {
            if (Bot.Objects.IsWorldLoaded)
            {
                var seenNpcs = Bot.Objects.All.OfType<IWowUnit>()
                    .Where(t => !t.IsDead && (t.IsQuestgiver || t.IsRepairer || t.IsVendor || t.IsFlightMaster))
                    .ToList();

                var creatureIdsWithKnownSpawnLocations = await World.Creatures
                    .Where(t => t.map == (int)Bot.Objects.MapId)
                    .Select(t => t.id)
                    .ToListAsync();

                seenNpcs.RemoveAll(t => creatureIdsWithKnownSpawnLocations.Contains(t.EntryId));

                foreach(var unknownUnit in seenNpcs)
                {
                    var creature = new DbCreature()
                    {
                        id = unknownUnit.EntryId,
                        guid = unknownUnit.Guid,
                        map = (int)Bot.Objects.MapId,
                        position_x = unknownUnit.Position.X,
                        position_y = unknownUnit.Position.Y,
                        position_z = unknownUnit.Position.Z,
                        spawnMask = 1
                    };

                    await World.Creatures.AddAsync(creature);
                }
                await World.SaveChangesAsync();
            }
        }

        private async void PopulateProfile()
        {
            if (Bot.Player.QuestlogEntries != null)
            {
                SelectedProfile.Quests.Clear();
                foreach (QuestlogEntry entry in Bot.Player.QuestlogEntries)
                {
                    try
                    {
                        IBotQuest? quest = await cache.GetOrCreateAsync($"quest-{entry.Id}", async t => await GetDbQuest(entry.Id));

                        if (SelectedProfile.Quests.Any(q => q.Any(t => t.Id == quest?.Id)) == false)
                        {
                            SelectedProfile.Quests.Enqueue(new[] { quest });
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private bool CompleteQuest(IBotQuest quest)
        {
            if (quest.Returned)
            {
                return true;
            }

            (IWowObject, Vector3) objectPositionCombo = quest.GetEndObject();

            if (objectPositionCombo.Item1 != null)
            {
                // move to unit / object
                if (Bot.Player.Position.GetDistance(objectPositionCombo.Item1.Position) > 5.0)
                {
                    Bot.Movement.SetMovementAction(MovementAction.Move, objectPositionCombo.Item1.Position);
                    ActionToggle = true;
                }
                else
                {
                    ActionToggle = !ActionToggle;

                    // interact with it
                    if (!ActionToggle)
                    {
                        RightClickQuestgiver(objectPositionCombo.Item1);
                    }
                    else if (quest.ActionEvent.Run())
                    {
                        Bot.Wow.SelectQuestByNameOrGossipId(quest.Name, 0, false);
                        Thread.Sleep(1000);
                        Bot.Wow.CompleteQuest();
                        Thread.Sleep(1000);

                        bool selectedReward = false;
                        // TODO: This only works for the english locale!
                        if (Bot.Wow.GetQuestLogIdByTitle(quest.Name, out int questLogId))
                        {
                            Bot.Wow.SelectQuestLogEntry(questLogId);

                            if (Bot.Wow.GetNumQuestLogChoices(out int numChoices))
                            {
                                for (int i = 1; i <= numChoices; ++i)
                                {
                                    if (Bot.Wow.GetQuestLogChoiceItemLink(i, out string itemLink))
                                    {
                                        string itemJson = Bot.Wow.GetItemByNameOrLink(itemLink);
                                        IWowInventoryItem item = ItemFactory.BuildSpecificItem(ItemFactory.ParseItem(itemJson));

                                        if (item == null)
                                        {
                                            break;
                                        }

                                        if (item.Name == "0" || item.ItemLink == "0")
                                        {
                                            // get the item id and try again
                                            itemJson = Bot.Wow.GetItemByNameOrLink
                                            (
                                                itemLink.Split(new string[] { "Hitem:" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                        .Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                            );

                                            item = ItemFactory.BuildSpecificItem(ItemFactory.ParseItem(itemJson));
                                        }

                                        if (Bot.Character.IsItemAnImprovement(item, out _))
                                        {
                                            Bot.Wow.SelectQuestReward(i);
                                            Bot.Wow.SelectQuestReward(i);
                                            Bot.Wow.SelectQuestReward(i);
                                            selectedReward = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (!selectedReward)
                        {
                            Bot.Wow.SelectQuestReward(1);
                        }

                        Thread.Sleep(250);
                        quest.Returned = Bot.Player.QuestlogEntries.All(t => t.Id != quest.Id);
                        return quest.Returned;
                    }
                }
            }
            else if (objectPositionCombo.Item2 != default)
            {
                // move to position
                if (Bot.Player.Position.GetDistance(objectPositionCombo.Item2) > 5.0)
                {
                    Bot.Movement.SetMovementAction(MovementAction.Move, objectPositionCombo.Item2);
                }
            }

            return false;
        }

        private void RightClickQuestgiver(IWowObject obj)
        {
            if (obj.GetType().IsAssignableTo(typeof(IWowGameobject)))
            {
                Bot.Wow.InteractWithObject(obj);
            }
            else if (obj.GetType().IsAssignableTo(typeof(IWowUnit)))
            {
                Bot.Wow.InteractWithUnit((IWowUnit)obj);
            }
        }

        public async Task<IBotQuest> GetDbQuest(int questId)
        {
            Models.DbQuest? dbQuest = World.Quests.SingleOrDefault(t => t.Id == questId);
            // method 2 kill/loot
            Models.DbQuestObjective[] objectives = World.QuestObjectives.Where(t => t.QuestId == questId).ToArray();
            List<IQuestObjective> questObjectives = new();
            foreach (Models.DbQuestObjective? objective in objectives)
            {
                BotQuestObjective qo = new()
                {
                    DbQuestObjective = objective
                };

                qo.Action = objective.Type switch
                {
                    0 => () => HuntTargets(qo, new[] { objective.ObjectId }),
                    1 => () => HuntItems(qo),
                    _ => () => DoNothing(qo),
                };

                questObjectives.Add(qo);
            }

            int[] questGiverIds = World.CreatureQuestStarters.Where(t => t.QuestId == questId).Select(t => t.Entry).ToArray();
            BotQuestGetPosition start = () => (default, default);
            if (questGiverIds.Any())
            {
                start = () => GetQuestUnitLocation(dbQuest.Id, questGiverIds).Result;
            }

            int[] questEnderIds = World.CreatureQuestEnders.Where(t => t.QuestId == questId).Select(t => t.Entry).ToArray();
            BotQuestGetPosition end = () => (default, default);
            if (questEnderIds.Any())
            {
                end = () => GetQuestUnitLocation(dbQuest.Id, questEnderIds).Result;
            }

            QuestlogEntry playerQuest = Bot.Player.QuestlogEntries.DefaultIfEmpty(new QuestlogEntry { Id = -1 }).SingleOrDefault(t => t.Id == questId);

            BotQuest botQuest = new()
            {
                Id = dbQuest.Id,
                Name = dbQuest.Title,
                Objectives = questObjectives,
                GetStartObject = start,
                GetEndObject = end,
                Accepted = playerQuest.Id == dbQuest.Id
            };

            if (playerQuest.Finished == 1)
            {
                botQuest.Finished = true;
            }

            return botQuest;
        }

        private void DoNothing(BotQuestObjective objective)
        {

        }

        private void HuntItems(BotQuestObjective objective)
        {
            UpdateQuantityProgress(objective);
            if (objective.Finished || Bot.Player.IsCasting)
            {
                return;
            }

            int itemId = objective.DbQuestObjective.ObjectId;

            int have = Bot.Character.Inventory.Items.Where(t => t.Id == itemId).Sum(t => t.Count);

            objective.Progress = Math.Round(have > 0 ? (have / objective.DbQuestObjective.Amount) : 0.0, 2) * 100;
            if (objective.Finished || Bot.Player.IsCasting)
            {
                return;
            }

            string? lootType = cache.Get<string>($"loottype-{itemId}");

            if (lootType is null or "creatureloot")
            {
                IQueryable<int>? creatureLoot = cache.GetOrCreate($"creatureloot-{itemId}", t => World.CreatureLootTemplates.Where(t => t.item == itemId).Select(t => t.entry));
                if (creatureLoot?.Any() == true)
                {
                    lootType = cache.Set($"loottype-{itemId}", "creatureloot");
                    HuntTargets(objective, creatureLoot);
                }
            }

            if (lootType is null or "worldloot")
            {
                int[] worldLoot = cache.GetOrCreate($"worldloot-{itemId}", t => World.GameObjectTemplates.Where(t =>
                    t.questItem1 == itemId || t.questItem2 == itemId || t.questItem3 == itemId || t.questItem4 == itemId || t.questItem5 == itemId || t.questItem6 == itemId))
                    .Select(t => t.entry)
                    .ToArray();
                if (worldLoot?.Any() == true)
                {
                    lootType = cache.Set($"loottype-{itemId}", "worldloot");
                    InteractWithWorldObject(objective, worldLoot);
                }
            }
        }

        private void InteractWithWorldObject(BotQuestObjective objective, IEnumerable<int> worldLoot)
        {
            string cacheKey = $"quest-worldloot-{string.Join(',', worldLoot)}";

            if (cache.TryGetValue(cacheKey, out ulong guid) && Bot.Objects.All.OfType<IWowGameobject>().SingleOrDefault(t => t.Guid == guid) == null)
            {
                cache.Remove(cacheKey);
            }

            ulong? lootGuid = cache.GetOrCreate($"quest-worldloot-{string.Join(',', worldLoot)}", t =>
            {
                return Bot.Objects.All.OfType<IWowGameobject>()
                .Where(e => worldLoot.Contains(e.EntryId))
                .OrderBy(e => e.Position.GetDistance(Bot.Player.Position))
                .FirstOrDefault()?
                .Guid;
            });

            IWowGameobject? lootableObject = Bot.Objects.All.OfType<IWowGameobject>().SingleOrDefault(t => t.Guid == lootGuid);

            if (lootableObject != null)
            {
                if (lootableObject.Position.GetDistance(Bot.Player.Position) > 4.5)
                {
                    Bot.Movement.SetMovementAction(MovementAction.Move, lootableObject.Position);
                }
                else
                {
                    if (RightClickEvent.Run())
                    {
                        Bot.Movement.Reset();
                        Bot.Wow.StopClickToMove();
                        Bot.Wow.InteractWithObject(lootableObject);
                    }
                }
            }
        }

        private async Task<(IWowUnit Unit, Vector3 Location)> GetQuestUnitLocation(int questId, IEnumerable<int> entryIds)
        {
            Vector3 backup = default;
            // try to find the target by loaded entities
            IWowUnit? target = Bot.GetClosestQuestGiverByEntityId(Bot.Player.Position, entryIds);
            if (target == null)
            { // we're not close enough, query the DB
                backup = await cache.GetOrCreateAsync($"vector3-{string.Join(',', entryIds)}", async t =>
                {
                    t.SetSlidingExpiration(TimeSpan.FromSeconds(10));

                    var npc = await World.Creatures
                        .Where(t => entryIds.Contains(t.id))
                        .FirstOrDefaultAsync();

                    if (npc != null)
                    { // found an NPC in the DB
                        return new Vector3(npc.position_x, npc.position_y, npc.position_z);
                    }
                    
                    var quest = await World.Quests.SingleOrDefaultAsync(t => t.Id == questId);
                    if(quest != null && quest.PointX != 0 && quest.PointY != 0)
                    { // Maybe the quest location can help?
                        return new Vector3(quest.PointX, quest.PointY, 255);
                    }
                    
                    // welp, that's not good
                    return default;
                });

            }
            return (target, backup);
        }

        private void HuntTargets(BotQuestObjective objective, IEnumerable<int> targetEntryIds)
        {
            Models.DbQuestObjective? obj = objective.DbQuestObjective;
            if (obj?.Type == 0)
            { // objective is to kill
                UpdateQuantityProgress(objective);
            }

            if (objective.Finished || Bot.Player.IsCasting) { return; }

            IWowUnit? Unit = null;
            Vector3 backupLocation = default;

            if (Bot.Target != null
                && !Bot.Target.IsDead
                && !Bot.Target.IsNotAttackable
                && Bot.Db.GetReaction(Bot.Player, Bot.Target) != WowUnitReaction.Friendly)
            {
                Unit = Bot.Target;
            }
            else
            {
                string cacheKey = $"closest-{string.Join('-', targetEntryIds)}";
                if (cache.TryGetValue(cacheKey, out ulong guid))
                {
                    Unit = Bot.Objects.All
                        .OfType<IWowUnit>()
                        .SingleOrDefault(t => t.Guid == guid && !t.IsDead);
                }

                if (Unit == null)
                {
                    IEnumerable<IWowUnit> potentialTargets = Bot.Objects.All
                        .OfType<IWowUnit>()
                        .Where(e => !e.IsDead && targetEntryIds.Contains(e.EntryId));

                    Unit = potentialTargets
                        .OrderBy(e => e.Position.GetDistance(Bot.Player.Position))
                        .FirstOrDefault();

                    if (Unit == null)
                    { // check the DB
                        backupLocation = cache.GetOrCreate("", c => World.Creatures
                            .Where(t => targetEntryIds.Contains(t.id))
                            .ToArray()
                            .Select(t => new Vector3(t.position_x, t.position_y, t.position_z))
                            .OrderBy(t => t.GetDistance(Bot.Player.Position))
                            .FirstOrDefault());
                    }

                    if (Unit != null)
                    {
                        cache.Set(cacheKey, Unit.Guid, DateTimeOffset.Now.AddSeconds(10));
                    }
                }
            }

            if (Unit != null)
            {
                if (Bot.Target?.Guid != Unit.Guid)
                {
                    Bot.Wow.ChangeTarget(Unit.Guid);
                }

                Bot.CombatClass.AttackTarget();
            }
            else if (backupLocation != default)
            {
                Bot.Movement.SetMovementAction(MovementAction.Move, backupLocation);
            }
        }

        private void UpdateQuantityProgress(BotQuestObjective objective)
        {
            QuestlogEntry gameQuest = Bot.Player.QuestlogEntries.Single(t => t.Id == objective.DbQuestObjective.QuestId);
            int progress = 0;
            switch (objective.DbQuestObjective.Index)
            {
                case 0:
                    progress = gameQuest.ProgressPartymember1;
                    break;
                case 1:
                    progress = gameQuest.ProgressPartymember2;
                    break;
                case 2:
                    progress = gameQuest.ProgressPartymember3;
                    break;
                case 3:
                    progress = gameQuest.ProgressPartymember4;
                    break;
            }

            objective.Progress = Math.Round(progress / (double)objective.DbQuestObjective.Amount, 2) * 100;
        }

        #region
        private void RegisterEvent(string eventName, Action<long, List<string>> action)
        {
            if (!Bot.Wow.Events.Events.Any(t => t.Key == eventName))
            {
                Bot.Wow.Events.Subscribe(eventName, action);
            }
        }

        private void UnregisterEvent(string eventName, Action<long, List<string>> action)
        {
            if (!Bot.Wow.Events.Events.Any(t => t.Key == eventName))
            {
                Bot.Wow.Events.Unsubscribe(eventName, action);
            }
        }
        #endregion
    }
}