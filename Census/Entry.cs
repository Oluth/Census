using ICities;

namespace Census
{

    /// <summary>
    /// This is the entry point of <b>Census</b> once it's loaded in game. 
    /// </summary>
    public class Entry : IUserMod
    {
        string IUserMod.Name => "Census";

        string IUserMod.Description => "[Pre-Alpha] A Cities: Skylines demography tool.";
    }
}
