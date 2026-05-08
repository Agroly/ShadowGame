using _project.Scripts.Services.GameManagement;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _project.Scripts.LevelManagement
{
    public class LevelInitializer
    {
        private LevelsDatabase _levelsDatabase;
        private GameManager _gameManager;
        
        [Inject]
        public void Construct(LevelsDatabase levelsDatabase, GameManager gameManager)
        {
            _levelsDatabase = levelsDatabase;
            _gameManager = gameManager;
        }
        public async UniTask StartLevel(string levelId)
        {
            var level = _levelsDatabase.GetLevelById(levelId);
            await _gameManager.StartGameplay(level);
        }
    }
}