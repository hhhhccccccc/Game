using System.Collections.Generic;

public static class GameConst
{
    public const string AssemblyNameForController = "Game.Controller";
    public const string AssemblyNameForModel = "Game.Model";
    public const string AssemblyNameForView = "Game.View";
    public const string AssemblyNameForManager = "Game.Manager";
    public const string AssemblyNameForConfig = "Game.Config";
    public const string AssemblyNameForInterface = "Game.Interface";
    public const string AssemblyNameForMessage = "Game.Message";

    public const int ReferenceResolutionX = 1920;
    public const int ReferenceResolutionY = 1080;
    public const int MatchWidthOrHeight = 1;
    
    public static class Battle
    {
        public static int KeyMax = 10;
        public static float CalculateSpeedOffset = 0.1f;
        public static int CalculateActionWheelNormal = 5;//默认计算为5息
        public static int ImmunityCounterBuffID = 30421;
        public static int ShieldBuffID = 80001;
        public static int ArmorBuffID = 81001;
        public static int CounterBuffID = 99999;
        public static int MaxRandomCount = 3;//最大随机次数为3 超过3跳出去
        /// <summary>
        /// ↑类留劲buffID
        /// </summary>
        public static List<int> BuffUpFirstSkillList = new()
        {
            76901,77301,77701,78101
        };
        /// <summary>
        /// ↓类留劲buffID
        /// </summary>
        public static List<int> BuffDownFirstSkillList = new()
        {
            77101,77501,77901,78301
        };
        /// <summary>
        /// ←类留劲buffID
        /// </summary>
        public static List<int> BuffLeftFirstSkillList = new()
        {
            77201,77601,78001,78401
        };
        /// <summary>
        /// →类留劲buffID
        /// </summary>
        public static List<int> BuffRightFirstSkillList = new()
        {
            77001,77401,77801,78201
        };
        /// <summary>
        /// 化身类buff
        /// </summary>
        public static List<int> BuffAvatarList = new()
        {
            30371,30381,30391
        };
    }
    
    public static class View
    {
        public static string SceneRoot = "Assets/GameResource/Prefab/Scene/Scene/";
        public static string PasserbyRoot = "Assets/GameResource/Prefab/Scene/Passerby/";
    }
}