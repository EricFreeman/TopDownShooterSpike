using System.Collections.Generic;
using Krypton.Core;
using Krypton.Lights;

namespace Krypton
{
    public abstract class LightingEngine
    {
        public abstract void RenderLightmap(IList<ILitObject> litObjects);
    }

    public interface ILitObject
    {
        ShadowType ShadowType { get; set; }
        IList<ShadowHull> ShadowHulls  { get; }
    }
}