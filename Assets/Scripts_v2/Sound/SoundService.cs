namespace UnderwaterCats
{
    public enum Sound 
    {
        Click,
        Menu,
        Start,
        Brif,
        Level,
        Win,
        Lost
    }
    
    public interface SoundService
    {
        public void PlaySound(Sound sound);

        public void StopSound(Sound sound);
    }
}