using System;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [Serializable]
    public enum VersionType : byte
    {
        Major = 0,
        Feature = 1,
        Bugfix = 2,
        Build = 3,
    }
}