namespace _project.Scripts.Services.LevelManagement
{
    public interface IProgressStorage
    {
        void Save(LevelProgressData data);
        LevelProgressData Load();
    }   
}