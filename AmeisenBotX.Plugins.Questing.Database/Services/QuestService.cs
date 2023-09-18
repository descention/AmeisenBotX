using AmeisenBotX.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Services
{
    internal class QuestService
    {
        private readonly IServiceProvider serviceProvider;

        public QuestService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Vector3 GetQuestStartLocation(int questId) => throw new NotImplementedException();
        public Vector3 GetQuestEndLocation(int questId) => throw new NotImplementedException();
        public IEnumerable<Vector3> GetQuestObjectiveLocations(int questId, int objectiveIndex) => throw new NotImplementedException();

    }
}
