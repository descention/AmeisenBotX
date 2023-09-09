using AmeisenBotX.Common;
using AmeisenBotX.Common.Math;
using AmeisenBotX.Core.Engines.Movement.Enums;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Core.Engines.Quest.Objects.Quests;
using AmeisenBotX.Plugins.Questing.Database.Repository;
using AmeisenBotX.Wow.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace AmeisenBotX.Plugins.Questing.Database
{
    public class QuestingDatabaseEngine : IQuestEngine
    {
        public QuestingDatabaseEngine(AmeisenBotInterfaces bot)
        {
            Bot = bot;

            Services = CreateServiceProvider(bot);

            Profiles.Clear();
            var profiles = this.Services.GetServices<IQuestProfile>();
            foreach(var profile in profiles)
                Profiles.Add(profile);
            
            SelectedProfile = Profiles.FirstOrDefault();
        }
        internal IServiceProvider Services { get; init; }

        private WorldContext World => Services.GetService<WorldContext>();

        private IServiceProvider CreateServiceProvider(AmeisenBotInterfaces bot)
        {
            IServiceCollection services = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("local.settings.json", true)
            .Build();

            services.AddSingleton<IConfigurationRoot>(configuration);
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton(bot);

            services.AddSingleton<IQuestProfile, DBProfile>();
            services.AddSingleton<IQuestEngine>(this);

            AddDbContext(services);

            return services.BuildServiceProvider();
        }

        private void AddDbContext(IServiceCollection services)
        {
            // Replace with your connection string.
            var connectionString = "server=host;port=3343;user=user;password=pass;database=world";

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            var serverVersion = new MariaDbServerVersion("10.11.4");

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<WorldContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
        }

        public bool ActionToggle { get; set; }

        public IConfiguration Configuration { get; set; }

        public AmeisenBotInterfaces Bot { get; set; }

        public List<int> CompletedQuests { get; } = new List<int>();

        public IQuestProfile SelectedProfile { get; set; }
        public bool UpdatedCompletedQuests { get; set; }

        public ICollection<IQuestProfile> Profiles { get; init; } = new List<IQuestProfile>();
        public DateTime LastAbandonQuestTime { get; private set; }

        public new void Enter()
        {
            var incompleteQuests = Bot.Player?.QuestlogEntries?.Where(t => t.Finished != 1);

            SelectedProfile = Services.GetService<IQuestProfile>();
        }

        public new void Execute()
        {
            if (SelectedProfile == null)
            {
                return;
            }

            if (Bot.Player.QuestlogEntries != null)
                foreach (var entry in Bot.Player.QuestlogEntries)
                {
                    var finished = entry.Finished == 1;
                    var quest = GetDbQuest(entry.Id).Result;
                    quest.Accepted = true;
                    
                    if(SelectedProfile.Quests.Any(q=>q.Any(t=>t.Id == quest.Id)) == false)
                    {
                        SelectedProfile.Quests.Enqueue(new[] {quest});
                    }
                }

            if (SelectedProfile.Quests.Count > 0)
            {
                IEnumerable<IBotQuest> selectedQuests = SelectedProfile.Quests.Peek().Where(e => !e.Returned && (!CompletedQuests.Where(t => t > 0).Contains(e.Id)));

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
                                if (notReturnedQuest.CompleteQuest())
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
                    CompletedQuests.AddRange(SelectedProfile.Quests.Dequeue().Select(e => e.Id));
                    return;
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
                }
                else
                {
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
                                        WowBasicItem item = ItemFactory.BuildSpecificItem(ItemFactory.ParseItem(itemJson));

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
                        quest.Returned = true;
                        return true;
                    }

                    ActionToggle = !ActionToggle;
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
            if (obj.GetType() == typeof(IWowGameobject))
            {
                Bot.Wow.InteractWithObject(obj);
            }
            else if (obj.GetType() == typeof(IWowUnit))
            {
                Bot.Wow.InteractWithUnit((IWowUnit)obj);
            }
        }

        public async Task<IBotQuest> GetDbQuest(int questId)
        {
            var dbQuest = await World.Quests.SingleOrDefaultAsync(t => t.Id == questId);

            var objectives = await World.QuestObjectives.Where(t => t.QuestId == questId).ToArrayAsync();
            var questObjectives = new List<IQuestObjective>();
            foreach (var objective in objectives)
            {
                var qo = new BotQuestObjective()
                {
                    DbQuestObjective = objective
                };

                qo.Action = () =>
                {
                    var target = Bot.GetClosestGameObjectByDisplayId(Bot.Player.Position, new[] { objective.ObjectId });
                    if (target != null)
                    {
                        Bot.Movement.SetMovementAction(Core.Engines.Movement.Enums.MovementAction.Move, target.Position);
                    }
                };

                questObjectives.Add(qo);
            }

            return new BotQuest()
            {
                Id = dbQuest.Id,
                Name = dbQuest.Title,
                Objectives = questObjectives
            };
        }

        public void Dispose()
        {
            
        }
    }
}