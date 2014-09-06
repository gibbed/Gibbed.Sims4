using System;

namespace Gibbed.Sims4.FileFormats
{
    [Flags]
    public enum PartFlags : byte
    {
        DefaultForBodyType = 1 << 0,
        DefaultThumbnailPart = 1 << 1,
        AllowForRandom = 1 << 2,
        ShowInUI = 1 << 3,
        ShowInSimInfoPanel = 1 << 4,
        ShowInCASDemo = 1 << 5,
    }
}
